CREATE PROCEDURE [dbo].[UpdateQuantity]
	@orderedQuantity int,
	@Id int
AS
	Update Products
	set Quantity= Quantity-@orderedQuantity
	where  Catalog_Id=@Id
RETURN 0
