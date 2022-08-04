import { RecurringIntervalType } from "./RecurringIntervalType";


export interface dictMonth {
    [key: number]: string;
  }

export interface dictSummaryPaytStatus {
    [key: number]: string;
  }

export interface dictExpenseEntryPaytStatus {
    [key: number]: string;
  }

export class EnumHelper{
    mon: dictMonth = {};
    paytStatus: dictSummaryPaytStatus = {};
    eePaytStatus: dictExpenseEntryPaytStatus = {};

    getMonthDictionary(){
        this.mon[1] = "JAN";
        this.mon[2] = "FEB";
        this.mon[3] = "MAR";
        this.mon[4] = "APR";
        this.mon[5] = "MAY";
        this.mon[6] = "JUN";
        this.mon[7] = "JUL";
        this.mon[8] = "AUG";
        this.mon[9] = "SEP";
        this.mon[10] = "OCT";
        this.mon[11] = "NOV";
        this.mon[12] = "DEC";
        return this.mon;
    }

    //These statuses should come from REST API.. ok for Web UI but NOT OK for Android/iOS apps.
    getSummaryPaymentStatusDictionary(){
        this.paytStatus[1] = "PAID";
        this.paytStatus[2] = "UNPAID";
        this.paytStatus[3] = "PARTIALLY PAID";
        return this.paytStatus;
    }

    //These statuses should come from REST API.. ok for Web UI but NOT OK for Android/iOS apps.
    getExpenseEntryPaymentStatusDictionary(){
        this.eePaytStatus[1] = "PAID";
        this.eePaytStatus[2] = "UNPAID";
        this.eePaytStatus[3] = "PARTIALLY PAID";
        return this.eePaytStatus;
    }

    getExpenseTypeRecurringIntervalTypes(): Array<RecurringIntervalType>{

      let types = Array<RecurringIntervalType>();
      types.push(new RecurringIntervalType(1,"Monthly"));
      types.push(new RecurringIntervalType(2,"Quarterly"));
      types.push(new RecurringIntervalType(3,"Halfyearly"));
      types.push(new RecurringIntervalType(4,"Annual"));
      return types;
  }

}