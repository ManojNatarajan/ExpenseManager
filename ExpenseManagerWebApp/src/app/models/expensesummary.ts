import { ExpenseEntry } from "./expenseentry";
import { ModelHelper } from "./modelhelper";

export class ExpenseSummary
{
    helper: ModelHelper = new ModelHelper();

    constructor();
    constructor(item: any);
    constructor(item?: any){
            if(item){
                this.Id = item.id;
                this.UserId = item.userId;
                this.Billmonth = item.billmonth;
                this.Billyear = item.billyear;
                this.TotalAmount = item.totalAmount;
                this.PaidAmount = item.paidAmount;
                this.DueAmount = item.dueAmount;
                this.Monthlypaymentstatusid = item.monthlypaymentstatusid;
                this.Additionalremarks =  item.additionalremarks ?  item.additionalremarks : 'N/A';
                this.Modifieddate = new Date(item.modifieddate);
                this.BillPeriod = this.helper.getBillPeriod(item.billmonth, item.billyear);
                this.MonthlyPaymentStatusText = this.helper.getSummaryPaymentStatusText(item.monthlypaymentstatusid);
                
                let ee:ExpenseEntry;
                if(item.expenseEntries){
                    this.ExpenseEntries = [];
                    item.expenseEntries.forEach( (e:any) => {
                        ee = new ExpenseEntry(e);
                        ee.BillPeriod = this.BillPeriod;
                        ee.UserId = this.UserId;
                        this.ExpenseEntries.push(ee);
                    });
                    //console.log(this.ExpenseEntries);
                }
            }
    }

    Id: number;
    UserId: number;
    Billmonth: number;
    Billyear: number;
    BillPeriod: string;
    TotalAmount: number;
    PaidAmount: number;
    DueAmount: number;
    Monthlypaymentstatusid: number;
    MonthlyPaymentStatusText: string;
    Additionalremarks: string;
    Modifieddate: Date;
    ExpenseEntries:ExpenseEntry[];

    IsSelected:boolean;
    
}



