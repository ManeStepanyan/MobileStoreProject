CREATE PROCEDURE [dbo].[DeleteFromShopCart]
	@Catalog_Id int,
	@Customer_Id int
AS
	delete from ShopCart
	where Catalog_Id=@Catalog_Id and Customer_Id=@Customer_Id
GO