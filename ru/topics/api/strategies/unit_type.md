# Тип Unit

Для упрощения работы с арифметическими операциями над такими величинами как проценты и абсолютные значения можно использовать тип данных [Unit](xref:StockSharp.Messages.Unit). Он позволяет прозрачно оперировать с операциями сложения, вычитания, умножения и деления. [Unit](xref:StockSharp.Messages.Unit) можно конвертировать в [Decimal](xref:System.Decimal) (невозможно только если величина является процентной, то есть тип [Unit.Type](xref:StockSharp.Messages.Unit.Type) установлен в [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent)) и обратно (в этом случае всегда создается величина с абсолютным значением, то есть тип [Unit.Type](xref:StockSharp.Messages.Unit.Type) установлен в [UnitTypes.Absolute](xref:StockSharp.Messages.UnitTypes.Absolute)).

## Использование Unit

- Создавать [Unit](xref:StockSharp.Messages.Unit) можно через специальные конструкторы или использовать более короткую запись с помощью [UnitHelper](xref:StockSharp.Messages.UnitHelper):

  ```csharp
  // создание абсолютного значения
  var absolute = new Unit(30);
  
  // создание процентного значения
  var percent = 30.0.Percents();
  ```

- [Unit](xref:StockSharp.Messages.Unit) имеет форматированный вывод. Поэтому приведение значений к строке в зависимости от типа [UnitTypes](xref:StockSharp.Messages.UnitTypes):

  ```csharp
  Console.WriteLine("absolute = " + absolute);
  Console.WriteLine("percent = " + percent);
  ```

  будет выводить следующие строчки:

  ```none
  absolute = 30
  percent = 30%
  ```

- Арифметические операции над [Unit](xref:StockSharp.Messages.Unit) осуществляются так же, как и над обычными числами:

  ```csharp
  // сложение величин
  Console.WriteLine("absolute + percent = " + (absolute + percent));
  
  // умножение величин
  Console.WriteLine("absolute * percent = " + (absolute * percent));
  
  // вычитание величин
  Console.WriteLine("absolute - percent = " + (absolute - percent));
  
  // деление величин
  Console.WriteLine("absolute / percent = " + (absolute / percent));
  ```

- Результатом арифметических операций [Unit](xref:StockSharp.Messages.Unit) становится сам [Unit](xref:StockSharp.Messages.Unit), тип которого равен типу первого операнда. Например, если сложить абсолютное значение и проценты, то результат будет в абсолютном значении:

  ```csharp
  // сложение абсолютного значения и процентов
  var resultAbsolutePercents = absolute + percent;
  // и приведением из абсолютного значения в decimal
  var resultAbsolutePercentsDecimal = (decimal)resultAbsolutePercents;
  Console.WriteLine("absolute + percent = " + resultAbsolutePercents);
  Console.WriteLine("(decimal)(absolute + percent) = " + resultAbsolutePercentsDecimal);
  ```

  Вывод таких операций будет следующим:

  ```none
  absolute + percent = 39
  (decimal)(absolute + percent) = 39
  ```

- Для сравнения значений также можно использовать привычные операторы сравнения:

  ```csharp
  if (absolute > percent)
      Console.WriteLine("Абсолютное значение больше процентного");
      
  if (absolute == percent)
      Console.WriteLine("Значения равны");
  ```

- Чтобы сделать значение положительным, можно использовать метод `Abs()`:

  ```csharp
  var negative = new Unit(-10);
  var positive = negative.Abs(); // 10
  ```

- Для умножения значения на определенный коэффициент можно использовать метод `Times()`:

  ```csharp
  var multiplied = absolute.Times(5); // 150
  ```

## Ограничения

- Значения типа [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent) нельзя преобразовать в другие типы
- Значения типа [UnitTypes.Limit](xref:StockSharp.Messages.UnitTypes.Limit) нельзя использовать в арифметических операциях
- Сравнение между значениями разных типов не всегда возможно

Тип [Unit](xref:StockSharp.Messages.Unit) предоставляет последовательный способ работы с различными видами значений в торговых приложениях, что упрощает выражение расчетов, связанных с ценой и объемом.