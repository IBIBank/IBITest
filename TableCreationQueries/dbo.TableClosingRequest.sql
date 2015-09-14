CREATE TABLE [dbo].[ClosingRequest]
(
	[RequestID] INT NOT NULL PRIMARY KEY, 
    [SubmissionDate] DATETIME NOT NULL, 
    [ServiceDate] DATETIME NULL, 
    [Status] NCHAR(1) NOT NULL, 
    [AccountNumber] BIGINT NOT NULL, 
    [CustomerID] BIGINT NOT NULL
)
