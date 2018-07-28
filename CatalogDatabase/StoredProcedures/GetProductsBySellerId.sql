CREATE PROCEDURE [dbo].[GetProductsBySellerId]
	@Seller_Id int
AS
	select Product_Id from SellerProduct where @Seller_Id=@Seller_Id
GO
