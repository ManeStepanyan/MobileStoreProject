CREATE PROCEDURE [dbo].[CreateProduct]
    @Name VARCHAR(30), 
    @Brand VARCHAR(20), 
    @Version varchar(20), 
    @Price MONEY, 
    @RAM INT, 
    @Year INT, 
    @Display decimal, 
    @Battery int, 
    @Camera INT, 
    @Image VARCHAR(200),
	@Quantity int
AS
declare @res int
begin
	exec @res= InsertHelper @Name, @Brand, @Version, @Price, @RAM, @Year, @Display, @Battery, @Camera, @Image,@Quantity
	select @res
end