CREATE PROCEDURE [dbo].[GetOrderByCatalogId]
	@Catalog_Id int
AS
	select * from Orders where CatalogId=@Catalog_Id
