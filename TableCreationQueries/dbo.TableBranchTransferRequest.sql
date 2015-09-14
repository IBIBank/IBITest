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
