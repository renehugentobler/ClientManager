/****** Script for SelectTopNRows command from SSMS  ******/
SELECT [table],[_auditt].createdate,DisplayName,OldValue
  FROM [ClientManager].[dbo].[_auditt], [ClientManager].[dbo].Employee
  where [table] = 'LOGIN' 
  and Employee.Id = [Key]
  and DisplayName NOT IN ('Tatyana Neudecker','ALL')
  order by [_auditt].createdate desc

  select * from Lead_Flat order by Customer desc

select * from [dbo].[LeadReceived] order by createdate desc 
select * from [dbo].[LeadReceived] where Data like '%6588%' order by createdate desc 

strName=Jason+Cohen&strEmail=jason%40bathroomtransformationspecialists.co.uk&strPhone=791-390-9357&strComments=&dtePostDate=5%2f11%2f2014+4%3a27%3a26+PM&strSource=Video+Lead&intID=6588
select * from Lead_Flat order by Customer desc

select Id, Customer, Name, EMail, Phone, AssignedToId, AssignedTo,  EntryDate
from Lead_Flat 
where AssignedTo = 'Harry Raker'
order by Customer desc

strName=miriam+calles&strEmail=1universalapplianceservice%40gmail.com&strPhone=925-812-1123&strComments=I+will+like+to+know+how+much+does+it+cost+to+get+videos+on+learning+how+to+fix+appliances.&dtePostDate=5%2f5%2f2014+9%3a55%3a38+AM&strSource=Video+Lead&intID=-1&target=1

select * from [dbo].[LeadReceived] order by createdate desc
select * from [dbo].Lead_Flat order by createdate desc


update Lead_Flat
set CallLaterDate = EntryDate +5
where CallLaterDate is null

select * from Lead_Flat where EntryDate=CallLaterDate

select * from Lead_Flat where EMail = 'captjmmcnyr@yahoo.com'

select * from _auditt where [Table] = 'LeadReceiver'
  order by [_auditt].createdate desc

  captjmmcnyr@yahoo.com

  dbcc showcontig ('[Lead_Flat]') with tableresults


select * from Lead_Flat 
where EMail in (SELECT EMail from Lead_Flat group by email having count(*)>1)