//import { ThisReceiver } from '@angular/compiler';

import { AnyCatcher } from "rxjs/internal/AnyCatcher";
import { EnumHelper, dictMonth, dictSummaryPaytStatus, dictExpenseEntryPaytStatus } 
from "./enumhelper";
import { RecurringIntervalType } from "./RecurringIntervalType";

export class ModelHelper{

    private enumHelper: EnumHelper = new EnumHelper();
    private mon: dictMonth = {};
    private paytStatus: dictSummaryPaytStatus = {};
    private expenseEntryPaytStatus: dictExpenseEntryPaytStatus = {};
    private expenseRecurringIntervalTypes: Array<RecurringIntervalType>;
    
    constructor(){
        this.mon = this.enumHelper.getMonthDictionary();
        this.paytStatus = this.enumHelper.getSummaryPaymentStatusDictionary();        
        this.expenseEntryPaytStatus = this.enumHelper.getExpenseEntryPaymentStatusDictionary();
        this.expenseRecurringIntervalTypes = this.enumHelper.getExpenseTypeRecurringIntervalTypes();
    }

    getBillPeriod(billMonth: number, billYear: number): string{
        let month = this.mon[billMonth];
        let billPeriod = month + ', ' + billYear;
        return billPeriod
    }

    getSummaryPaymentStatusText(summaryStatusId: number): string{
        return this.paytStatus[summaryStatusId];
    }

    getExpenseEntryPaymentStatusText(expenseEntryStatusId: number): string{
        return this.expenseEntryPaytStatus[expenseEntryStatusId];
    }

    getExpenseTypeRecurringIntervalTypes(): Array<RecurringIntervalType>{
        return this.expenseRecurringIntervalTypes;
    }

    getExpenseTypeRecurringIntervalText(recurringIntID: number): string{
        for(var index in this.expenseRecurringIntervalTypes)
        { 
            if(this.expenseRecurringIntervalTypes[index].Id === recurringIntID)
                return  this.expenseRecurringIntervalTypes[index].Description;
        }
        return "";
    }
}