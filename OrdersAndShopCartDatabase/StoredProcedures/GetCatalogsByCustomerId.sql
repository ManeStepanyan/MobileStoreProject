CREATE PROCEDURE [dbo].[GetCatalogsByCustomerId]
	@Customer_Id int
AS
	select *  from ShopCart
	where Customer_Id=@Customer_Id
	go