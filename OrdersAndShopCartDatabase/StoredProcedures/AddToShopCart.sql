CREATE PROCEDURE [dbo].[AddToShopCart]
	@Customer_Id int,
	@Catalog_Id int
	
AS
declare @oldquantity int
begin
 select @oldquantity= Quantity from ShopCart where CustomerId=@Customer_Id and CatalogId=@Catalog_Id
 if @oldquantity>=1
 update  ShopCart
 Set Quantity=@oldquantity+1
	else Insert into ShopCart(CustomerId, CatalogId,Quantity)
	values(@Customer_Id, @Catalog_Id,1)
	end
GO
