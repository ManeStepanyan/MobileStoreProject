﻿CREATE PROCEDURE [dbo].[SearchProducts]
    @Name VARCHAR(30)=null, 
    @Brand VARCHAR(20)=null, 
    @Version varchar(20)=null, 
    @PriceFrom MONEY=null,
	@PriceTo MONEY=null,
    @RAM INT =null,
    @Year INT =null,
    @Display decimal(3, 2) =null,
    @Battery int =null,
    @Camera INT =null,
	@Memory DECIMAL(3,2)= null,
	@Color VARCHAR(10)= null,
	@Description VARCHAR(100)= null
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
	select * from products where @PriceFrom is  null) intersect
(select * from products where [RAM]=@RAM and @RAM is not null union
	select * from products where @RAM is null) intersect
	(select * from products where [Year]=@Year and @Year is not null union
	select * from products where @Year is null) intersect
(select * from products where [Display]=@Display and @Display is not null union
	select * from products where @Display is null) intersect
(select * from products where [Battery]=@Battery and @Battery is not null union
	select * from products where @Battery is null) intersect
	(select * from products where [Camera]=@Camera and @Camera is not null union
	select * from products where @Camera is null )
	(select * from products where [Memory]=@Memory and @Memory is not null union
	select * from products where @Memory is null )
	(select * from products where [Color]=@Color and @Color is not null union
	select * from products where @Color is null )
	(select * from products where [Description]=@Description and @Description is not null union
	select * from products where @Description is null )
	order by Price
GO
