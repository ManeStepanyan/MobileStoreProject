CREATE PROCEDURE [dbo].[GetByProductId]
	@Product_Id int
AS
	SELECT Id from SellerProduct where ProductId=@Product_Id
go
