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
