CREATE PROCEDURE [dbo].[GetCustomerIdByOrderId]
	@id int

AS
	SELECT Customer_Id from CustomerOrder
	where Order_Id=@id
go
