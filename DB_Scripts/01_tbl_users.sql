-- Table: public.Users

CREATE TABLE IF NOT EXISTS public.users
(
    Id BIGINT GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1000) PRIMARY KEY,
    Mobile bigint NULL,
    FirstName varchar(100) NOT NULL,
    LastName varchar(100) NOT NULL,
    UserName varchar(200) NOT NULL,
    Password varchar(50) NULL,
    Email varchar(200),
    UserStatusID int NOT NULL,
    SocialUserId varchar(100),
    SocialProvider varchar(50),
    IsVerified boolean NOT NULL DEFAULT false,
    AcceptTandC boolean NOT NULL,
    LastLogin timestamptz DEFAULT NOW(),
    CreatedDate timestamptz NOT NULL DEFAULT NOW(),
    UpdatedDate timestamptz
)
TABLESPACE pg_default;
ALTER TABLE IF EXISTS public.users OWNER to postgres;
