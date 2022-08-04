using ExpenseManagerRest.ActionFilters;
using Expenses.Domain.Model.Models;
using Expenses.Domain.Model.Models.API;
using Expenses.Domain.Repo.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using JWTAuth.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GE = GoldenEagles.Logger;
using ExpenseManagerRest.Models;
using Google.Apis.Auth;
using Newtonsoft.Json;

namespace ExpenseManagerRest.Controllers
{
    [EnableCors("AngularOrigin")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly GE.ILogger _logger;
        private readonly IUserDomainRepository _userDomainRepo;

        public AuthController(GE.ILogger logger, IConfiguration config, IUserDomainRepository userDomainRepo)
        {
            _logger = logger;
            _configuration = config;       
            _userDomainRepo = userDomainRepo;
        }

        [EnableCors("AngularOrigin")]
        [HttpPost]
        [Route("user/signup")]
        public IActionResult SignupNewUser(UserDTO user)
        {
            //Duplicate User/Mobile Check Validation: To Do

            DomainResponse<long> response = _userDomainRepo.AddEntity(user);
            if (response.IsSuccess)
                return Ok(response.Value);
            else
                return BadRequest(response.ErrorSummary);
        }


        [EnableCors("AngularOrigin")]
        [HttpPost]
        [Route("user/signInUsingMobileAndPassword")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AuthenticateUser(UserLoginCredential userLoginCred)
        {
            Task<UserDTO> userTask = GetUser(Convert.ToInt64(userLoginCred.Mobile), userLoginCred.Password);
            UserDTO user = await userTask;
            if (user != null)
                return Ok(GetJwtToken(user));
            else
                return BadRequest("Invalid credentials! User NOT authorized. ");
        }

        [EnableCors("AngularOrigin")]
        [HttpPost]
        [Route("user/signinwithsocialuser")]
        public async Task<IActionResult> AuthenticateSocialUser([FromBody] SocialUser socialUser)
        {
            try
            {
                UserDTO? user = null;
                if (socialUser.Provider.ToUpper() == "GOOGLE")
                {
                    if (ValidateGoogleUser(socialUser))
                    {
                        user = GetUser(socialUser);
                    }
                    else
                    {
                        return BadRequest("login_failure. Invalid google token.");
                    }
                }
                else if (socialUser.Provider.ToUpper() == "FACEBOOK")
                {
                    if (ValidateFacebookUser(socialUser))
                    {
                        user = GetUser(socialUser);
                    }
                    else
                    {
                        return BadRequest("login_failure. Invalid facebook token.");
                    }

                }
                if (user != null)
                {
                    Task<UserDTO> dbUser = GetUser(user.Socialuserid);
                    UserDTO u = await dbUser;
                    if (u == null)
                    {
                        DomainResponse<long> response = _userDomainRepo.AddEntity(user);
                        user.Id = response.Value;
                    }

                    return Ok(GetJwtToken(user));
                }
                else
                {
                    return BadRequest("Authentication failed.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return BadRequest(ex.Message);
            }

        }


        private async Task<UserDTO> GetUser(long mobile, string password)
        {
            //Best to update Domain/DB Repo Methods as Async rather than using Task.FromResult 
            DomainResponse<UserDTO> response = await Task.FromResult(_userDomainRepo.AuthenticateUserByMobileAndPassword(mobile, password));
            return response?.Value;
        }

        private async Task<UserDTO> GetUser(string socialUserId)
        {
            DomainResponse<UserDTO> response = await Task.FromResult(_userDomainRepo.AuthenticateUserBySocialUserId(socialUserId));
            return response?.Value;
        }

        private UserDTO GetUser(SocialUser socialUser)
        {
            return new UserDTO()
            {
                Email = socialUser.Email,
                Firstname = socialUser.FirstName,
                Lastname = socialUser.LastName,
                UserName = socialUser.Name,
                Socialuserid = socialUser.Id,
                Socialprovider = socialUser.Provider,
                Userstatusid = 1,
                Accepttandc = true
            };
        }

        private bool ValidateGoogleUser(SocialUser u)
        {
            try
            {
                GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
                settings.Audience = new List<string>() { _configuration["Google:ClientId"] };
                GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(u.AuthToken, settings)?.Result;
                return payload != null ? true : false;
            }
            catch(Exception e)
            {
                _logger.LogError(e);
                return false;
            }
        }

        private bool ValidateFacebookUser(SocialUser u)
        {
            try
            {
                using (HttpClient httpclient = new HttpClient())
                {
                    // generate an facebook app access token
                    var appAccessTokenResponse = httpclient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["Facebook:AppId"]}&client_secret={_configuration["Facebook:AppSecret"]}&grant_type=client_credentials");
                    var appAccessTokenResp =  appAccessTokenResponse.Result;
                    var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResp);
                    // validate the user access token
                    var userAccessTokenValidationResponse = httpclient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={u.AuthToken}&access_token={appAccessToken?.AccessToken}");
                    var userAccessTokenValidationResp = userAccessTokenValidationResponse.Result;
                    var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResp);

                    bool? resp = userAccessTokenValidation?.Data?.IsValid;
                    return resp.HasValue ? resp.Value : false;
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e);
                return false;
            }
        }

        private string GetJwtToken(UserDTO user)
        {
            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("UserName", user.UserName),
                        new Claim("Mobile", user.Mobile.ToString()),
                        new Claim("Email", user.Email?.ToString())
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
