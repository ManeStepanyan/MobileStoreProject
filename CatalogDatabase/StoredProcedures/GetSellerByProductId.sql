CREATE PROCEDURE [dbo].[GetSellerByProductId]
	@Product_Id int
AS
	SELECT SellerId from [SellerProduct] where ProductId=@Product_Id
GO
