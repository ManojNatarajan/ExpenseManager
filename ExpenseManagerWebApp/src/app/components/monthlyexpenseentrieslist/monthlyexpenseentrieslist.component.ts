import { Component, OnInit, Input, OnChanges, SimpleChange, SimpleChanges } from '@angular/core';
import { ExpenseEntry } from 'src/app/models/expenseentry';
import { ExpenseSummary } from 'src/app/models/expensesummary';

@Component({
  selector: 'app-monthlyexpenseentrieslist',
  templateUrl: './monthlyexpenseentrieslist.component.html',
  styleUrls: ['./monthlyexpenseentrieslist.component.css']
})
export class MonthlyexpenseentrieslistComponent implements OnInit, OnChanges {


  @Input() SelectedSummary: ExpenseSummary;
  @Input() ExpenseEntries: ExpenseEntry[];

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(this.SelectedSummary){
      
    }
  }

}
