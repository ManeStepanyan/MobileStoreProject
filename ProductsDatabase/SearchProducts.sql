CREATE PROCEDURE [dbo].[SearchProducts]
     @Name VARCHAR(30)=null, 
    @Brand VARCHAR(20)=null, 
    @Version varchar(20)=null, 
    @PriceFrom MONEY=null,
	@PriceTo MONEY=null,
    @RAM INT =null,
	@RAMTo INT=NULL,
    @Year INT =null,
	@YearTo int=null,
    @Display float =null,
    @Battery int =null,
	@BatteryTo int=null,
    @Camera INT =null,
	@CameraTo int=null,
	@Memory float= null,
	@MemoryTo DECIMAL(3,2)= null,
	@Color VARCHAR(10)= null
as
	(select * from products where [Name]=@Name and @Name is not null union
	select * from products where  @Name is  null) intersect
    (select * from products where [Brand]=@Brand and @Brand is not null union
	select * from products where @Brand is null) intersect
   (select * from products where [Version]=@Version and @Version is not null union
	select * from products where @Version is null) intersect
   ( select * from products where [Price] between @PriceFrom and  @PriceTo and @PriceTo is not null and @PriceFrom is not null union
   select * from products where [Price] >= @PriceFrom and @PriceTo is null and @PriceFrom is not null union
   select * from products where [Price] <= @PriceTo and @PriceTo is not null and @PriceFrom is  null union
	select * from products where @PriceFrom is null and @PriceTo is null) intersect
  ( select * from products where [RAM] between @RAM and  @RAMTo and @RAMTo is not null and @RAM is not null union
   select * from products where [RAM] >= @RAM and @RAMTo is null and @RAM is not null union
   select * from products where [RAM] <= @RAMTo and @RAMTo is not null and @RAM is  null union 	select * from products where @RAM is null and @RAMTo is null) intersect
 ( select * from products where [Year] between @Year and  @YearTo and @YearTo is not null and @Year is not null union
   select * from products where [Year] >= @Year and @YearTo is null and @Year is not null union
   select * from products where [Year] <= @YearTo and @YearTo is not null and @Year is  null union 	select * from products where @Year is null and @YearTo is null)intersect
 ( select * from products where [Display] =@Display and @Display is not null union select * from products where  @Display is null) intersect
 ( select * from products where [Camera] between @Camera and  @CameraTo and @CameraTo is not null and @Camera is not null union
   select * from products where [Camera] >= @Camera and @CameraTo is null and @Camera is not null union
   select * from products where [Camera] <= @CameraTo and @CameraTo is not null and @Camera is  null union 	select * from products where @Camera is null and @CameraTo is null )intersect
 ( select * from products where [Memory] between @Memory and  @MemoryTo and @MemoryTo is not null and @Memory is not null union
   select * from products where [Memory] >= @Memory and @MemoryTo is null and @Memory is not null union
   select * from products where [Memory] <= @MemoryTo and @MemoryTo is not null and @Memory is null union 	select * from products where @Memory is null and @MemoryTo is null ) intersect
 ( select * from products where [Battery] between @Battery and  @BatteryTo and @BatteryTo is not null and @Battery is not null union
   select * from products where [Battery] >= @Battery and @BatteryTo is null and @Battery is not null union
   select * from products where [Battery] <= @BatteryTo and @BatteryTo is not null and @Battery is  null union 	select * from products where @Battery is null and @BatteryTo is null)  intersect
	(select * from products where [Color]=@Color and @Color is not null union
	select * from products where @Color is null )

	order by Price
GO