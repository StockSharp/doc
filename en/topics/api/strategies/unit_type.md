# Unit Type

To simplify working with arithmetic operations on values such as percentages and absolute values, you can use the [Unit](xref:StockSharp.Messages.Unit) data type. It allows transparent operations with addition, subtraction, multiplication, and division. [Unit](xref:StockSharp.Messages.Unit) can be converted to [Decimal](xref:System.Decimal) (except when the value is a percentage, i.e., the type [Unit.Type](xref:StockSharp.Messages.Unit.Type) is set to [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent)) and back (in this case, a value with an absolute value is always created, i.e., the type [Unit.Type](xref:StockSharp.Messages.Unit.Type) is set to [UnitTypes.Absolute](xref:StockSharp.Messages.UnitTypes.Absolute)).

## Using Unit

- You can create a [Unit](xref:StockSharp.Messages.Unit) using special constructors or use a shorter notation with the help of [UnitHelper](xref:StockSharp.Messages.UnitHelper):

  ```csharp
  // creating an absolute value
  var absolute = new Unit(30);
  
  // creating a percentage value
  var percent = 30.0.Percents();
  ```

- [Unit](xref:StockSharp.Messages.Unit) has formatted output. Therefore, converting values to a string depending on the type [UnitTypes](xref:StockSharp.Messages.UnitTypes):

  ```csharp
  Console.WriteLine("absolute = " + absolute);
  Console.WriteLine("percent = " + percent);
  ```

  will output the following lines:

  ```none
  absolute = 30
  percent = 30%
  ```

- Arithmetic operations on [Unit](xref:StockSharp.Messages.Unit) are performed in the same way as on regular numbers:

  ```csharp
  // addition of values
  Console.WriteLine("absolute + percent = " + (absolute + percent));
  
  // multiplication of values
  Console.WriteLine("absolute * percent = " + (absolute * percent));
  
  // subtraction of values
  Console.WriteLine("absolute - percent = " + (absolute - percent));
  
  // division of values
  Console.WriteLine("absolute / percent = " + (absolute / percent));
  ```

- The result of arithmetic operations on [Unit](xref:StockSharp.Messages.Unit) becomes a [Unit](xref:StockSharp.Messages.Unit) itself, the type of which equals the type of the first operand. For example, if you add an absolute value and a percentage, the result will be in an absolute value:

  ```csharp
  // addition of absolute value and percentage
  var resultAbsolutePercents = absolute + percent;
  // and converting from absolute value to decimal
  var resultAbsolutePercentsDecimal = (decimal)resultAbsolutePercents;
  Console.WriteLine("absolute + percent = " + resultAbsolutePercents);
  Console.WriteLine("(decimal)(absolute + percent) = " + resultAbsolutePercentsDecimal);
  ```

  The output of such operations will be as follows:

  ```none
  absolute + percent = 39
  (decimal)(absolute + percent) = 39
  ```

- You can also use familiar comparison operators to compare values:

  ```csharp
  if (absolute > percent)
      Console.WriteLine("Absolute value is greater than percentage");
      
  if (absolute == percent)
      Console.WriteLine("Values are equal");
  ```

- To make a value positive, you can use the `Abs()` method:

  ```csharp
  var negative = new Unit(-10);
  var positive = negative.Abs(); // 10
  ```

- To multiply a value by a specific factor, you can use the `Times()` method:

  ```csharp
  var multiplied = absolute.Times(5); // 150
  ```

## Limitations

- Values of type [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent) cannot be converted to other types
- Values of type [UnitTypes.Limit](xref:StockSharp.Messages.UnitTypes.Limit) cannot be used in arithmetic operations
- Comparison between values of different types is not always possible

The [Unit](xref:StockSharp.Messages.Unit) type provides a consistent way to work with various kinds of values in trading applications, making it easier to express calculations related to price and volume.