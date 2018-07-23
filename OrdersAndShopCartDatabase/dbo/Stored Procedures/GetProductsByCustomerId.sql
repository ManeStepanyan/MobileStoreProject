CREATE PROCEDURE [dbo].[GetProductsByCustomerId]
	@Customer_Id int
AS
	select Product_Id from ShopCart
	where Customer_Id=@Customer_Id
	group by Product_Id
	go
	