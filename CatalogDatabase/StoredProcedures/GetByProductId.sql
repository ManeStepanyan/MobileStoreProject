CREATE PROCEDURE [dbo].[GetByProductId]
	@Product_Id int
AS
	SELECT Id from SellerProduct where Product_Id=@Product_Id
go
