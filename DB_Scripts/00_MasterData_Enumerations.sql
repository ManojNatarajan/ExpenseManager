/*
Master Enumerations 
1. UserStatus -- Used for users
2. RecurringIntervalType -- Used when defining Expense Type
3. MonthlyPaymentStatus -- Used in MonthlyExpense
4. ExpensePaymentStatus        -- Used in ExpenseEntry
*/
---------------------------------------------------------------
create table UserStatus(
Id INT GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1) PRIMARY KEY,
Description varchar(100) not null
);
insert into UserStatus(Description) values('Active');
insert into UserStatus(Description) values('Inactive');
insert into UserStatus(Description) values('Dormant'); --Users whose last login beyond 12 months. 
--select * from userstatus; 
---------------------------------------------------------------
create table RecurringIntervalType(
Id INT GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1) PRIMARY KEY,
Description varchar(100) not null
);
insert into RecurringIntervalType(Description) values('Monthly');
insert into RecurringIntervalType(Description) values('Quarterly');
insert into RecurringIntervalType(Description) values('Halfyearly'); 
insert into RecurringIntervalType(Description) values('Annual'); 
--select * from RecurringIntervalType; 
---------------------------------------------------------------
create table MonthlyPaymentStatus(
Id INT GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1) PRIMARY KEY,
Description varchar(100) not null
);
insert into MonthlyPaymentStatus(Description) values('Paid');
insert into MonthlyPaymentStatus(Description) values('Unpaid');
insert into MonthlyPaymentStatus(Description) values('PartiallyPaid');  
--select * from MonthlyPaymentStatus; 
---------------------------------------------------------------
create table ExpensePaymentStatus(
Id INT GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1) PRIMARY KEY,
Description varchar(100) not null
);
insert into ExpensePaymentStatus(Description) values('Paid');
insert into ExpensePaymentStatus(Description) values('Unpaid');
insert into ExpensePaymentStatus(Description) values('PartiallyPaid');  
--select * from ExpensePaymentStatus; 
---------------------------------------------------------------