CREATE PROCEDURE [dbo].[GetByCatalogId]
	@Catalog_Id int

AS
	SELECT * from SellerProduct where Id=@Catalog_Id
Go
