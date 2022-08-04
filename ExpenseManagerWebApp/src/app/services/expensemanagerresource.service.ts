import { Injectable } from '@angular/core';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LocalService } from '../common/local.service';
import { LoggerService } from '../common/logger.service';
import { ExpenseSummary } from '../models/expensesummary';
import { ExpenseSummaryAdapter } from '../models/expensesummaryadapter';
import { Observable } from "rxjs";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class ExpensemanagerresourceService {

  private readonly url: string; 

  constructor(
    private http: HttpClient,
    private localStore: LocalService,
    private logger: LoggerService,
    private expSummaryAdapter: ExpenseSummaryAdapter) { 
    this.url = environment.apiServer.baseUrl + environment.apiServer.version + environment.apiServer.resourceAPI;
    this.logger.Info(`Resource Endpoint: ${this.url}`);
  }

  
  //This is NOT recommended as this directly returns JSON result. 
  getExpenseSummaryList(userId: number){
    let expenseSummaryUrl = this.url + `/ExpenseSummaryList/${userId}`;    
    this.logger.debug(expenseSummaryUrl);    
    return this.http.get(expenseSummaryUrl);
  }
  
  getExpenseSummaryListAlt(userId: number): Observable<ExpenseSummary[]>{
    let expenseSummaryUrl = this.url + `/ExpenseSummaryList/${userId}`;    
    this.logger.debug(expenseSummaryUrl);    
    return this.http.get<any[]>(expenseSummaryUrl)
    .pipe(map((data: any[]) => data.map(item => this.expSummaryAdapter.adapt(item))));
  }



}
