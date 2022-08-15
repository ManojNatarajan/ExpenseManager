import { Injectable } from '@angular/core';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LocalService } from '../common/local.service';
import { LoggerService } from '../common/logger.service';
import { ExpenseSummary } from '../models/expensesummary';
import { ExpenseSummaryAdapter } from '../models/expensesummaryadapter';
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { ExpenseEntryContract } from '../models/expenseentrycontract';
import { ExpenseEntry } from '../models/expenseentry';

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

  getExpenseEntryListMonthYearJson(userId: number, month:number, year:number){
    let expenseEntryUrl = this.url + `/GetExpenseEntriesForMonth/${userId}/${month}/${year}`;    
    this.logger.debug(expenseEntryUrl);    
    return this.http.get(expenseEntryUrl);
  }
  getExpenseEntryListMonthYear(userId: number, month:number, year:number):Observable<ExpenseEntry[]>{
    let expenseEntryUrl = this.url + `/GetExpenseEntriesForMonth/${userId}/${month}/${year}`;    
    this.logger.debug(expenseEntryUrl);    
    return this.http.get<any[]>(expenseEntryUrl)
    .pipe(map((data: any[]) => data.map(item => new ExpenseEntry(item))));
  }
  
  getExpenseSummaryListAlt(userId: number): Observable<ExpenseSummary[]>{
    let expenseSummaryUrl = this.url + `/ExpenseSummaryList/${userId}`;    
    this.logger.debug(expenseSummaryUrl);    
    return this.http.get<any[]>(expenseSummaryUrl)
    .pipe(map((data: any[]) => data.map(item => this.expSummaryAdapter.adapt(item))));
  }

  addExpenseEntry(expenseEntryContract: ExpenseEntryContract){
    let expenseSummaryUrl = this.url + `/AddExpenseEntry`;   
    let requestBody = JSON.parse(JSON.stringify(expenseEntryContract)); 
    this.logger.debug(expenseSummaryUrl);    
    return this.http.post(expenseSummaryUrl, requestBody, { responseType: 'text' });
  }

  updateExpenseEntry(expenseEntryContract: ExpenseEntryContract){
    let expenseSummaryUrl = this.url + `/UpdateExpenseEntry`;   
    let requestBody = JSON.parse(JSON.stringify(expenseEntryContract)); 
    this.logger.debug(expenseSummaryUrl);    
    return this.http.put(expenseSummaryUrl, requestBody, { responseType: 'text' });
  }

  deleteExpenseEntry(e:ExpenseEntry){
    let deleteExpenseEntryUrl = this.url + '/DeleteExpenseEntry/' + e.Id;
    this.logger.debug(`deleteExpenseTypeUrl: ${deleteExpenseEntryUrl}`);
    return this.http.delete(deleteExpenseEntryUrl, { responseType: 'text' });
  }

}
