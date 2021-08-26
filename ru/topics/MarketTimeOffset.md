# Биржевое время

Для работы со временем в [S\#](StockSharpAbout.md) используется структура [DateTimeOffset](https://msdn.microsoft.com/ru-ru/library/system.datetimeoffset(v=vs.110).aspx). Эта структура хранит информацию о времени в формате UTC (англ. Coordinated Universal Time \- [Всемирное координированное время](https://ru.wikipedia.org/wiki/Всемирное_координированное_время)), а также смещение локального времени относительно UTC. Таким образом, всегда можно определить локальное время и смещение (часовой пояс) источника данных при помощи свойств [DateTimeOffset.DateTime](https://msdn.microsoft.com/ru-ru/library/system.datetimeoffset.datetime(v=vs.110).aspx) (или [DateTimeOffset.LocatDateTime](https://msdn.microsoft.com/ru-ru/library/system.datetimeoffset.localdatetime(v=vs.110).aspx) ) и [DateTimeOffset.Offset](https://msdn.microsoft.com/ru-ru/library/system.datetimeoffset.offset(v=vs.110).aspx) соответственно. 

Для определения биржевого времени в [S\#](StockSharpAbout.md) можно использовать стандартные методы [C\#](https://ru.wikipedia.org/wiki/C_Sharp): [DateTime.Now](../api/System.DateTime.Now.html) или [DateTimeOffset.Now](../api/System.DateTimeOffset.Now.html). Чтобы данные методы возвращали точное время, необходимо синхронизировать локальное время компьютера с атомными часами через вызов метода [TimeHelper.SyncMarketTime](../api/Ecng.Common.TimeHelper.SyncMarketTime.html). Разница во времени будет записана в [TimeHelper.NowOffset](../api/Ecng.Common.TimeHelper.NowOffset.html). После этого в [TimeHelper.Now](../api/Ecng.Common.TimeHelper.Now.html) будет хранится синхронизированное локальное время с учетом [TimeHelper.NowOffset](../api/Ecng.Common.TimeHelper.NowOffset.html). Класс **TimeHelper** находится в пространстве имен **Ecng.Common**. 

### Пример получения скорректированного времени

Пример получения скорректированного времени

- ```cs
  				
  					
  					// выводим текущее локальное время
  					Console.WriteLine(TimeHelper.Now);
  					
  					// синхронизируем локальное время с атомными часами
  					TimeHelper.SyncMarketTime(10000);
  					
  					// выводим синхронизированное текущее локальное время
  					Console.WriteLine(TimeHelper.Now);
  					
  			  
  ```
