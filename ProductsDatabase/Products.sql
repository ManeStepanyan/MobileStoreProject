CREATE TABLE [dbo].[Products] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (30)  NOT NULL,
    [Brand]       VARCHAR (20)  NOT NULL,
    [Version]     VARCHAR (20)  NULL,
    [Price]       MONEY         NOT NULL,
    [RAM]         INT           NOT NULL,
    [Year]        INT           NOT NULL,
    [Display]     DECIMAL (3, 2)  NULL,
    [Battery]     INT           NULL,
    [Camera]      INT           NOT NULL,
    [Image]       VARCHAR (200) NOT NULL,
    [Quantity]    INT           NULL,
    [Catalog_Id]  INT           NULL,
    [SearchCount] INT           DEFAULT ((0)) NOT NULL,
	[Memory] DECIMAL(3,2) NOT NULL,
	[Color] VARCHAR(10) NOT NULL,
	[Description] VARCHAR(100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);














