# Идентификатор инструмента

В [S\#](../../api.md) инструменты из разных источников имеют унифицированные идентификаторы [Security.Id](xref:StockSharp.BusinessEntities.Security.Id). Это сделано для того, чтобы код торгового робота не зависел от типа подключения ([Plaza II](../connectors/russia/plaza.md), [Quik](../connectors/russia/quik.md), [Tinkoff](../connectors/russia/tinkoff.md) и т.д.). Для идентификатора инструмента используется следующий синтаксис \- **\[код инструмента\]@\[код площадки\]**. Например, для акций Лукойла идентификатором будет **LKOH@EQBR**. Для инструментов срочного рынка [MOEX](https://moex.com/) площадкой будет "FORTS". Так, например, для июньского фьючерса на индекс РТС идентификатором будет **RIM5@FORTS**. 

> [!TIP]
> Программа [Hydra](../../hydra.md) для скачивания маркет\-данных нумерует папки с историей, основываясь на таком же механизме. 

## Переопределение алгоритма генерации идентификаторов

1. Для того, чтобы начать генерировать идентификаторы инструментов по своему алгоритму, необходимо создать наследника класса [SecurityIdGenerator](xref:StockSharp.Messages.SecurityIdGenerator), и переопределить метод [SecurityIdGenerator.GenerateId](xref:StockSharp.Messages.SecurityIdGenerator.GenerateId(System.String,System.String))**(**[System.String](xref:System.String) secCode, [System.String](xref:System.String) boardCode **)**: 

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
   connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
   ```
