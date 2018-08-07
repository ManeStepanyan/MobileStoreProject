CREATE PROCEDURE [dbo].[AddToShopCart]
	@Customer_Id int,
	@Catalog_Id int
	
AS
	Insert into ShopCart(CustomerId, CatalogId)
	values(@Customer_Id, @Catalog_Id)
GO
