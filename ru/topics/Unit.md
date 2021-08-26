# Арифметические операции

Для упрощения работы с арифметическими операциями над такими величинами как проценты, пункты или пипсы можно использовать тип данных [Unit](../api/StockSharp.Messages.Unit.html). Он позволяет прозрачно оперировать с операциями сложения, вычитания, умножения и деления. [Unit](../api/StockSharp.Messages.Unit.html) можно конвертировать в [Decimal](../api/System.Decimal.html) (невозможно только если величина является процентной, то есть тип [Unit.Type](../api/StockSharp.Messages.Unit.Type.html) установлен в [UnitTypes.Percent](../api/StockSharp.Messages.UnitTypes.Percent.html)) и обратно (в этом случае всегда создается величина с абсолютным значением, то есть тип [Unit.Type](../api/StockSharp.Messages.Unit.Type.html) установлен в [UnitTypes.Absolute](../api/StockSharp.Messages.UnitTypes.Absolute.html)). 

### Использование Unit

Использование Unit

- Создавать [Unit](../api/StockSharp.Messages.Unit.html) можно через специальные конструкторы, или использовать более короткую запись с помощью [UnitHelper](../api/StockSharp.Messages.UnitHelper.html): 

  ```cs
  // тестовый инструмент с шагом цены в 1 копейку и стоимостью в 10 рублей
  // (в реальном приложении информацию необходимо получать через Connector.NewSecurity)
  var security = new Security
  {
  	Name = "Тестовый инструмент",
  	StepPrice = 10,
  	PriceStep = 0.01m,
  };
  			
  var absolute = new Unit(30);
  var percent = 30.0.Percents();
  var pips = 30.0.Pips(security);
  var point = 30.0.Points(security);
  ```
- [Unit](../api/StockSharp.Messages.Unit.html) имеет форматированный вывод. Поэтому приведение значений к строке в зависимости от типа [UnitTypes](../api/StockSharp.Messages.UnitTypes.html): 

  ```cs
  Console.WriteLine("absolute = " + absolute);
  Console.WriteLine("percent = " + percent);
  Console.WriteLine("pips = " + pips);
  Console.WriteLine("point = " + point);
  ```

  будет выводить следующие строчки:

  ```none
  absolute = 30
  percent = 30%
  pips = 30ш
  point = 30п
  ```

  Символ ш означает минимальный шаг цены (пипс), п \- стоимость шага цены (пункт).
- Арифметические операции над [Unit](../api/StockSharp.Messages.Unit.html) осуществляются так же, как и над обычными числами: 

  ```cs
  // сложение всех величин
  Console.WriteLine("testValue + absolute = " + (testValue + absolute));
  Console.WriteLine("testValue + percent = " + (testValue + percent));
  Console.WriteLine("testValue + pips = " + (testValue + pips));
  Console.WriteLine("testValue + point = " + (testValue + point));
  Console.WriteLine();
  // умножение всех величин
  Console.WriteLine("testValue * absolute = " + (testValue * absolute));
  Console.WriteLine("testValue * percent = " + (testValue * percent));
  Console.WriteLine("testValue * pips = " + (testValue * pips));
  Console.WriteLine("testValue * point = " + (testValue * point));
  Console.WriteLine();
  // вычитание всех величин
  Console.WriteLine("testValue - absolute = " + (testValue - absolute));
  Console.WriteLine("testValue - percent = " + (testValue - percent));
  Console.WriteLine("testValue - pips = " + (testValue - pips));
  Console.WriteLine("testValue - point = " + (testValue - point));
  Console.WriteLine();
  // деление всех величин
  Console.WriteLine("testValue / absolute = " + (testValue / absolute));
  Console.WriteLine("testValue / percent = " + (testValue / percent));
  Console.WriteLine("testValue / pips = " + (testValue / pips));
  Console.WriteLine("testValue / point = " + (testValue / point));
  Console.WriteLine();
  ```
- Результатом арифметических операций [Unit](../api/StockSharp.Messages.Unit.html) становится сам [Unit](../api/StockSharp.Messages.Unit.html), тип которого равен типу первого операнда. Например, если сложить пипсы и пункты, то результат будет в пипсах: 

  ```cs
  // сложение пипсов и пунктов
  var resultPipsPoint = pips + point;
  // и приведением из пипсов в decimal
  var resultPipsPointDecimal = (decimal)resultPipsPoint;
  Console.WriteLine("pips + point = " + resultPipsPoint);
  Console.WriteLine("(decimal)(pips + point) = " + resultPipsPointDecimal);
  ```

  Вывод такой операции будет следующим:

  ```none
  pips + point = 30030ш
  (decimal)(pips + point) = 300,3
  ```

  Или, если сложить пипсы и проценты: 

  ```cs
  // сложение пипсов и процентов
  var resultPipsPercents = pips + percent;
  // и приведением из пипсов в decimal
  var resultPipsPercentsDecimal = (decimal)resultPipsPercents;
  Console.WriteLine("pips + percent = " + resultPipsPercents);
  Console.WriteLine("(decimal)(pips + percent) = " + resultPipsPercentsDecimal);
  ```

  Вывод будет следующим:

  ```none
  pips + percent = 39ш
  (decimal)(pips + percent) = 0,39
  ```

### Следующие шаги

[Пользовательский интерфейс (GUI)](UIMarshalling.md)
