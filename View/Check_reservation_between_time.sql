SELECT * FROM Reservations 
WHERE Workzone_Id = 4 and (
'2022-10-13 13:51' BETWEEN DateTime_Arriving and DateTime_Leaving
OR
'2022-10-13 15:15' BETWEEN DateTime_Arriving and DateTime_Leaving
OR
DateTime_Arriving BETWEEN '2022-10-13 13:15' and '2022-10-13 15:51'
OR
DateTime_Leaving BETWEEN '2022-10-13 13:15' and '2022-10-13 15:51')