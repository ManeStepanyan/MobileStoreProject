CREATE PROCEDURE [dbo].[GetOrdersByCustomerId]
	@Customer_Id int
AS
	SELECT OrderId from CustomerOrder
	where CustomerId=@Customer_Id

