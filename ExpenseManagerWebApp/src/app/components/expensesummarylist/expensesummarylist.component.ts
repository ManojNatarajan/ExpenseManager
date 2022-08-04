import { Component, OnInit, Input, Output, OnChanges, ChangeDetectorRef, SimpleChanges, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { LoggerService } from 'src/app/common/logger.service';
import { ExpensemanagerresourceService } from 'src/app/services/expensemanagerresource.service';
import { ExpenseSummary } from 'src/app/models/expensesummary';
import { ExpenseEntry } from 'src/app/models/expenseentry';

@Component({
  selector: 'app-expensesummarylist',
  templateUrl: './expensesummarylist.component.html',
  styleUrls: ['./expensesummarylist.component.css']
})
export class ExpensesummarylistComponent implements OnInit, OnChanges {
 
  @Input() expenseSummary:ExpenseSummary[];
  @Input() IsSummaryFetchComplete:boolean;

  //@Output('expenseSummarySelected') expenseSummaryEmitter = new EventEmitter<ExpenseEntry>();
  //To emit the selected summary back to the Home component
  //https://angular-university.io/lesson/angular-beginners-component-outputs

  selectedSummary: ExpenseSummary;
  expenseEntries: ExpenseEntry[];
  userId: number;
  expenseSummarySearchStr: string = "Loading your expenses. Please wait...";
  

  constructor(
  private logger: LoggerService,
  private resourceAPI: ExpensemanagerresourceService,
  private ref: ChangeDetectorRef
  ) {
  }
    

  ngOnInit(): void {   
  }

  ngOnChanges(changes: SimpleChanges): void {    
    this.logger.debug("ExpenseSummary Component: Change detected!");

    if(this.IsSummaryFetchComplete && this.expenseSummary && this.expenseSummary[0]){
      this.loadExpenseEntriesForSummary(this.expenseSummary[0]); //load first summary item by default 
      this.detectChanges();    
    }
    else if(this.IsSummaryFetchComplete && (!this.expenseSummary || this.expenseSummary.length == 0))
    {           
      this.logger.Info("Expense Summary not found for user!");
      this.selectedSummary = this.expenseSummary[0];
      this.expenseEntries = [];
      this.expenseSummarySearchStr = "You do not have any expenses to show. Please start by adding an expense.";
      this.detectChanges();
    }   
    
  }

  onRowClick(eSummary:ExpenseSummary){
    this.logger.debug("Expense Summary Row Click Working!");
    this.loadExpenseEntriesForSummary(eSummary);
    this.detectChanges();
  }

  loadExpenseEntriesForSummary(eSummary:ExpenseSummary){
    //Mark selected expense Summary to highlight in UI
    this.expenseSummary.forEach((e: ExpenseSummary) => {
        if(e.Id === eSummary.Id)
          e.IsSelected = true;
        else
          e.IsSelected = false;
    });

    //Load expense entries from selected expense Summary
    this.expenseEntries = [];
    eSummary.ExpenseEntries.forEach( (e:ExpenseEntry) => {
      this.expenseEntries.push(e);
    });

    this.selectedSummary = eSummary;
  }

  detectChanges(){
    this.ref.detectChanges();  //Trigger change detector to support any cascading operation in next child component. 
  }
  
}
