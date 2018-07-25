CREATE PROCEDURE [dbo].[GetOrderByCatalogId]
	@Catalog_Id int
AS
	select * from Orders where Catalog_Id=@Catalog_Id
