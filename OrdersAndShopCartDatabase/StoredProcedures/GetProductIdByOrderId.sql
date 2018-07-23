CREATE PROCEDURE [dbo].[GetProductIdByOrderId]
	@orderId int
AS
	SELECT Product_Id from Orders
	where Id=@OrderId
RETURN 0
