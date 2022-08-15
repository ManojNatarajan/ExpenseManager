import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import { environment } from "./../../environments/environment";
import { LocalService } from '../common/local.service';
import { LoggerService } from '../common/logger.service';
import { User } from '../models/user';
import { SocialUserDTO } from '../models/SocialUser';
import { LoginService } from './loginService.service';

@Injectable({
  providedIn: 'root'
})
export class ExpensemanagerauthService {

  private readonly url: string; 

  constructor(
    private http: HttpClient,
    private localStore: LocalService,
    private logger: LoggerService,
    private loginService: LoginService
    ) {
    this.url = environment.apiServer.baseUrl + environment.apiServer.version + environment.apiServer.authAPI;
    console.log(this.url);
  }

  signupUser(user: User){
    let signupUserUrl = this.url + '/signup';
    let requestBody = JSON.parse(JSON.stringify(user));
    this.logger.debug(signupUserUrl);
    this.logger.debugTable(requestBody);
    return this.http.post(signupUserUrl, requestBody, { responseType: 'text' });
  }

  SignInUsingMobileAndPassword(mob: number, pwd: string){

    let signinUserUrl = this.url + '/signInUsingMobileAndPassword';
    this.logger.debug(signinUserUrl);
    //let requestBody: any = {mobile: 9944556306,password: '123123'};
    let requestBody = { mobile: mob, password:  pwd}
    this.logger.debugTable(requestBody);
    return this.http.post(signinUserUrl, requestBody, { responseType: 'text' });     
  }

  SignInUsingSocialUser(user: SocialUserDTO){
    let signinUserUrl = this.url + '/signinwithsocialuser';
    this.logger.debug(signinUserUrl);
    return this.http.post(signinUserUrl, user,
      {
        headers: { Authorization: `Bearer ${user.authToken}` },
        responseType: 'text'
      });
  }

  SignOut()
  {
    this.loginService.userLoggedIn = false;
    this.localStore.removeData('jwt');
  }
}
