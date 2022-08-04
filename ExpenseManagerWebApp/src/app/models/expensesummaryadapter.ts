import { Injectable } from "@angular/core";
import { ModelAdapter } from "../models/modeladapter";
import { ExpenseSummary } from "./expensesummary";

@Injectable({
providedIn: "root",
})
export class ExpenseSummaryAdapter implements ModelAdapter<ExpenseSummary> {
    adapt(item: any): ExpenseSummary {
        return new ExpenseSummary(item);
    }
}