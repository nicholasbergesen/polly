/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000)
      [UrlHash]
      ,[Price]
      ,p.[Name]
      ,[Description]
      ,[Subtitle]
      ,[Breadcrumb]
      ,[Category]
      ,[DownloadDataId]
	  ,w.DescriptionXPath
	  ,d.Url
  FROM [PollyTest].[dbo].[Product] p
  inner join DownloadData d on p.[DownloadDataId] = d.Id
  inner join Website w on d.WebsiteId = w.Id


  truncate table product