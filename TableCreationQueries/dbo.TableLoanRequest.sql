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
