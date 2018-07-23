CREATE PROCEDURE [dbo].[GetQuantity]
	@Order_Id int

AS
	SELECT Quantity from Orders where Id=@Order_Id
RETURN 0
