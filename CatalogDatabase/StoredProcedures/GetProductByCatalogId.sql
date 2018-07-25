CREATE PROCEDURE [dbo].[GetProductByCatalogId]
	@Catalog_Id int
AS
	SELECT Product_Id from SellerProduct
	where Id=@Catalog_Id
go
