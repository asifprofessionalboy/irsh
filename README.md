select distinct max(BasicRate+DARate) as MaxWage,MonthWage,WorkOrderNo,LocationNM,TotPaymentDays,WorkManCategory from App_WagesDetailsJharkhand  where  VendorCode='13732'
and YearWage='2024' and AadharNo='802488904291' and WorkOrderNo='4700023096' order by MonthWage desc
