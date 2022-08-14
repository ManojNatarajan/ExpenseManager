import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/common/alert.service';
import { NoNegativeNumbers } from 'src/app/common/custom-validators/nonegativenumber';
import { LoggerService } from 'src/app/common/logger.service';
import { TokenHelper } from 'src/app/common/token-helper';
import { dictMonth } from 'src/app/models/enumhelper';
import { ExpenseEntry } from 'src/app/models/expenseentry';
import { ExpenseEntryContract } from 'src/app/models/expenseentrycontract';
import { ExpenseSummary } from 'src/app/models/expensesummary';
import { ExpenseType } from 'src/app/models/expensetype';
import { ModelHelper } from 'src/app/models/modelhelper';
import { ExpensemanagerauthService } from 'src/app/services/expensemanagerauth.service';
import { ExpensemanagerresourceService } from 'src/app/services/expensemanagerresource.service';
import { ExpensetypeService } from 'src/app/services/expensetype.service';
import { faTrashAlt, faPencil, faToggleOff, faToggleOn } from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-expenseentry',
  templateUrl: './expenseentry.component.html',
  styleUrls: ['./expenseentry.component.css']
})
export class ExpenseentryComponent implements OnInit {
  faTrashAlt = faTrashAlt;
  faPencil = faPencil;
  faToggleOff = faToggleOff;
  faToggleOn = faToggleOn;

  helper: ModelHelper = new ModelHelper();
  submitted = false;
  expenseEntryForm: FormGroup;
  userId: number;
  expenseTypeList: ExpenseType[];
  expenseEntryContract: ExpenseEntryContract;
  expenseTypeSelected: ExpenseType = new ExpenseType();
  expenseEntryList: ExpenseEntry[];
  isSummaryFetchComplete: boolean = false;
  selectedYear: number;
  selectedMonth: number;
  isSplitedEntry: boolean = false;
  paymentStatusMsg: string;

  btnSubmit: string = "Submit";
  updatingRecord: number = 0;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private logger: LoggerService,
    private alert: AlertService,
    private tokenHelper: TokenHelper,
    private auth: ExpensemanagerauthService,
    private expenseManagerService: ExpensemanagerresourceService,
    private expenseTypeService: ExpensetypeService,
    private datePipe: DatePipe
  ) {
  }

  ngOnInit(): void {
    this.paymentStatusMsg = "You are paying less than total due amount. Please mark this as installment payment if you will add another payment entry in future to pay the outstanding due amount.";
    this.userId = this.tokenHelper.getUserIdClaim();
    this.logger.debug(`Loggedin UserID: ${this.userId}`);

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

    this.expenseEntryForm = this.formBuilder.group(
      {
        id: [""],
        month: ["", Validators.required],
        expenseType: ["", [Validators.required]],
        dueDate: ["", [Validators.required]],
        dueAmount: ["", [Validators.required, NoNegativeNumbers]],
        paymentDate: ["", Validators.required],
        paymentAmount: ["", [Validators.required, NoNegativeNumbers]],
        additionalRemarks: [""],
        isSplitPayment: [false],
      }
    );
    this.expenseEntryForm.controls['month'].setValue(this.datePipe.transform(new Date(), 'yyyy-MM'));
    this.expenseEntryForm.controls['paymentDate'].setValue(this.datePipe.transform(new Date(), 'yyyy-MM-dd'));
    this.getExpenseEntryList();
  }

  getExpenseEntryList() {
    this.isSummaryFetchComplete = false;
    if (!this.expenseEntryForm.controls['month'].value) {
      this.expenseEntryList = [];
      return;
    }
    let selectedMonth = this.expenseEntryForm.controls['month'].value;
    let splitted = selectedMonth.split("-");
    this.selectedYear = Number(splitted[0]);
    this.selectedMonth = Number(splitted[1]);
    this.expenseManagerService.getExpenseEntryListMonthYear(this.userId, this.selectedMonth, this.selectedYear)
      .subscribe({
        next: (expEntryList) => {
          console.log(expEntryList);
          this.expenseEntryList = expEntryList;
          this.expenseEntryList[0].ExpenseType = expEntryList[0].ExpenseType;
        },
        error: (e) => {
          this.expenseEntryList = [];
          this.isSummaryFetchComplete = true;
          this.logger.debug('Error Occurred in getExpenseSummaryList');
          this.logger.debugUnknown(e);
        },
        complete: () => {
          this.isSummaryFetchComplete = true;
          this.logger.debug('getExpenseSummaryList: Complete!');
        }
      });
  }

  changeExpenseType(target: string) {
    if (target == null || target == "others") {
      this.logger.Info(target);
    }
    else {
      let expenseType = this.expenseTypeList?.find(x => x.Id == Number(target));
      this.expenseTypeSelected = expenseType != null ? expenseType : null as any;
      if (this.expenseTypeSelected?.DefaultDueDateInMonth != undefined) {
        let dueDate = new Date(this.selectedYear, this.selectedMonth - 1, this.expenseTypeSelected?.DefaultDueDateInMonth);
        this.expenseEntryForm.controls['dueDate'].setValue(this.datePipe.transform(dueDate, 'yyyy-MM-dd'));
      }
      else {
        this.expenseEntryForm.controls['dueDate'].setValue(this.datePipe.transform(new Date(), 'yyyy-MM-dd'));
      }
      this.expenseEntryForm.controls['dueAmount'].setValue(this.expenseTypeSelected?.DefaultDueAmount);

      let splittedExpenseEntry = this.expenseEntryList?.filter(x => x.ExpenseTypeId == Number(target) && x.Issplittedpayment);
      const totalPaidAmount = splittedExpenseEntry.reduce((accumulator, expenseEntry) => {
        return accumulator + expenseEntry.Paymentamount;
      }, 0);
      if (splittedExpenseEntry.length > 0) {
        this.expenseEntryForm.controls['isSplitPayment'].setValue(true);
        this.isSplitedEntry = true;
        if (totalPaidAmount < this.expenseTypeSelected?.DefaultDueAmount)
          this.paymentStatusMsg = "You have already payed " + totalPaidAmount + " for this expense. You can pay the balance amount";
        else
          this.paymentStatusMsg = "";
      }
      else {
        this.isSplitedEntry = false;
        this.paymentStatusMsg = "You are paying less than total due amount. Please mark this as installment payment if you will add another payment entry in future to pay the outstanding due amount.";
      }
    }
  }

  onSubmit() {
    this.submitted = true;
    if (this.btnSubmit === "Submit") {
      this.addExpense();
    }
    else if (this.btnSubmit === "Update") {
      this.updateExpense();
    }
  }

  addExpense() {
    if (this.expenseEntryForm.invalid) {
      this.alert.error("Expense entry Validation Failed.")
      return;
    }
    let expenseEntry = new ExpenseEntry();
    expenseEntry.ExpenseTypeId = this.expenseEntryForm.controls['expenseType'].value;
    expenseEntry.Duedate = this.expenseEntryForm.controls['dueDate'].value;
    expenseEntry.Dueamount = this.expenseEntryForm.controls['dueAmount'].value;
    expenseEntry.Paymentdate = this.expenseEntryForm.controls['paymentDate'].value;
    expenseEntry.Paymentamount = this.expenseEntryForm.controls['paymentAmount'].value;
    expenseEntry.Issplittedpayment = this.expenseEntryForm.controls['isSplitPayment'].value != null ? this.expenseEntryForm.controls['isSplitPayment'].value : false;

    //expenseEntry.Issplittedpayment = this.expenseEntryForm.controls['paymentAmount'].value

    this.expenseEntryContract = new ExpenseEntryContract()
    this.expenseEntryContract.UserId = this.userId;
    this.expenseEntryContract.BillMonth = this.selectedMonth;
    this.expenseEntryContract.BillYear = this.selectedYear;
    this.expenseEntryContract.ExpenseEntry = expenseEntry;
    console.log(this.expenseEntryContract);
    this.expenseManagerService.addExpenseEntry(this.expenseEntryContract).subscribe({
      next: (response) => {
        this.logger.Info(`Expense entry added: ${response}`)
        this.onReset();
        this.alert.success("Expense entry added successfully.");
      },
      error: (err) => {
        this.logger.UnknownWithMessage(err, 'Expense Type addition Failed with API Error.')
      },
      complete: () => this.logger.Info('Expense Type addition Complete!')
    });
  }

  updateExpense() {
    if (this.expenseEntryForm.invalid) {
      this.alert.error("Expense entry Validation Failed.")
      return;
    }
    let expenseEntry = new ExpenseEntry();
    expenseEntry.Id = this.expenseEntryForm.controls['id'].value;
    expenseEntry.ExpenseTypeId = this.expenseEntryForm.controls['expenseType'].value;
    expenseEntry.Duedate = this.expenseEntryForm.controls['dueDate'].value;
    expenseEntry.Dueamount = this.expenseEntryForm.controls['dueAmount'].value;
    expenseEntry.Paymentdate = this.expenseEntryForm.controls['paymentDate'].value;
    expenseEntry.Paymentamount = this.expenseEntryForm.controls['paymentAmount'].value;
    expenseEntry.Issplittedpayment = this.expenseEntryForm.controls['isSplitPayment'].value != null ? this.expenseEntryForm.controls['isSplitPayment'].value : false;

    this.expenseEntryContract = new ExpenseEntryContract()
    this.expenseEntryContract.UserId = this.userId;
    this.expenseEntryContract.BillMonth = this.selectedMonth;
    this.expenseEntryContract.BillYear = this.selectedYear;
    this.expenseEntryContract.ExpenseEntry = expenseEntry;
    this.expenseManagerService.updateExpenseEntry(this.expenseEntryContract).subscribe({
      next: (response) => {
        this.logger.Info(`Expense entry added: ${response}`)
        this.onReset();
        this.alert.success("Expense entry added successfully.");
      },
      error: (err) => {
        this.logger.UnknownWithMessage(err, 'Expense Type addition Failed with API Error.')
      },
      complete: () => this.logger.Info('Expense Type addition Complete!')
    });
  }

  get ctrl() {
    return this.expenseEntryForm.controls;
  }

  onDelete(expenseEntry: ExpenseEntry) {
    this.expenseManagerService.deleteExpenseEntry(expenseEntry).subscribe(
      {
        next: (response) => {
          this.logger.Info(`Expense Entry deleted: ${response}`)
          this.alert.success("Expense Entry deleted successfully.");
          this.onReset();
        },
        error: (err) => {
          this.logger.UnknownWithMessage(err, 'Expense Entry delete Failed with API Error.')
        },
        complete: () => this.logger.Info('Expense Entry delete Complete!')
      }
    );
  }

  onEdit(expenseEntry: ExpenseEntry) {
    this.btnSubmit = "Update";
    this.expenseEntryForm.controls['id'].setValue(expenseEntry.Id)
    this.expenseEntryForm.controls['expenseType'].setValue(expenseEntry.ExpenseTypeId)
    this.expenseEntryForm.controls['dueDate'].setValue(this.datePipe.transform(expenseEntry.Duedate, 'yyyy-MM-dd'))
    this.expenseEntryForm.controls['dueAmount'].setValue(expenseEntry.Dueamount)
    this.expenseEntryForm.controls['paymentDate'].setValue(this.datePipe.transform(expenseEntry.Paymentdate, 'yyyy-MM-dd'))
    this.expenseEntryForm.controls['paymentAmount'].setValue(expenseEntry.Paymentamount)
    this.expenseEntryForm.controls['additionalRemarks'].setValue(expenseEntry.AdditionalRemarks)
    this.expenseEntryForm.controls['isSplitPayment'].setValue(expenseEntry.Issplittedpayment)
  }

  onReset() {
    this.submitted = false;
    this.expenseEntryForm.reset();
    this.expenseEntryList = [];
    this.expenseEntryForm = this.formBuilder.group(
      {
        id: [""],
        month: ["", Validators.required],
        expenseType: ["", [Validators.required]],
        dueDate: ["", [Validators.required]],
        dueAmount: ["", [Validators.required, NoNegativeNumbers]],
        paymentDate: ["", Validators.required],
        paymentAmount: ["", [Validators.required, NoNegativeNumbers]],
        additionalRemarks: [""],
        isSplitPayment: [false],
      }
    );
    this.expenseEntryForm.controls['month'].setValue(this.datePipe.transform(new Date(), 'yyyy-MM'));
    this.expenseEntryForm.controls['paymentDate'].setValue(this.datePipe.transform(new Date(), 'yyyy-MM-dd'));

    this.getExpenseEntryList();
    this.isSplitedEntry = false;
    this.paymentStatusMsg = "You are paying less than total due amount. Please mark this as installment payment if you will add another payment entry in future to pay the outstanding due amount.";
  }
}
