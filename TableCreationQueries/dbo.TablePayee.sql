CREATE TABLE [dbo].[Payee]
(
	[PayeeID] INT NOT NULL PRIMARY KEY, 
    [PayeeNickName] NCHAR(50) NOT NULL, 
    [CustomerID] BIGINT NOT NULL, 
    [PayeeAccountNumber] BIGINT NOT NULL
)
