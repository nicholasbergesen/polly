INSERT INTO [PollyTest].[dbo].[DownloadData] 
	  ([TimeStamp]
      ,[Url]
      ,[RawHtml]
      ,[WebsiteId]
      ,[ProcessDateTime])
SELECT TOP (1000)
      [TimeStamp]
      ,[Url]
      ,[RawHtml]
      ,[WebsiteId]
      ,[ProcessDateTime]
  FROM [PollyDb].[dbo].[DownloadData]