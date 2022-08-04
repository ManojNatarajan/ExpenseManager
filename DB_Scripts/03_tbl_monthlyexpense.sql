-- Table: public.MonthlyExpense

CREATE TABLE IF NOT EXISTS public.MonthlyExpense
(
    Id BIGINT GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1000) PRIMARY KEY,
    UserId bigint NOT NULL,
    BillMonth int NOT NULL,
    BillYear int NOT NULL,
    TotalAmount numeric(18,2) NOT NULL,
    PaidAmount numeric(18,2) NOT NULL,
    DueAmount numeric(18,2) NOT NULL,
    MonthlyPaymentStatusID int NOT NULL,
    AdditionalRemarks  varchar(300),
    CreatedDate timestamptz NOT NULL DEFAULT NOW(),
    ModifiedDate timestamptz,
    CONSTRAINT fk_monthlyexpense_userid_users_id FOREIGN KEY (UserId)
        REFERENCES public.Users(Id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    UNIQUE(UserId,BillMonth,BillYear)
)
TABLESPACE pg_default;
ALTER TABLE IF EXISTS public.MonthlyExpense OWNER to postgres;
