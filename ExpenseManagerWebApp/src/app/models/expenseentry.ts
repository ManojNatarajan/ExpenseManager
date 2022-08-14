import { ExpenseType } from "./expensetype";
import { ModelHelper } from "./modelhelper";

export class ExpenseEntry{    

    helper: ModelHelper = new ModelHelper();

    constructor();
    constructor(item: any);
    constructor(item?: any){
        if(item){
            this.Id = item.id;
            this.MonthlyExpenseId = item.monthlyExpenseId;
            this.ExpenseTypeId = item.expenseTypeId;
            this.Duedate = new Date(item.duedate);
            this.Dueamount = item.dueamount;
            this.Paymentamount = item.paymentamount;
            this.Issplittedpayment = item.issplittedpayment ;
            this.Paymentdate = new Date(item.paymentdate);
            this.ExpensepaymentstatusId = item.expensepaymentstatusid;
            this.AdditionalRemarks =  item.additionalremarks ?  item.additionalremarks : 'N/A';
            this.Createddate = new Date(item.createddate);
            this.Modifieddate = new Date(item.modifieddate);
            this.ExpensePaymentStatusText = this.helper.getExpenseEntryPaymentStatusText(item.expensepaymentstatusid);
            this.ExpenseType = new ExpenseType(item.expenseType);
        }
    }
 
    Id: number;
    MonthlyExpenseId: number;
    ExpenseTypeId: number;
    Duedate: Date;
    Dueamount: number;
    Paymentamount: number;
    Issplittedpayment: boolean;
    Paymentdate: Date;
    ExpensepaymentstatusId: number;    
    AdditionalRemarks: string;
    Createddate: Date;
    Modifieddate: Date;

    UserId: number;    
    BillPeriod: string;
    ExpensePaymentStatusText: string;

    ExpenseType : ExpenseType;
}