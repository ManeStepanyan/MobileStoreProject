CREATE PROCEDURE [dbo].[GetCatalogIdByOrderId]
	@orderId int
AS
	SELECT CatalogId from Orders
	where Id=@OrderId
RETURN 0