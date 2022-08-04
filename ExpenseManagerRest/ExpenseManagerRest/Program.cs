using ExpenseManagerRest.ActionFilters;
using ExpenseManagerRest.Controllers;
using Expenses.DAL.Models;
using Expenses.DAL.Repo;
using Expenses.Domain.Model;
using Expenses.Domain.Model.Converters;
using Expenses.Domain.Repo.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using GE = GoldenEagles.Logger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region CORS
// Default Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AngularOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});
#endregion

#region JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
#endregion

#region Database
builder.Services.AddDbContext<ExpenseManagerContext>(
    options => options    
    .UseLazyLoadingProxies(true)
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))    
    );
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //This is required to enable Postgres timestamtz storage
#endregion

#region Logger
builder.Services.AddSingleton((IServiceProvider arg) => "ExpenseManager");
builder.Services.AddSingleton<GE.ILogger, GE.Serilog_Logger>();
//builder.Services.AddSingleton<GE.ILogger, GE.Serilog_Logger<UserController>>();
//builder.Services.AddSingleton<GE.ILogger, GE.Serilog_Logger_Alt>();
//builder.Services.AddSingleton<GE.ILogger, GE.MS_ConsoleLogger>();
#endregion

#region model validation using data annotation
builder.Services.AddScoped<ValidationFilterAttribute>(); //Action Filter to handle model validation globally

//Below statement needed to suppress default model state validation behavior (i.e. 400 Bad Request) of APIController & instead send 422 Unprocessable Entity Status code.
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
#endregion

//To Do: Conditional Injection i.e. Example: different logger objects to be injected to different classes
//To Do: Conditional Injection i.e. Example: different string objects to be injected to different classes
//To Do: INFO should NOT be logged in Error Log File

builder.Services.Configure<JsonSerializerOptions>(options => options.Converters.Add(new DateOnlyConverter()));

#region Expense Manager Project Classes
builder.Services.AddScoped<IUserStatusDomainRepository, UserStatusDomainRepository>();
builder.Services.AddScoped<IRecurringintervaltypeDomainRepository, RecurringintervaltypeDomainRepository>();
builder.Services.AddScoped<IMonthlypaymentstatusDomainRepository, MonthlypaymentstatusDomainRepository>();
builder.Services.AddScoped<IExpensepaymentstatusDomainRepository, ExpensepaymentstatusDomainRepository>();
builder.Services.AddScoped<IUserDomainRepository, UserDomainRepository>();
builder.Services.AddScoped<IExpenseTypeDomainRepository, ExpenseTypeDomainRepository>();
builder.Services.AddScoped<IMonthlyExpenseDomainRepository, MonthlyExpenseDomainRepository>();
builder.Services.AddScoped<IExpenseEntryDomainRepository, ExpenseEntryDomainRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(AutoMappingProfile));
#endregion



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AngularOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


