CREATE PROCEDURE [dbo].[GetProductsBySellerId]
	@Seller_Id int
AS
	select * from CustomerOrder where @Seller_Id=@Seller_Id
GO
