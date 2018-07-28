CREATE PROCEDURE [dbo].[DeleteSellerProduct]
	@Product_Id int
AS
	delete from SellerProduct 
	where ProductId=@Product_Id
GO
