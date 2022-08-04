import { Injectable } from '@angular/core';
import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LocalService } from '../common/local.service';
import { LoggerService } from '../common/logger.service';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class ExpensemanageruserService {

  private readonly url: string; 

  constructor(
    private http: HttpClient,
    private localStore: LocalService,
    private logger: LoggerService) { 
    this.url = environment.apiServer.baseUrl + environment.apiServer.version + environment.apiServer.userAPI;
    this.logger.Info(`User Endpoint: ${this.url}`);
  }  

  getAllUsers(){    
    let usersUrl = this.url + '/GetAll';
    return this.http.get<any>(usersUrl);
  }
}
