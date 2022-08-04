-- Table: public.ExpenseType

CREATE TABLE IF NOT EXISTS public.ExpenseType
(
    Id BIGINT GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1000) PRIMARY KEY,
    UserId bigint NOT NULL,
    Description varchar(100) NOT NULL,
    DefaultDueAmount numeric(18,2) NOT NULL,
    DefaultDueDateInMonth int NOT NULL,
    IsRecurring boolean,
    RecurringIntervalTypeID int,    
    IsActive boolean NOT NULL DEFAULT true,
    CreatedDate timestamptz NOT NULL DEFAULT NOW(), 
    ModifiedDate timestamptz,
    CONSTRAINT fk_expensetype_users_id FOREIGN KEY (UserId)
        REFERENCES public.Users(Id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
TABLESPACE pg_default;
ALTER TABLE IF EXISTS public.ExpenseType OWNER to postgres;
