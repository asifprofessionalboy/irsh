   Select '10' as month,'2024' as year,convert(varchar,datepart(d, ML.Dates) )as Dates,
   ML.EngagementType,
   ad.WorkManSl as WorkManSLNo,
   ad.WorkManName as WorkManName, 
   ad.DayDef as DayDef, 
   case when( ML.EngagementType = AD.EngagementType and Present = 'True')
   then Convert(varchar,1) else Convert(varchar,0) end as Present ,
   ad.EngagementType as Eng_Type from dbo.ListOfDaysByEngagementType('10','2024')
   as ML
   left join 
   
   App_AttendanceDetails as ad on ML.Dates = AD.Dates
   where AD.VendorCode = '17201'  and DATEPART(month, AD.Dates)= '10' and DATEPART(year, AD.Dates)= '2024'  and AadharNo='275225445020' 
   group by ML.Dates,ML.EngagementType,AD.EngagementType,AD.WorkManSl,ad.WorkManName,AD.Present,AD.WorkOrderNo,ad.DayDef order by ML.Dates,AD.WorkManSl  
