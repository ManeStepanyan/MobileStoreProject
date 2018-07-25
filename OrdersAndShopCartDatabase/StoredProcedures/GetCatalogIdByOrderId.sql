CREATE PROCEDURE [dbo].[GetCatalogIdByOrderId]
	@orderId int
AS
	SELECT Catalog_Id from Orders
	where Id=@OrderId
RETURN 0