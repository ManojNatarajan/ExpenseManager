select * from public.Users;
select * from public.ExpenseType;
select * from public.MonthlyExpense;
select * from public.ExpenseEntry;

--Master Tables
select * from userstatus;
select * from RecurringIntervalType; 
select * from MonthlyPaymentStatus; 
select * from ExpensePaymentStatus; 

/*
DROP TABLE IF EXISTS public.ExpenseEntry;
DROP TABLE IF EXISTS public.MonthlyExpense;
DROP TABLE IF EXISTS public.ExpenseType;
DROP TABLE IF EXISTS public.Users;

truncate table public.expenseentry; 
delete from public.monthlyexpense;
*/