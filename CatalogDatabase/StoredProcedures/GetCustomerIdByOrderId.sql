CREATE PROCEDURE [dbo].[GetCustomerIdByOrderId]
	@id int

AS
	SELECT CustomerId from CustomerOrder
	where OrderId=@id
go
