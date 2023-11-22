Create Database ClientApiDB

Create Table ClientMst(
Id int not null identity(1,1) PRIMARY KEY,
Fullname varchar (200) not null,
Gender Varchar(50) not null,
DOB datetime not null,
Image nvarchar(400) not null,
Username varchar(50) not null,
Password varchar(50) not null,
IsActive bit not null,
IsDelete bit not null,
CreatedBy bit not null,
CreatedOn datetime null,
UpdateBy bit not null,
UpdatedOn datetime null
)

  ALTER TABLE ClientMst
ALTER COLUMN CreatedBy Int;

  ALTER TABLE ClientMst
ALTER COLUMN UpdateBy Int;

 alter table ClientMst
  add  RefreshToken  nvarchar(200)

 alter table ClientMst
  add  RefreshTokenExpiryTime datetime 

     alter table ClientMst
  add  Token  nvarchar(500)

 alter table ClientMst
  add  TokenExpiryTime datetime 