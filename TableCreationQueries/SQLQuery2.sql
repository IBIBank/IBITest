CREATE TABLE [dbo].[Transaction]
(
	[TransactionID] INT NOT NULL PRIMARY KEY, 
    [Type] NCHAR(1) NOT NULL, 
    [TransactionDate] DATETIME NOT NULL, 
    [Amount] MONEY NOT NULL, 
    [TransactionRemarks] NCHAR(100) NOT NULL, 
    [SrcAccount] BIGINT NOT NULL, 
    [DestAccount] BIGINT NULL
)

CREATE TABLE [dbo].[Payee]
(
	[PayeeID] INT NOT NULL PRIMARY KEY, 
    [PayeeNickName] NCHAR(50) NOT NULL, 
    [CustomerID] BIGINT NOT NULL, 
    [PayeeAccountNumber] BIGINT NOT NULL
)

CREATE TABLE [dbo].[NewAccountRequest]
(
	[RequestID] INT NOT NULL PRIMARY KEY, 
    [BranchCode] BIGINT NOT NULL, 
    [CustomerID] BIGINT NOT NULL, 
    [SubmissionDate] DATETIME NOT NULL, 
    [ServiceDate] DATETIME NULL, 
    [Status] NCHAR(1) NOT NULL, 
    [AddressProof] IMAGE NOT NULL
)

CREATE TABLE [dbo].[Loans]
(
	[LoanAccountNumber] BIGINT NOT NULL PRIMARY KEY, 
    [AnnualIncome] MONEY NOT NULL, 
    [LoanType] NCHAR(1) NOT NULL, 
    [Tenure] NUMERIC NOT NULL, 
    [SalaryProof] IMAGE NOT NULL
)
CREATE TABLE [dbo].[LoanRequest]
(
	[RequestID] INT NOT NULL PRIMARY KEY, 
    [TypeOfLoan] NCHAR(1) NOT NULL, 
    [CustomerID] BIGINT NOT NULL, 
    [SubmissionDate] DATETIME NOT NULL, 
    [ServiceDate] DATETIME NULL, 
    [Status] NCHAR(1) NOT NULL, 
    [BranchCode] BIGINT NOT NULL, 
    [AnnualIncome] MONEY NOT NULL, 
    [Amount] MONEY NOT NULL, 
    [Tenure] NUMERIC NOT NULL, 
    [AddressProof] IMAGE NOT NULL, 
    [SalaryProof] IMAGE NOT NULL
)
CREATE TABLE [dbo].[Customer]
(
	[CustomerID] BIGINT NOT NULL PRIMARY KEY, 
    [CustomerName] NCHAR(50) NOT NULL, 
    [DOB] DATE NOT NULL, 
    [UserID] NCHAR(20) NOT NULL, 
    [Password] NCHAR(20) NOT NULL, 
    [PermanentAddress] NCHAR(100) NOT NULL, 
    [CommunicationAddress] NCHAR(100) NOT NULL, 
    [ContactNumber] NCHAR(10) NOT NULL, 
    [Email] NCHAR(50) NOT NULL, 
    [TransactionPassword] NCHAR(20) NOT NULL, 
    [Token] NCHAR(10) NOT NULL, 
    [PhotoIDProof] IMAGE NOT NULL
)
CREATE TABLE [dbo].[ClosingRequest]
(
	[RequestID] INT NOT NULL PRIMARY KEY, 
    [SubmissionDate] DATETIME NOT NULL, 
    [ServiceDate] DATETIME NULL, 
    [Status] NCHAR(1) NOT NULL, 
    [AccountNumber] BIGINT NOT NULL, 
    [CustomerID] BIGINT NOT NULL
)






CREATE TABLE [dbo].[BranchTransferRequest]
(
	[RequestID] INT NOT NULL PRIMARY KEY, 
    [SubmissionDate] DATETIME NOT NULL, 
    [ServiceDate] DATETIME NULL, 
    [Status] NCHAR(1) NOT NULL, 
    [AccountNumber] BIGINT NOT NULL, 
    [FromBranch] BIGINT NOT NULL, 
    [ToBranch] BIGINT NOT NULL, 
    [CustomerID] BIGINT NOT NULL
)
CREATE TABLE [dbo].[Branch]
(
	[BranchCode] BIGINT NOT NULL PRIMARY KEY, 
    [BranchName] NCHAR(50) NOT NULL, 
    [CityName] NCHAR(50) NOT NULL, 
    [Address] NCHAR(100) NOT NULL, 
    [ContactNumber] NCHAR(10) NOT NULL, 
    [BankerName] NCHAR(50) NOT NULL, 
    [BranchLogInID] NCHAR(20) NOT NULL, 
    [BranchPassword] NCHAR(20) NOT NULL, 
    [Email] NCHAR(50) NOT NULL
)
CREATE TABLE [dbo].[Account]
(
	[AccountNumber] BIGINT NOT NULL PRIMARY KEY, 
    [AccountType] NCHAR(1) NOT NULL, 
    [OpeningDate] DATE NOT NULL, 
    [Status] NCHAR(1) NOT NULL, 
    [Balance] MONEY NOT NULL, 
    [BranchCode] BIGINT NOT NULL, 
    [CustomerID] BIGINT NOT NULL, 
    [AddressProof] IMAGE NOT NULL
)
