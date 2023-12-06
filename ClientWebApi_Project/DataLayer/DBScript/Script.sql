Create Database ClientApiDB

Create Table ClientMst(
Id int not null identity(1,1) PRIMARY KEY,
Fullname varchar (200) not null,
Gender Varchar(50) not null,
DOB datetime not null,
Image nvarchar(400) not null,
Username varchar(50) not null,
Password varchar(50) not null,
CTC  decimal(13,2) not null,
DocumentName  nvarchar(200) null,
Document  nvarchar(max) null,
Token  nvarchar(500) null,
TokenExpiryTime datetime null,
RefreshToken  nvarchar(200) null,
RefreshTokenExpiryTime datetime null,
IsActive bit not null,
IsDelete bit not null,
CreatedBy int not null,
CreatedOn datetime null,
UpdateBy int  null,
UpdatedOn datetime null
)

Alter Table ClientMst
  add  Joining_Date datetime  null;

  Alter Table ClientMst
  add  Probation_Period int  null;

--Create Table ClientMst(
--Id int not null identity(1,1) PRIMARY KEY,
--Fullname varchar (200) not null,
--Gender Varchar(50) not null,
--DOB datetime not null,
--Image nvarchar(400) not null,
--Username varchar(50) not null,
--Password varchar(50) not null,
--IsActive bit not null,
--IsDelete bit not null,
--CreatedBy bit not null,
--CreatedOn datetime null,
--UpdateBy bit not null,
--UpdatedOn datetime null
--)

--  ALTER TABLE ClientMst
--ALTER COLUMN CreatedBy Int;

--  ALTER TABLE ClientMst
--ALTER COLUMN UpdateBy Int;

-- alter table ClientMst
--  add  RefreshToken  nvarchar(200)

-- alter table ClientMst
--  add  RefreshTokenExpiryTime datetime 

--     alter table ClientMst
--  add  Token  nvarchar(500)

-- alter table ClientMst
--  add  TokenExpiryTime datetime 

  
--   alter table ClientMst
--  add  DocumentName  nvarchar(200)

--   alter table ClientMst
--  add  Document  nvarchar(max)

--         alter table ClientMst
--  add  CTC  decimal(13,2)


Create Table SalaryMst(
Salaryid int not null identity(1,1) PRIMARY KEY,
Id int not null,
Month int not null,
Year int not null,
Fixed_Basic_Salary decimal(13,2) not null,
Fixed_HRA decimal(13,2) not null,
Fixed_Conveyance_Allowance decimal(13,2) not null,
Fixed_Medical_Allowance decimal(13,2) not null,
Additional_HRA_Allowance decimal(13,2) not null,
Total_days int not null,
Payable_days int not null,
Gross_salary_payable decimal(13,2) not null,
Basic decimal(13,2) not null,
House_Rent_Allowance decimal(13,2) not null,
Employer_Cont_PF decimal(13,2) not null,
Conveyance_Allowance decimal(13,2) not null,
Medical_Allowance decimal(13,2) not null,
Salary_Additional_HRA_Allowance decimal(13,2) not null,
Flexible_Allowance decimal(13,2) not null,
Incentives decimal(13,2),
Total decimal(13,2) not null,
PF_one decimal(13,2) not null,
PF_two decimal(13,2) not null,
ESIC decimal(13,2) not null,
PT decimal(13,2) not null,
Advances decimal(13,2) not null,
Income_Tax decimal(13,2) not null,
Total_Ded decimal(13,2) not null,
Actual_Net_Payable decimal(13,2) not null,
IsActive bit not null,
IsDelete bit not null,
CreatedBy int not null,
CreatedOn datetime null,
UpdateBy bit not null,
UpdatedOn datetime null
)

create table LeaveMst
(
LeaveId int not null identity(1,1) PRIMARY KEY,
Id int not null,
Month int not null,
Year int not null,
Opening_Leave_Balance DECIMAL(10, 2)  not null,
Closing_Leave_Balance DECIMAL(10, 2) not null,
Earned_Leave DECIMAL(3, 2)  null,
Casual_Leave DECIMAL(3, 2)  null,
Seek_Leave DECIMAL(3, 2) null,
Total_Leaves_Taken  DECIMAL(3, 2) not null,
Leave_Balance DECIMAL(10, 2) not null,
Month_Leave DECIMAL(3, 2) not null
)

--  Create Table DocumentMst(
--DocumentId int not null identity(1,1) PRIMARY KEY,
--DocumentName  nvarchar(200),
--Document  nvarchar(max),
--Id int not null,
--IsActive bit not null,
--IsDelete bit not null,
--CreatedBy bit not null,
--CreatedOn datetime null,
--UpdateBy bit not null,
--UpdatedOn datetime null
--)