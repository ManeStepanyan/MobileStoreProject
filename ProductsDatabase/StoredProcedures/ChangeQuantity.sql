CREATE PROCEDURE [dbo].[ChangeQuantity]
	@Id int,
	@NewQuantity int
AS
begin
	Update Products
	set  Quantity=@NewQuantity
	where Id=@Id
end
