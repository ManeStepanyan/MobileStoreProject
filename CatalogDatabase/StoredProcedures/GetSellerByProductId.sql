CREATE PROCEDURE [dbo].[GetSellerByProductId]
	@Product_Id int
AS
	SELECT Seller_Id from [SellerProduct] where Product_Id=@Product_Id
GO
