CREATE TABLE [dbo].[Orders] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Product_Id]  INT          NOT NULL,
    [Date]        DATE         NOT NULL,
    [Address]     VARCHAR (50) NOT NULL,
    [CellPhone]   VARCHAR (50) NOT NULL,
    [Quantity]    INT          NOT NULL,
    [TotalAmount] MONEY        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

