USE [pad_db]
SET IDENTITY_INSERT [dbo].[PeriodCate] ON 
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (1, N'天', 1, 1001, 0, 0)
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (2, N'周', 1, 1002, 0, 1)
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (3, N'月', 1, 1003, 0, 2)
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (4, N'季度', 1, 1004, 0, 3)
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (5, N'半年', 1, 1005, 0, 4)
GO
INSERT [dbo].[PeriodCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (6, N'年', 1, 1006, 0, 5)
GO