USE [pad_db]

SET IDENTITY_INSERT [dbo].[ArticleCate] ON
INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (1, N'关于我们', NULL, 100000, 0, 0, 0)

INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (2, N'帮助', NULL, 110000, 0, 1, 0)

INSERT [dbo].[ArticleCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex],  [IsSingle]) VALUES (3, N'服务条款', NULL, 120000, 0, 2, 0)

SET IDENTITY_INSERT [dbo].[ArticleCate] OFF
