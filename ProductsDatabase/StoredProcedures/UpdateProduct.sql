CREATE PROCEDURE [dbo].[UpdateProduct]
	@Id INT,
	@Name varchar(30)=null,  
	@Brand varchar(20)=null,
	@Version varchar(20)=null,
    @Price MONEY=null,
	@RAM int=null,
	@Year int=null,
	@Display decimal(3,2)=null,
	@Battery int = null,
	@Camera int =null,
	@Image varchar(200)=null,
	@Quantity int = null,
	@Memory DECIMAL(3,2)= null,
	@Color VARCHAR(10)= null,
	@Description VARCHAR(100)= null
AS
	set @Name=ISNULL(@Name, (select [Name] from Products where Id = @Id))
	set @Brand=ISNULL(@Brand, (select [Brand] from Products where Id = @Id))
	set @Version=ISNULL(@Version, (select [Version] from Products where Id = @Id))
	set @Price=ISNULL(@Price, (select [Price] from Products where Id = @Id)) 
	set @RAM=ISNULL(@RAM, (select [RAM] from Products where Id = @Id))
	set @Year=ISNULL(@Year, (select [Year] from Products where Id = @Id))
	set @Display=ISNULL(@Display, (select [Display] from Products where Id = @Id))
	set @Battery=ISNULL(@Battery, (select [Battery] from Products where Id = @Id))
	set @Camera=ISNULL(@Camera, (select [Camera] from Products where Id = @Id))
	set @Image=ISNULL(@Image,(select [Image] from Products where Id = @Id) )
	set @Quantity=ISNULL(@Quantity, (select [Quantity] from Products where Id = @Id))
	set @Memory=ISNULL(@Memory, (select Memory from Products where Id = @Id))
	set @Color=ISNULL(@Color, (select Color from Products where Id = @Id))
	set @Description=ISNULL(@Description, (select [Description] from Products where Id = @Id))
	update Products 
	set 
	[Name] = ISNULL(@Name, [Name]),
	Brand=ISNULL(@Brand, Brand),
	[Version]=ISNULL(@Version, [Version]) ,
	Price=ISNULL(@Price, Price),
	RAM=ISNULL(@RAM, Ram), 
	[Year]=ISNULL(@Year, [Year]),
	Display=ISNULL(@Display, Display),
	Battery=ISNULL(@Battery, Battery),
	Camera=ISNULL(@Camera, Camera),
	[Image]=ISNULL(@Image,[Image]),
	Quantity=ISNULL(@Quantity, Quantity),
	Memory=ISNULL(@Memory, Memory),
	Color=ISNULL(@Color, Color),
	[Description]=ISNULL(@Description, [Description])
	where Id=@Id
GO
