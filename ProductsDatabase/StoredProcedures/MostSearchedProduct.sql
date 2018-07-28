CREATE PROCEDURE [dbo].[MostSearchedProduct]
	as
	select * from Products
	where SearchCount=(select MAX(SearchCount) from Products as maximum)
go