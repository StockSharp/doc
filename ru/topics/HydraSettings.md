# Настройки

[S\#.Data](Hydra.md) может исользоваться в серверном режиме, в данном режиме можно удаленно подключиться к [S\#.Data](Hydra.md) и получить имеющиеся данные в хранилище. К [S\#.Data](Hydra.md), работающей в серверном режиме, можно подключиться из [S\#.Designer](Designer.md) (как это сделать описано в пункте [Создание хранилища исторических данных](Designer_Creating_repository_of_historical_data.md) документации по [S\#.Designer](Designer.md)). Также к [S\#.Data](Hydra.md) можно подключиться через [S\#.API](StockSharpAbout.md) (как это сделать описано в пунктах [Подключение через FIX протокол](hydraAPIConnect.md)).

В серверном режиме програма [S\#.Data](Hydra.md) позволяет пользователю работать, используя одно подключение, сразу с несколькими програмами. Путем настройки ключа доступа в настройках программы, пользователь может одновременно работать с одним источником под одной учетной записью.

Фактически подключение к источнику происходит чере S\#.Hydra, к которой одновременно подключаются, например, [S\#.Designer](Designer.md), [S\#.Terminal](Terminal.md). Такой метод позволяет избежать переподключение между программами, покупку дополнительного подключения. При такой работе, исключаются конфликты, возникновение которых обусловлено выставлением заявок или сделок с различных программ. S\#.Hydra получает сигнал и передает обратно результат той программе от которой он получен, при этом других последовательность работы не нарушается.

Для включения серверного режима [S\#.Data](Hydra.md) необходимо в верхнем меню программы выбрать вкладку **Серверный режим**.

![hydra server menu](../images/hydra_server_menu.png)

После этого нажимаем на кнопку **Настройки** откроется окно настройки серверного режима.

![hydra server](../images/hydra_server.png)

**S\#.Data сервер**

- **FIX сервер** \- перевести [S\#.Data](Hydra.md) в серверный режим, распространяющий live торговлю и исторические данные через FIX протокол. 

  В данном разделе настроек пользователь настраивает подключение для работы с источниками: 
  1. **ConvertToLatin** \- конвертировать кириллицу в латиницу 
  2. **QuotesInterval** \- период обновления котировок 
  3. **TransactionSession** \- настройка торговой сессии. Настройка для торговли через программа S\#.Hydra. 

     Настройка включает в себя возможность нстроить: Диалект FIX протокола, Отправителя и Получателя, Формат данных и другие настройки. Подробнее см. [свойства FIXServer.](https://doc.stocksharp.ru/html/Properties_T_StockSharp_Fix_FixServer.htm)
  4. **MarketDataSession** \- настройки передачи маркет\-данных, получаемых в при помощи S\#.Hydra. Подробнее см. [свойства FIXServer.](https://doc.stocksharp.ru/html/Properties_T_StockSharp_Fix_FixServer.htm)
  5. **KeepSubscriptionsOnDisconnect** \- сохранение подписок при отключении от источника. 
  6. **DeadSessionCleanupInterval** \- через какой интервал времени произойдет очистка информации, если соединение будет отключено.
- **Авторизация** \- авторизация для получения доступа к S\#.Data сервер 
- **Количество инструментов** \- максимальное количество инструментов, которое можно запрашивать с сервера 
- **Свечи (дни)** \- максимальное количество дней, доступных для загрузки истории свечей 
- **Тики (дни)** \- максимальное количество дней, доступных для загрузки истории тиковых данных 
- **Стаканы (дни)** \- максимальное количество дней, доступных для загрузки истории стаканов 
- **ОЛ (дни)** \- максимальное количество дней, доступных для загрузки истории ОЛ данных 
- **Транзакции (дни)** \- максимальное количество дней, доступных для загрузки истории транзакций 
- **Симулятор** \- включение режима симулятора 
- **Security maping** \- включить режим передачи только указанных инструментов. 

Если установить **Авторизацию** отличную от **Анонимно**, на вкладке **Общее** появится кнопка **Пользователи**, после нажатия на которую появляется окно **Пользователи**

![hydra users](../images/hydra_users.png)

В левой части окна можно добавить нового пользователя, а в правой установить для него права доступа.
