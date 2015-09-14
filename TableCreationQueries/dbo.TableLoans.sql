CREATE TABLE [dbo].[Loans]
(
	[LoanAccountNumber] BIGINT NOT NULL PRIMARY KEY, 
    [AnnualIncome] MONEY NOT NULL, 
    [LoanType] NCHAR(1) NOT NULL, 
    [Tenure] NUMERIC NOT NULL, 
    [SalaryProof] IMAGE NOT NULL
)
