CREATE PROCEDURE [dbo].[GetProductsBySellerId]
	@Seller_Id int
AS
	select * from SellerProduct where SellerId=@Seller_Id
GO
