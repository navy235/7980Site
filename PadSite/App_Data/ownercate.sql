USE [pad_db]
SET IDENTITY_INSERT [dbo].[OwnerCate] ON 
GO
INSERT [dbo].[OwnerCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (1, N'一般代理', 1, 1001, 0, 0)
GO
INSERT [dbo].[OwnerCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (2, N'自有媒体', 1, 1002, 0, 1)
GO
INSERT [dbo].[OwnerCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (3, N'独家代理', 1, 1003, 0, 2)
GO
INSERT [dbo].[OwnerCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (4, N'行业代理', 1, 1004, 0, 3)
GO
INSERT [dbo].[OwnerCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (5, N'优势代理', 1, 1005, 0, 4)
GO
