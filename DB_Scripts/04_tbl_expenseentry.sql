-- Table: public.ExpenseEntry

CREATE TABLE IF NOT EXISTS public.ExpenseEntry
(
    Id BIGINT GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1000) PRIMARY KEY,
    MonthlyExpenseId bigint NOT NULL,
    ExpenseTypeId bigint NOT NULL,    
    DueDate date NOT NULL,
    DueAmount numeric(18,2) NOT NULL,    
    PaymentAmount numeric(18,2) NOT NULL,
    PaymentDate timestamptz,
    ExpensePaymentStatusID int NOT NULL,
    AdditionalRemarks  varchar(300),
    IsDeleted boolean,
    CreatedDate timestamptz NOT NULL DEFAULT NOW(),
    ModifiedDate timestamptz,
    CONSTRAINT fk_expenseentry_exptypid_expensetype_id FOREIGN KEY (ExpenseTypeId)
        REFERENCES public.ExpenseType(Id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_expenseentry_monthlyexpid_monthlyexpense_id FOREIGN KEY (MonthlyExpenseId)
        REFERENCES public.MonthlyExpense(Id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
TABLESPACE pg_default;
ALTER TABLE IF EXISTS public.ExpenseEntry OWNER to postgres;

