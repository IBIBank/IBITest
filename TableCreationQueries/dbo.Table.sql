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
