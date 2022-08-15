import { Component, OnInit } from '@angular/core';
import { ExpensetypeService } from 'src/app/services/expensetype.service';
import { LoggerService } from 'src/app/common/logger.service';
import { TokenHelper } from 'src/app/common/token-helper';
import { ExpenseType } from 'src/app/models/expensetype';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/common/alert.service';
import { ModelHelper } from 'src/app/models/modelhelper';
import { RecurringIntervalType } from 'src/app/models/RecurringIntervalType';
import { faTrashAlt, faPencil, faToggleOff, faToggleOn } from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-expensetypeconfig',
  templateUrl: './expensetypeconfig.component.html',
  styleUrls: ['./expensetypeconfig.component.css']
})
export class ExpensetypeconfigComponent implements OnInit {

  faTrashAlt = faTrashAlt;
  faPencil = faPencil;
  faToggleOff = faToggleOff;
  faToggleOn = faToggleOn;

  btnAdd: string = "Add";
  updatingRecord:number = 0;

  helper: ModelHelper = new ModelHelper();

  expenseTypeList:ExpenseType[];
  e:ExpenseType;
  userId: number;
  submitted: boolean = false;
  form: NgForm;
  selectedRecurringTypeID:number;
  recIntDisabled: boolean = true;

  recurringIntervalTypes = Array<RecurringIntervalType>();

  constructor(
    private expenseTypeService: ExpensetypeService,
    private logger: LoggerService,
    private tokenHelper: TokenHelper,
    private router: Router,
    private alert: AlertService
  ) { 
    this.recurringIntervalTypes = this.helper.getExpenseTypeRecurringIntervalTypes();
  }

  ngOnInit(): void {
    this.userId = this.tokenHelper.getUserIdClaim();
    this.logger.debug(`Loggedin UserID: ${this.userId}`);
    this.getExpenseTypesForUser();
    this.e = new ExpenseType();
  }

  get ctrl(){
    return this.form.controls;
  }

  getExpenseTypesForUser(){
    this.expenseTypeService.getExpenseTypes(this.userId)   
      .subscribe({
      next: (expenseTypes) => {        
        this.expenseTypeList = expenseTypes;
        this.logger.debugUnknown(this.expenseTypeList);           
      },
      error: (e) => {
        this.logger.debug('Error Occurred in getExpenseTypes'); 
        this.logger.debugUnknown(e);
      },
      complete: () => {
        this.logger.debug('ExpenseTypeConfig Component: getExpenseTypesForUser: Complete!');
      }
  });  
  }

  

  onSubmit(f: NgForm){
    this.submitted = true;
    this.form = f;

    if(this.e.IsRecurring && (this.selectedRecurringTypeID === undefined || this.selectedRecurringTypeID < 1)){
      this.ctrl['dropdownRecurringInterval'].setErrors({required: true});
    }

    if(!this.form.valid){
      this.alert.error("Error: Please check all input fields.");
      return;
    }

    if(this.e){

      this.e.UserId = this.userId;

      if(!this.e.IsRecurring)
      {  
        this.e.IsRecurring = false;
        this.e.RecurringIntervalTypeID = 0;
      }
      else
      {
        this.e.RecurringIntervalTypeID = this.selectedRecurringTypeID;
      }
      
      let eStr = JSON.stringify(this.e);
      this.logger.debug(eStr);
      
      if(this.btnAdd === "Add")
      {
        this.add();
      }
      else if(this.btnAdd === "Update")
      {
        this.update();
      }
      
    }
    
  }

  onUpdate(e: ExpenseType){    
    this.e = e;
    this.onRecurringExpense();
    this.selectedRecurringTypeID = this.e.RecurringIntervalTypeID;
    this.btnAdd = "Update";
    this.updatingRecord = e.Id;
  }

  onStatusChange(e:ExpenseType){
    this.e = e;
    this.e.IsActive = !e.IsActive;
    this.update();
  }

  add(){
    this.expenseTypeService.addExpenseType(this.e).subscribe({
      next: (response) => {
        this.logger.Info(`Expense Type added: ${response}`)
        this.onReset();
        this.alert.success("Expense Type added successfully.");
        this.getExpenseTypesForUser();
      },
      error: (err) => {
        if (err.status == 409){
          this.alert.error(err.error);
        }
        else{
          this.logger.UnknownWithMessage(err, 'Expense Type addition Failed with API Error.')
        }
      },
      complete: () => this.logger.Info('Expense Type addition Complete!')
    });
  }

  update(){
    let expenseTypeExistsInOtherType = this.expenseTypeList.filter(
      x => x.Id != this.e.Id && x.Description.toUpperCase() == this.e.Description.toUpperCase()
    );
    if (expenseTypeExistsInOtherType.length == 0)
    {
      this.expenseTypeService.updateExpenseType(this.e).subscribe({
        next: (response) => {
          this.logger.Info(`Expense Type updated: ${response}`)
          this.onReset();
          this.alert.success("Expense Type updated successfully.");
          this.getExpenseTypesForUser();
          this.updatingRecord = 0;
        },
        error: (err) => {
          this.logger.UnknownWithMessage(err, 'Expense Type update Failed with API Error.')
          this.onReset();
          this.updatingRecord = 0;
          this.alert.success("Sorry, Expense Type update failed.");
        },
        complete: () => this.logger.Info('Expense Type update Complete!')
      });
    }
    else{
      this.alert.error("Expense type already exist.");
    }
  }


  

  onDelete(e:ExpenseType){
    this.expenseTypeService.deleteExpense(e).subscribe(
      {
        next: (response) => {
          this.logger.Info(`Expense Type deleted: ${response}`)          
          this.alert.success("Expense Type deleted successfully.");
          this.onReset();
          this.getExpenseTypesForUser();
        },
        error: (err) => {
          this.logger.UnknownWithMessage(err, 'Expense Type delete Failed with API Error.')
        },
        complete: () => this.logger.Info('Expense Type delete Complete!')
      }
    );
  }

  onReset(){
    this.btnAdd = "Add";
    this.updatingRecord = 0;
    this.submitted = false;
    this.e = new ExpenseType();
    this.selectedRecurringTypeID = -1;
    this.onRecurringExpense();
  }

  onRecurringExpense(){
    if(this.e.IsRecurring)
      this.recIntDisabled = false;
    else
      this.recIntDisabled = true;
  }


  
}
