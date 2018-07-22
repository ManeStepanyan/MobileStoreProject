CREATE PROCEDURE [dbo].[GetOrdersByCustomerId]
	@Customer_Id int
AS
	SELECT * from CustomerOrder
	where Customer_Id=@Customer_Id

