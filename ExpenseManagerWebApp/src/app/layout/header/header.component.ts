import { Component, OnInit } from '@angular/core';
import { ExpensemanagerauthService } from 'src/app/services/expensemanagerauth.service';
import { Router } from '@angular/router';
import { LocalService } from 'src/app/common/local.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoggerService } from 'src/app/common/logger.service';
import { AlertService } from 'src/app/common/alert.service';
import { SocialLoginHandler } from 'src/app/common/Social-login-handler';
import { LoginService } from 'src/app/services/loginService.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  readonly userLoggedIn: boolean;

  constructor(
    private auth: ExpensemanagerauthService,
    private router: Router,
    private alert: AlertService,
    private localStore: LocalService,
    private jwtHelper: JwtHelperService,
    private logger: LoggerService,
    private socialHandler: SocialLoginHandler,
    protected loginService: LoginService
  ) { 
      this.loginService.userLoggedIn =this.loginService.userLoggedIn || this.isUserLoggedIn();
  }

  ngOnInit(): void {
  }

  isUserLoggedIn(){
    const token  = this.localStore.getData('jwt');
    if(token && !this.jwtHelper.isTokenExpired(token)){
      this.logger.Info(`isUserLoggedIn ${token}`);
      return true;
    }
    else{
      this.logger.Info(`isUserLoggedIn: False`);
      return false;
    }
  }

  async handleSignOut() {    
    this.auth.SignOut();
    this.socialHandler.unsubscribeSocialUser();
    this.router.navigateByUrl('/signin');
    this.alert.success('Login again to continue!');
  }

}
