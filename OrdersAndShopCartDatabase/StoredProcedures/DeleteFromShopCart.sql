CREATE PROCEDURE [dbo].[DeleteFromShopCart]
	@Product_Id int,
	@Customer_Id int
AS
	delete from ShopCart
	where Product_Id=@Product_Id and Customer_Id=@Customer_Id
GO