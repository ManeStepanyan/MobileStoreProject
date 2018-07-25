CREATE PROCEDURE [dbo].[UpdateCatalogIdByProductId]
	@Product_Id int ,
	@Catalog_Id int
AS
	Update Products set Catalog_Id=@Catalog_Id
	where Id=@Product_Id
go
