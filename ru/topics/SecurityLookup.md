# Поиск инструмента

Некоторые коннекторы (например, [OpenECry](OEC.md), [Interactive Brokers](IB.md) или [Sterling](Sterling.md)) не поддерживают после вызова соединения ([IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect)) передачу всех имеющихся на сервере инструментов на клиент (как правило, это сделано для уменьшения нагрузки на сервер брокера). 

Для поиска инструмента необходимо вызывать метод [IConnector.LookupSecurities](xref:StockSharp.BusinessEntities.IConnector.LookupSecurities). Передаваемый в него инструмент используется в качестве фильтра. Доступны следующие критерии поиска (точное количество зависит от брокерской системы): 

- Свойство [Security.Code](xref:StockSharp.BusinessEntities.Security.Code) задает маску имени инструмента или описания (например, «ES» или «e\-mini» или «gold») или точное название (например, «esh5»).
- Свойство [Security.Type](xref:StockSharp.BusinessEntities.Security.Type) задает тип инструмента.
- Свойство [Security.Board](xref:StockSharp.BusinessEntities.Security.Board) задает, на какой площадке торгуется инструмент (например, [ExchangeBoard.Forts](xref:StockSharp.BusinessEntities.ExchangeBoard.Forts) или [ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq)).

Найденные инструменты будут возвращены через событие [Connector.NewSecurity](xref:StockSharp.Algo.Connector.NewSecurity). 
