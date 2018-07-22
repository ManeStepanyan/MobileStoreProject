CREATE PROCEDURE [dbo].[AddCustomerProduct]
	@Order_Id int,
	@Customer_Id int
AS
	insert into CustomerOrder(Order_Id, Customer_Id)
	values (@Order_Id, @Customer_Id)
GO
