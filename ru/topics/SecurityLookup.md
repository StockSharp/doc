# Поиск инструмента

Некоторые коннекторы (например, [OpenECry](OEC.md), [Interactive Brokers](IB.md) или [Sterling](Sterling.md)) не поддерживают после вызова соединения ([IConnector.Connect](../api/StockSharp.BusinessEntities.IConnector.Connect.html)) передачу всех имеющихся на сервере инструментов на клиент (как правило, это сделано для уменьшения нагрузки на сервер брокера). 

Для поиска инструмента необходимо вызывать метод [IConnector.LookupSecurities](../api/StockSharp.BusinessEntities.IConnector.LookupSecurities.html). Передаваемый в него инструмент используется в качестве фильтра. Доступны следующие критерии поиска (точное количество зависит от брокерской системы): 

- Свойство [Security.Code](../api/StockSharp.BusinessEntities.Security.Code.html) задает маску имени инструмента или описания (например, «ES» или «e\-mini» или «gold») или точное название (например, «esh5»).
- Свойство [Security.Type](../api/StockSharp.BusinessEntities.Security.Type.html) задает тип инструмента.
- Свойство [Security.Board](../api/StockSharp.BusinessEntities.Security.Board.html) задает, на какой площадке торгуется инструмент (например, [ExchangeBoard.Forts](../api/StockSharp.BusinessEntities.ExchangeBoard.Forts.html) или [ExchangeBoard.Nasdaq](../api/StockSharp.BusinessEntities.ExchangeBoard.Nasdaq.html)).

Найденные инструменты будут возвращены через событие [Connector.NewSecurity](../api/StockSharp.Algo.Connector.NewSecurity.html). 

## См. также
