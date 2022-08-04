import { Injectable } from '@angular/core';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LocalService } from '../common/local.service';
import { LoggerService } from '../common/logger.service';
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { ExpenseType } from '../models/expensetype';

@Injectable({
  providedIn: 'root'
})
export class ExpensetypeService {

  private readonly url: string; 

  constructor(
    private http:HttpClient,
    private localStore: LocalService,
    private logger: LoggerService    
  ) { 

    this.url = environment.apiServer.baseUrl + environment.apiServer.version + environment.apiServer.expenseTypeAPI;

  }

  getExpenseTypes(userId: number): Observable<ExpenseType[]>{
    let expenseTypesUrl = this.url + `/user/${userId}`;    
    this.logger.debug(expenseTypesUrl);    
    return this.http.get<any[]>(expenseTypesUrl)
    .pipe(map((data: any[]) => data.map(item => new ExpenseType(item))));
  }

  addExpense(e:ExpenseType){
    let addExpenseUrl = this.url + '/add';
    let requestBody = JSON.parse(JSON.stringify(e));
    this.logger.debug(`addExpenseTypeUrl: ${addExpenseUrl}`);
    return this.http.post(addExpenseUrl, requestBody, { responseType: 'text' });
  }

  updateExpense(e:ExpenseType){
    let updateExpenseUrl = this.url + '/update';
    let requestBody = JSON.parse(JSON.stringify(e));
    this.logger.debug(`updateExpenseTypeUrl: ${updateExpenseUrl}`);
    return this.http.put(updateExpenseUrl, requestBody, { responseType: 'text' });
  }

  deleteExpense(e:ExpenseType){
    let deleteExpenseUrl = this.url + '/delete/' + e.Id;
    this.logger.debug(`deleteExpenseTypeUrl: ${deleteExpenseUrl}`);
    return this.http.delete(deleteExpenseUrl, { responseType: 'text' });
  }
}
