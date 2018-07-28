CREATE PROCEDURE [dbo].[InsertHelper]
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
begin
	INSERT INTO Products ([Name], [Brand], [Version], [Price], RAM, [Year], Display, Battery, 
							Camera, [Image],[Quantity])
	VALUES (@Name, @Brand, @Version, @Price, @RAM, @Year, @Display, @Battery, @Camera, @Image,@Quantity)
	return scope_identity()
end