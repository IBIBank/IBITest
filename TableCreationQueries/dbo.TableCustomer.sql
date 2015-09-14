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
