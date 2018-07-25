CREATE PROCEDURE [dbo].[GetProductsBySellerId]
	@Seller_Id int
AS
	select * from SellerProduct where @Seller_Id=@Seller_Id
GO
