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
