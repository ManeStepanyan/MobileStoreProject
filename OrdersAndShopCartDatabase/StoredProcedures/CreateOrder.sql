CREATE PROCEDURE [dbo].[CreateOrder]
	@Catalog_Id int,
	@Date DATE,
	@Address VARCHAR(50), 
    @CellPhone VARBINARY(50), 
    @Quantity INT, 
    @TotalAmount MONEY,
	@CardAccount VARCHAR(50)
AS
	insert into Orders(CatalogId, [Date], [Address], CellPhone, Quantity, TotalAmount, CardAccount)
	Values (@Catalog_Id, @Date, @Address, @CellPhone, @Quantity, @TotalAmount, @CardAccount)
	select scope_identity()
	return 0
GO