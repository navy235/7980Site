USE [pad_db]




SET IDENTITY_INSERT [dbo].[Group] ON
INSERT [dbo].[Group] ([ID], [Name], [Description]) VALUES (1, N'超级管理员组', N'超级管理员组')
INSERT [dbo].[Group] ([ID], [Name], [Description]) VALUES (2, N'普通用户', N'普通用户')
SET IDENTITY_INSERT [dbo].[Group] OFF

SET IDENTITY_INSERT [dbo].[Department] ON
INSERT [dbo].[Department] ([ID], [Name], [Description], [Leader]) VALUES (1, N'总经办', N'总经办，最高权限', N'王总')
INSERT [dbo].[Department] ([ID], [Name], [Description], [Leader]) VALUES (2, N'开发部', N'负责程序的开发和部署', N'刘德华')
INSERT [dbo].[Department] ([ID], [Name], [Description], [Leader]) VALUES (3, N'编辑部', N'负责日常新闻的采集，和发布', N'张惠妹')
INSERT [dbo].[Department] ([ID], [Name], [Description], [Leader]) VALUES (4, N'人事部', N'负责公司人员招聘录用', N'张学友')
INSERT [dbo].[Department] ([ID], [Name], [Description], [Leader]) VALUES (5, N'销售部', N'负责媒体资源的销售', N'林志玲')
INSERT [dbo].[Department] ([ID], [Name], [Description], [Leader]) VALUES (6, N'测试部', N'负责相关软件测试', N'成龙')
SET IDENTITY_INSERT [dbo].[Department] OFF

SET IDENTITY_INSERT [dbo].[Permissions] ON
INSERT [dbo].[Permissions] ([ID], [Name], [Description], [Controller], [Action], [Namespace], [DepartmentID]) VALUES (1, N'城市信息管理', N'城市信息分类管理', N'citycate', N'controller', N'default', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [Description], [Controller], [Action], [Namespace], [DepartmentID]) VALUES (2, N'媒体信息管理', N'媒体信息管理', N'mediacate', N'controller', N'default', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [Description], [Controller], [Action], [Namespace], [DepartmentID]) VALUES (3, N'文章类别管理', N'文章类别管理', N'articlecate', N'controller', N'default', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [Description], [Controller], [Action], [Namespace], [DepartmentID]) VALUES (4, N'媒体形式管理', N'媒体形式管理', N'formatcate', N'controller', N'default', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [Description], [Controller], [Action], [Namespace], [DepartmentID]) VALUES (5, N'购买周期管理', N'购买周期管理', N'periodcate', N'controller', N'default', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [Description], [Controller], [Action], [Namespace], [DepartmentID]) VALUES (6, N'权限管理', N'权限管理', N'permissions', N'controller', N'default', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [Description], [Controller], [Action], [Namespace], [DepartmentID]) VALUES (7, N'角色管理', N'角色管理', N'roles', N'controller', N'default', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [Description], [Controller], [Action], [Namespace], [DepartmentID]) VALUES (8, N'群组管理', N'群组管理', N'group', N'controller', N'default', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [Description], [Controller], [Action], [Namespace], [DepartmentID]) VALUES (9, N'部门管理', N'部门管理', N'department', N'controller', N'default', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [Description], [Controller], [Action], [Namespace], [DepartmentID]) VALUES (10, N'基本会员', N'基本会员', N'home', N'controller', N'default', 6)
SET IDENTITY_INSERT [dbo].[Permissions] OFF

SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT [dbo].[Roles] ([ID], [Name], [Description]) VALUES (1, N'超级管理员', N'超级管理员')
INSERT [dbo].[Roles] ([ID], [Name], [Description]) VALUES (2, N'编辑部文员', N'编辑部文员')
INSERT [dbo].[Roles] ([ID], [Name], [Description]) VALUES (3, N'基本会员', N'基本会员')
SET IDENTITY_INSERT [dbo].[Roles] OFF

INSERT [dbo].[Group_Roles] ([GroupID], [RoleID]) VALUES (1, 1)
INSERT [dbo].[Group_Roles] ([GroupID], [RoleID]) VALUES (2, 3)

INSERT [dbo].[Role_Permissions] ([RoleID], [PermissionID]) VALUES (1, 1)
INSERT [dbo].[Role_Permissions] ([RoleID], [PermissionID]) VALUES (1, 2)
INSERT [dbo].[Role_Permissions] ([RoleID], [PermissionID]) VALUES (1, 3)
INSERT [dbo].[Role_Permissions] ([RoleID], [PermissionID]) VALUES (2, 3)
INSERT [dbo].[Role_Permissions] ([RoleID], [PermissionID]) VALUES (1, 4)
INSERT [dbo].[Role_Permissions] ([RoleID], [PermissionID]) VALUES (1, 5)
INSERT [dbo].[Role_Permissions] ([RoleID], [PermissionID]) VALUES (1, 6)
INSERT [dbo].[Role_Permissions] ([RoleID], [PermissionID]) VALUES (1, 7)
INSERT [dbo].[Role_Permissions] ([RoleID], [PermissionID]) VALUES (1, 8)
INSERT [dbo].[Role_Permissions] ([RoleID], [PermissionID]) VALUES (1, 9)
INSERT [dbo].[Role_Permissions] ([RoleID], [PermissionID]) VALUES (3, 10)
