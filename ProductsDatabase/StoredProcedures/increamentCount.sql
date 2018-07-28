CREATE PROCEDURE [dbo].[increamentCount]
	@id int
AS
declare @count int
 select @count= SearchCount from Products where Id=@id
	update Products
	set SearchCount=@count+1
	where Id=@id
go
