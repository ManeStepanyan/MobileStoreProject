CREATE PROCEDURE [dbo].[GetProductByCatalogId]
	@Catalog_Id int
AS
	SELECT ProductId from SellerProduct
	where Id=@Catalog_Id
go
