# Exchange time

To work with time in [S\#](StockSharpAbout.md), the [DateTimeOffset](https://msdn.microsoft.com/library/system.datetimeoffset.aspx) structure is used. This structure stores time information in the UTC ([Coordinated Universal Time](https://en.wikipedia.org/wiki/Coordinated_Universal_Time))format, as well as local time offset related to the UTC. Thus, it is always possible to determine local time and offset (time zone) of the source using the [DateTimeOffset.DateTime](https://msdn.microsoft.com/library/system.datetimeoffset.datetime.aspx) (or [DateTimeOffset.LocatDateTime](https://msdn.microsoft.com/library/system.datetimeoffset.localdatetime.aspx) ) and [DateTimeOffset.Offset](https://msdn.microsoft.com/library/system.datetimeoffset.offset.aspx) features, correspondingly. 

To determine the stock exchange time in the [S\#](StockSharpAbout.md), standard [C\#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) methods can be used: [DateTime.Now](xref:System.DateTime.Now) or [DateTimeOffset.Now](xref:System.DateTimeOffset.Now). For these methods to return the exact time, it is necessary to synchronize the PC local time with atomic clock by calling the [TimeHelper.SyncMarketTime](xref:Ecng.Common.TimeHelper.SyncMarketTime(System.Int32)). method. Time offset will be recorded in the [TimeHelper.NowOffset](xref:Ecng.Common.TimeHelper.NowOffset). After that, the [TimeHelper.Now](xref:Ecng.Common.TimeHelper.Now) will store the synchronized local time, taking into consideration the [TimeHelper.NowOffset](xref:Ecng.Common.TimeHelper.NowOffset). The **TimeHelper** class is located in the **Ecng.Common** namespace. 

## Example of getting the corrected time

- ```cs
  					
  					// printing the current local time
  					Console.WriteLine(TimeHelper.Now);
  					
  					// doing a sync with internet clocks
  					TimeHelper.SyncMarketTime(10000);
  					
  					// printing the current local time again
  					Console.WriteLine(TimeHelper.Now);
  					
  			  
  ```
