CREATE PROCEDURE [dbo].[GetCatalogsIdsBySellerId]
	@Seller_Id int
AS
   select Id from SellerProduct where @Seller_Id=@Seller_Id
	go
