CREATE PROCEDURE [dbo].[InsertHelper]
    @Name VARCHAR(30), 
    @Brand VARCHAR(20), 
    @Version varchar(20), 
    @Price MONEY, 
    @RAM INT, 
    @Year INT, 
    @Display float, 
    @Battery int, 
    @Camera INT, 
    @Image VARCHAR(200),
	@Quantity int, 
	@Memory float,
	@Color VARCHAR(10),
	@Description VARCHAR(100)
AS
begin
	INSERT INTO Products ([Name], [Brand], [Version], [Price], RAM, [Year], Display, Battery, 
							Camera, [Image],[Quantity], Memory, Color, [Description])
	VALUES (@Name, @Brand, @Version, @Price, @RAM, @Year, @Display, @Battery, @Camera, @Image,@Quantity, @Memory, @Color, @Description)
	return scope_identity()
end
