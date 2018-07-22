CREATE PROCEDURE [dbo].[EmailExists](
   @Email VARCHAR(40)
)
AS
  IF EXISTS(SELECT * FROM Users WHERE [Email] = @Email)
  select 1
 else select 2
Go