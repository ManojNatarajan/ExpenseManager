import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/common/alert.service';
import { LocalService } from 'src/app/common/local.service';
import { LoggerService } from 'src/app/common/logger.service';
import { ExpensemanagerauthService } from 'src/app/services/expensemanagerauth.service';
import { faFacebookF } from '@fortawesome/free-brands-svg-icons';
import { SocialLoginHandler } from 'src/app/common/Social-login-handler';
import { LoginService } from 'src/app/services/loginService.service';



@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent implements OnInit {
  faFacebook = faFacebookF;
   

  constructor(
    private router: Router,
    private auth: ExpensemanagerauthService,
    private localStore: LocalService,
    private logger: LoggerService,
    private alert: AlertService,
    private socialLoginHandler: SocialLoginHandler,
    private loginService : LoginService
  ) { 
  }

  ngOnInit(): void {
    this.socialLoginHandler.subscribeSocialUser();
  }
 
  signInWithFacebook(){
    this.socialLoginHandler.signInWithFacebook();
  }

  onSubmit(f: NgForm){
    const {mobile, password} = f.form.value; 
    //input validation 

  try{
    this.auth.SignInUsingMobileAndPassword(mobile, password).subscribe(
      {
        next: (accessTokenRes) => {
          this.loginService.userLoggedIn = true;
          this.logger.debug(`JWT Token: ${accessTokenRes}`);
          this.localStore.saveData('jwt', accessTokenRes);
          this.router.navigateByUrl('/');
          this.alert.success("Signin Successful!");
        },
        error: (e) => {
          this.logger.ErrorWithMessage(e, "signin failed!");
          this.alert.error("Signin Failed");
        },
        complete: () => this.logger.Info('SignInUsingMobileAndPassword complete.')
      });
    }
    catch(error)
    {
      this.logger.UnknownWithMessage(error, "Error in SignInUsingMobileAndPassword!");
    }
  }

  

}
