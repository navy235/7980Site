USE [pad_db]
SET IDENTITY_INSERT [dbo].[PeriodCate] ON 
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (1, N'天', Null, 1001, 0, 1)
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (2, N'周',  Null, 1002, 0, 7)
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (3, N'月',  Null, 1003, 0, 30)
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (4, N'季度',  Null, 1004, 0, 90)
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (5, N'半年',  Null, 1005, 0, 180)
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (6, N'年', Null, 1006, 0, 365)
GO
