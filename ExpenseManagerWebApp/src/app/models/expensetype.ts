import { ModelHelper } from "./modelhelper";

export class ExpenseType{
   

    constructor();
    constructor(item: any);
    constructor(item?: any){
        if(item){
            let helper = new ModelHelper();

            this.Id = item.id;
            this.UserId = item.userId;
            this.Description = item.description;
            this.DefaultDueAmount = item.defaultdueamount;
            this.DefaultDueDateInMonth = item.defaultduedateinmonth;
            this.IsRecurring = item.isrecurring;
            this.RecurringIntervalTypeID = item.recurringintervaltypeid;
            this.IsActive = item.isActive;
            this.CreatedDate =  new Date(item.createddate);
            this.ModifiedDate = new Date(item.modifieddate);

            if(this.IsRecurring === true)
            {
                this.IsRecurringStr = "YES";
                this.RecurringIntervalTypeStr = helper.getExpenseTypeRecurringIntervalText(this.RecurringIntervalTypeID);
            }
            else
            {
                this.IsRecurringStr = "NO";
                this.RecurringIntervalTypeStr = "N/A";
            }

            if(this.IsActive === true)
            {
                this.StatusText = "Active";
            }
            else
            {
                this.StatusText = "Inactive";
            }
        }
    }

    Id: number;
    UserId: number;
    Description: string;
    DefaultDueAmount: number;
    DefaultDueDateInMonth: number;
    IsRecurring: boolean;
    RecurringIntervalTypeID: number;
    IsActive: boolean;    
    CreatedDate: Date;
    ModifiedDate: Date;

    IsRecurringStr: string;
    RecurringIntervalTypeStr: string;
    StatusText: string;
}