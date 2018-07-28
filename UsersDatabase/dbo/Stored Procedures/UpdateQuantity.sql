CREATE PROCEDURE [dbo].[UpdateQuantity]
	@orderedQuantity int,
	@Id int
AS
	Update Products
	set Quantity= Quantity-@orderedQuantity
	where Id=@Id
RETURN 0