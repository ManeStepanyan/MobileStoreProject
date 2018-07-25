CREATE PROCEDURE [dbo].[AddToShopCart]
	@Customer_Id int,
	@Catalog_Id int
	
AS
	Insert into ShopCart(Customer_Id, Catalog_Id)
	values(@Customer_Id, @Catalog_Id)
GO
GO