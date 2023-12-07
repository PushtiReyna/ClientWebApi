Create Database ClientApiDB

--added on 07-12-2023 by PP-------------------------------------------START---------------------------------------------
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'ClientMst')
BEGIN 
    Create Table dbo.ClientMst(
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
			Joining_Date datetime  null,
			Probation_Period int  null,
			IsActive bit not null,
			IsDelete bit not null,
			CreatedBy int not null,
			CreatedOn datetime null,
			UpdateBy int  null,
			UpdatedOn datetime null
			);
	PRINT 'ClientMst Table Created' 
END
ELSE
BEGIN 
	PRINT 'ClientMst Table Already Exist' 
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'SalaryMst')
BEGIN 
	Create Table dbo.SalaryMst(
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
			);
	PRINT 'SalaryMst Table Created' 
END
ELSE
BEGIN 
	PRINT 'SalaryMst Table Already Exist' 
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'LeaveMst')
BEGIN 
	create table dbo.LeaveMst
			(
			LeaveId int not null identity(1,1) PRIMARY KEY,
			Id int not null,
			Month int not null,
			Year int not null,
			Opening_Leave_Balance DECIMAL(10, 2)  not null,
			Closing_Leave_Balance DECIMAL(10, 2) not null,
			Earned_Leave DECIMAL(10, 2)  null,
			Casual_Leave DECIMAL(10, 2)  null,
			Seek_Leave DECIMAL(10, 2) null,
			Total_Leaves_Taken  DECIMAL(10, 2) not null,
			Leave_Balance DECIMAL(10, 2) not null,
			Month_Leave DECIMAL(10, 2) not null,
			Loss_Of_Pay_Leave  DECIMAL(10, 2) null
			);
	PRINT 'LeaveMst Table Created' 
END
ELSE
BEGIN 
	PRINT 'LeaveMst Table Already Exist' 
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'AttendanceMst')
BEGIN 
	create table dbo.AttendanceMst
			(
			AttendanceId int not null identity(1,1) PRIMARY KEY,
			Id int not null,
			Date_Attendance datetime not null,
			Types_Of_Leave  varchar(max) not null,
			Present_Absent varchar(max) not null
			);
	PRINT 'AttendanceMst Table Created' 
END
ELSE
BEGIN 
	PRINT 'AttendanceMst Table Already Exist' 
END

--added on 07-12-2023 by PP-------------------------------------------END-----------------------------------------------
--executed on local by PP on 07-12-2023---------------------------------------------------------------------------------


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