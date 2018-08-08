﻿CREATE PROCEDURE [dbo].[GetSellerByUserId]
	@userid int
AS
begin
	SELECT Sellers.Id from Sellers
	inner join Users on Sellers.UserId=Users.Id
	where Sellers.UserId=@userid
end
