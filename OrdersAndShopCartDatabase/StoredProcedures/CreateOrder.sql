CREATE PROCEDURE [dbo].[CreateOrder]
	@Catalog_Id int,
	@Date DATE,
	@Address VARCHAR(50), 
    @CellPhone VARBINARY(50), 
    @Quantity INT, 
    @TotalAmount MONEY
AS
	insert into Orders(Catalog_Id, [Date], [Address], CellPhone, Quantity, TotalAmount)
	Values (@Catalog_Id, @Date, @Address, @CellPhone, @Quantity, @TotalAmount)
	select scope_identity()
	return 0
GO