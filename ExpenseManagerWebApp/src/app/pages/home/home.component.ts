import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { LoggerService } from 'src/app/common/logger.service';
import { ExpenseSummary } from 'src/app/models/expensesummary';
import { ExpensemanagerresourceService } from 'src/app/services/expensemanagerresource.service';
import { TokenHelper } from 'src/app/common/token-helper';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  expenseSummary: ExpenseSummary[];
  isSummaryFetchComplete:boolean = false;
  userId: number;

  constructor(
    private ref: ChangeDetectorRef,
    private logger: LoggerService,
    private resourceAPI: ExpensemanagerresourceService,
    private tokenHelper: TokenHelper
    ) { 
  }
  
  ngOnInit(): void {
      this.userId = this.tokenHelper.getUserIdClaim();
      this.logger.debug(`Loggedin UserID: ${this.userId}`);
      this.getExpenseSummaryList();
  }

  async getExpenseSummaryList(){
    this.isSummaryFetchComplete = false;
    await this.delay(1000); //delay is used to test UI progress bar. This is to be commented before going live.

    this.resourceAPI.getExpenseSummaryListAlt(this.userId)   
      .subscribe({
      next: (mSummaryList) => {        
        this.expenseSummary = mSummaryList;             
      },
      error: (e) => {
        this.expenseSummary = [];
        this.isSummaryFetchComplete = true;
        this.logger.debug('Error Occurred in getExpenseSummaryList'); 
        this.logger.debugUnknown(e);
        this.ref.detectChanges();
      },
      complete: () => {
        this.isSummaryFetchComplete = true;        
        this.logger.debug('Home Component: getExpenseSummaryList: Complete!');
        this.ref.detectChanges();   
      }
  });  

  }

  private delay(ms: number)
  {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}
