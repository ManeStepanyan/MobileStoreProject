﻿CREATE PROCEDURE [dbo].[CreateProduct]
    @Name VARCHAR(30), 
    @Brand VARCHAR(20), 
    @Version varchar(20)=null,
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
declare @res int
begin
	exec @res= InsertHelper @Name, @Brand, @Version, @Price, @RAM, @Year, @Display, @Battery, @Camera, @Image,@Quantity, @Memory, @Color, @Description
	select @res
	return 0
end