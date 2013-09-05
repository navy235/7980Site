USE [pad_db]

SET IDENTITY_INSERT [dbo].[ArticleCate] ON
INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (1, N'关于我们', NULL, 100000, 0, 0, 0)
INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (2, N'公司介绍', 1, 100100, 0, 1, 1)
INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (3, N'联系方式', 1, 100200, 0, 2, 1)
INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (4, N'客服展示', 1, 100300, 0, 3, 1)
INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (5, N'帮助', NULL, 110000, 0, 4, 0)
INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (6, N'服务流程', 5, 110100, 1, 5, 1)
INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (7, N'资源说明', 5, 110200, 1, 6, 1)
INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (8, N'常见问题', 5, 110300, 1, 7, 1)
INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (9, N'服务条款', NULL, 120000, 0, 8, 0)

SET IDENTITY_INSERT [dbo].[ArticleCate] OFF
