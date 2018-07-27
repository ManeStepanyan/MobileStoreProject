CREATE PROCEDURE [dbo].[GetOrdersByCustomerId]
	@Customer_Id int
AS
	SELECT Order_Id from CustomerOrder
	where Customer_Id=@Customer_Id

