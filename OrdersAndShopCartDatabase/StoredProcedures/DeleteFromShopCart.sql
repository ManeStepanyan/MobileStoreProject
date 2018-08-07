CREATE PROCEDURE [dbo].[DeleteFromShopCart]
	@Catalog_Id int,
	@Customer_Id int
AS
	delete from ShopCart
	where CatalogId=@Catalog_Id and CustomerId=@Customer_Id
GO