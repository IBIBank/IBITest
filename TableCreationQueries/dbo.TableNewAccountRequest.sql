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
