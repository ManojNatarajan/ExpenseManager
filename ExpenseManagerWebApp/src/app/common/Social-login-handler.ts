import {Subscription} from 'rxjs';
import { Injectable } from '@angular/core';
import { SocialAuthService, SocialUser, GoogleLoginProvider, FacebookLoginProvider } from '@abacritt/angularx-social-login';
import { SocialUserDTO } from 'src/app/models/SocialUser';
import { AlertService } from 'src/app/common/alert.service';
import { LocalService } from 'src/app/common/local.service';
import { LoggerService } from 'src/app/common/logger.service';
import { ExpensemanagerauthService } from '../services/expensemanagerauth.service';
import { Router } from '@angular/router';
import { LoginService } from '../services/loginService.service';

@Injectable({
  providedIn: 'root'
})
export class SocialLoginHandler{

    private userSubscription:Subscription;
    user: SocialUser ; 
    socialUserLoggedIn: boolean;
    accessToken: any = null;

    constructor(
        private authService: SocialAuthService,
        private localStore: LocalService,
        private logger: LoggerService,
        private alert: AlertService,
        private auth: ExpensemanagerauthService,
        private router: Router,
        private loginService: LoginService
        ){}

    

    subscribeSocialUser(){
    if (this.authService.authState )
    {
        this.userSubscription = this.authService.authState.subscribe({
        next: (user:SocialUser) => {
          this.logger.Info("next method for authservice");
          this.user = user;
          console.log(user);
          this.socialUserLoggedIn = (user != null);
          if (user !=null && this.socialUserLoggedIn)
          {
              let socialUser = this.user as SocialUserDTO;
              socialUser.authToken = this.user.idToken ? this.user.idToken : this.user.authToken;
              this.auth.SignInUsingSocialUser(socialUser).subscribe({
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
                complete: () => this.logger.Info('Sign in with social user complete.')
              });
          }
        },
        error: (e) => {
        this.logger.Unknown(e);
        },
        complete: () =>{
          this.logger.Info("Complete method for authservice");
          this.alert.success(JSON.stringify(this.user));
        }
      });
    }
  }

  unsubscribeSocialUser(){        
    if(this.userSubscription)
    {
        this.authService.signOut();
        this.userSubscription.unsubscribe();
    }
  }

refreshGoogleToken(): void {
    this.authService.refreshAuthToken(GoogleLoginProvider.PROVIDER_ID);
  }
  refreshGoogleAccessToken(): void {
    this.authService.refreshAccessToken(GoogleLoginProvider.PROVIDER_ID);
  }
  getGoogleAccessToken(): void {
    this.authService.getAccessToken(GoogleLoginProvider.PROVIDER_ID).then(accessToken => this.accessToken = accessToken);
  }
 
  signInWithFacebook(): void {
    this.authService.signIn(FacebookLoginProvider.PROVIDER_ID).then(data => {
      console.log(data);
    });
  }
  refreshFacebookToken(): void {
    this.authService.refreshAuthToken(FacebookLoginProvider.PROVIDER_ID);
  }
  refreshFacebookAccessToken(): void {
    this.authService.refreshAccessToken(FacebookLoginProvider.PROVIDER_ID);
  }
  getFacebookAccessToken(): void {
    this.authService.getAccessToken(FacebookLoginProvider.PROVIDER_ID).then(accessToken => this.accessToken = accessToken);
  }

  signInWithGoogle(): void {
    this.authService.signIn(GoogleLoginProvider.PROVIDER_ID).then(data => {
      console.log(data);
    });
  }

  signOut(): void {
    this.authService.signOut();
  }  

}