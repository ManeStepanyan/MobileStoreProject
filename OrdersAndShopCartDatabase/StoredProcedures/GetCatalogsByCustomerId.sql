CREATE PROCEDURE [dbo].[GetCatalogsByCustomerId]
	@Customer_Id int
AS
	select Catalog_Id from ShopCart
	where Customer_Id=@Customer_Id
	group by Catalog_Id
	go