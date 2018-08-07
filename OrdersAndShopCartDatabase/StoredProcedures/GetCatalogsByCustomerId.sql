CREATE PROCEDURE [dbo].[GetCatalogsByCustomerId]
	@Customer_Id int
AS
	select *  from ShopCart
	where CustomerId=@Customer_Id
	go