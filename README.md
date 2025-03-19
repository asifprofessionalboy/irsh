SELECT 
    MAX(BasicRate + DARate) AS MaxWage,
    MonthWage,
    WorkOrderNo,
    LocationNM,
    TotPaymentDays,
    WorkManCategory
FROM 
    App_WagesDetailsJharkhand
WHERE 
    VendorCode = '13732' 
    AND YearWage = '2024' 
    AND AadharNo = '802488904291' 
    AND WorkOrderNo = '4700023096'
GROUP BY 
    MonthWage,
    WorkOrderNo,
    LocationNM,
    TotPaymentDays,
    WorkManCategory
ORDER BY 
    MonthWage DESC;
