# Arithmetic operations

To simplify works with arithmetic operations on such values as percent, points or pips, the [Unit](xref:StockSharp.Messages.Unit) data type can be used. It allows transparently operate with operations of addition, subtraction, multiplication and division. [Unit](xref:StockSharp.Messages.Unit) may be converted into [Decimal](xref:System.Decimal) (not possible, of the value is percentage, i.e. the type [Unit.Type](xref:StockSharp.Messages.Unit.Type) is set to [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent)) and vice versa (in this case the created value is always of the absolute value, i.e. the [Unit.Type](xref:StockSharp.Messages.Unit.Type) is set to [UnitTypes.Absolute](xref:StockSharp.Messages.UnitTypes.Absolute)). 

### Usage of Unit

Usage of Unit

- The [Unit](xref:StockSharp.Messages.Unit) can be created using special design kits, or using shorter recording by [UnitHelper](xref:StockSharp.Messages.UnitHelper): 

  ```cs
  // test instrument with pips = 1 cent and points = 10 usd
  var security = new Security
  {
  	Id = "AAPL@NASDAQ",
  	StepPrice = 10,
  	PriceStep = 0.01m,
  };
  			
  var absolute = new Unit(30);
  var percent = 30.0.Percents();
  var pips = 30.0.Pips(security);
  var point = 30.0.Points(security);
  ```
- [Unit](xref:StockSharp.Messages.Unit) features formatted output. Therefore, reducing the values to a string depending on the [UnitTypes](xref:StockSharp.Messages.UnitTypes) type: 

  ```cs
  Console.WriteLine("absolute = " + absolute);
  Console.WriteLine("percent = " + percent);
  Console.WriteLine("pips = " + pips);
  Console.WriteLine("point = " + point);
  ```

  will output the following strings:

  ```none
  absolute = 30
  percent = 30%
  pips = 30s
  point = 30p
  ```

  The s symbol means minimal price increment (pips), p \- cost of price increment (point).
- Arithmetic operations with [Unit](xref:StockSharp.Messages.Unit) are performed in the same way, as with normal numbers: 

  ```cs
  // addition of all values
  Console.WriteLine("testValue + absolute = " + (testValue + absolute));
  Console.WriteLine("testValue + percent = " + (testValue + percent));
  Console.WriteLine("testValue + pips = " + (testValue + pips));
  Console.WriteLine("testValue + point = " + (testValue + point));
  Console.WriteLine();
  // multiplication of all values
  Console.WriteLine("testValue * absolute = " + (testValue * absolute));
  Console.WriteLine("testValue * percent = " + (testValue * percent));
  Console.WriteLine("testValue * pips = " + (testValue * pips));
  Console.WriteLine("testValue * point = " + (testValue * point));
  Console.WriteLine();
  // subtraction of values
  Console.WriteLine("testValue - absolute = " + (testValue - absolute));
  Console.WriteLine("testValue - percent = " + (testValue - percent));
  Console.WriteLine("testValue - pips = " + (testValue - pips));
  Console.WriteLine("testValue - point = " + (testValue - point));
  Console.WriteLine();
  // division of all values
  Console.WriteLine("testValue / absolute = " + (testValue / absolute));
  Console.WriteLine("testValue / percent = " + (testValue / percent));
  Console.WriteLine("testValue / pips = " + (testValue / pips));
  Console.WriteLine("testValue / point = " + (testValue / point));
  Console.WriteLine();
  ```
- The result of arithmetic operations [Unit](xref:StockSharp.Messages.Unit) itself becomes [Unit](xref:StockSharp.Messages.Unit), type of each is equal to the first operand type. For example, when adding pips and points, the result will be in pips: 

  ```cs
  // addition of pips and points
  var resultPipsPoint = pips + point;
  // and casting to decimal
  var resultPipsPointDecimal = (decimal)resultPipsPoint;
  Console.WriteLine("pips + point = " + resultPipsPoint);
  Console.WriteLine("(decimal)(pips + point) = " + resultPipsPointDecimal);
  ```

  Output of such operation will be as follows:

  ```none
  pips + point = 30030s
  (decimal)(pips + point) = 300.3
  ```

  Or, when adding pips and percent: 

  ```cs
  // addition of pips and percents
  var resultPipsPercents = pips + percent;
  // and casting to decimal
  var resultPipsPercentsDecimal = (decimal)resultPipsPercents;
  Console.WriteLine("pips + percent = " + resultPipsPercents);
  Console.WriteLine("(decimal)(pips + percent) = " + resultPipsPercentsDecimal);
  ```

  The output will be as follows:

  ```none
  pips + percent = 39s
  (decimal)(pips + percent) = 0.39
  ```

### Next Steps

[Currency converter](Currency.md)
