# Идентификатор инструмента

В [S\#](StockSharpAbout.md) инструменты из разных источников имеют унифицированные идентификаторы [Security.Id](xref:StockSharp.BusinessEntities.Security.Id). Это сделано для того, чтобы код торгового робота не зависел от типа подключения ([Plaza II](Plaza.md), [Quik](Quik.md), [SmartCOM](Smart.md) и т.д.). Для идентификатора инструмента используется следующий синтаксис \- **\[код инструмента\]@\[код площадки\]**. Например, для акций Лукойла идентификатором будет **LKOH@EQBR**. Для инструментов срочного рынка [MOEX](https://moex.com) площадкой будет "FORTS". Так, например, для июньского фьючерса на индекс РТС идентификатором будет **RIM5@FORTS**. 

> [!TIP]
> Программа [S\#.Data](Hydra.md) для скачивания маркет\-данных нумерует папки с историей, основываясь на таком же механизме. 

### Переопределение алгоритма генерации идентификаторов

Переопределение алгоритма генерации идентификаторов

1. Для того, чтобы начать генерировать идентификаторы инструментов по своему алгоритму, необходимо создать наследника класса [SecurityIdGenerator](xref:StockSharp.Algo.SecurityIdGenerator), и переопределить метод [SecurityIdGenerator.GenerateId](xref:StockSharp.Algo.SecurityIdGenerator.GenerateId): 

   ```cs
   class CustomSecurityIdGenerator : SecurityIdGenerator
   {
   	public override string GenerateId(string secCode, ExchangeBoard board)
   	{
   		// генерация идентификатора вида CODE--BOARD
   		return secCode + "--" + board.Code;
   	}
   }
   ```
2. Далее, созданный генератор нужно передать в шлюз: 

   ```cs
   var _connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
   ```

## См. также
