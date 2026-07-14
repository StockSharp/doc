# 单位类型

为了简化对百分比和绝对值等数值进行算术操作的工作，你可以使用 [Unit](xref:StockSharp.Messages.Unit) 数据类型。它允许透明地进行加法、减法、乘法和除法操作。[Unit](xref:StockSharp.Messages.Unit) 可以转换为 [Decimal](xref:System.Decimal)（仅当值为百分比时无法转换，即类型 [Unit.Type](xref:StockSharp.Messages.Unit.Type) 被设置为 [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent)），也可以从 Decimal 转换回来（在这种情况下，总是会创建一个绝对值，即类型 [Unit.Type](xref:StockSharp.Messages.Unit.Type) 被设置为 [UnitTypes.Absolute](xref:StockSharp.Messages.UnitTypes.Absolute)）。

## 使用单元

- 你可以使用特殊构造函数创建一个 [Unit](xref:StockSharp.Messages.Unit)，或者借助 [UnitHelper](xref:StockSharp.Messages.UnitHelper) 使用更简短的表示法：

  ```csharp
  // 创建绝对值
  var absolute = new Unit(30);
  
  // 创建百分比值
  var percent = 30.0.Percents();
  ```

- [Unit](xref:StockSharp.Messages.Unit) 有格式化输出。因此，根据类型 [UnitTypes](xref:StockSharp.Messages.UnitTypes) 将值转换为字符串：

  ```csharp
  Console.WriteLine("absolute = " + absolute);
  Console.WriteLine("percent = " + percent);
  ```

将输出以下几行：

  ```none
  absolute = 30
  percent = 30%
  ```

- 对 [Unit](xref:StockSharp.Messages.Unit) 的算术运算与对普通数字的运算方式相同：

  ```csharp
  // 数值相加
  Console.WriteLine("absolute + percent = " + (absolute + percent));
  
  // 数值相乘
  Console.WriteLine("absolute * percent = " + (absolute * percent));
  
  // 数值相减
  Console.WriteLine("absolute - percent = " + (absolute - percent));
  
  // 数值相除
  Console.WriteLine("absolute / percent = " + (absolute / percent));
  ```

- 对 [Unit](xref:StockSharp.Messages.Unit) 进行算术运算的结果本身会变成一个 [Unit](xref:StockSharp.Messages.Unit)，其类型等于第一个操作数的类型。例如，如果你将一个绝对值和一个百分比相加，结果将是绝对值：

  ```csharp
  // 绝对值和百分比相加
  var resultAbsolutePercents = absolute + percent;
  // 并从绝对值转换为 decimal
  var resultAbsolutePercentsDecimal = (decimal)resultAbsolutePercents;
  Console.WriteLine("absolute + percent = " + resultAbsolutePercents);
  Console.WriteLine("(decimal)(absolute + percent) = " + resultAbsolutePercentsDecimal);
  ```

此类操作的输出将如下所示：

  ```none
  absolute + percent = 39
  (decimal)(absolute + percent) = 39
  ```

- 你也可以使用熟悉的比较运算符来比较值：

  ```csharp
  if (absolute > percent)
      Console.WriteLine("绝对值大于百分比");
      
  if (absolute == percent)
      Console.WriteLine("数值相等");
  ```

- 要使一个值为正，您可以使用 `Abs()` 方法：

  ```csharp
  var negative = new Unit(-10);
  var positive = negative.Abs(); // 10
  ```

- 要将一个值乘以特定的因子，你可以使用 `Times()` 方法：

  ```csharp
  var multiplied = absolute.Times(5); // 150
  ```

## 限制

- 类型 [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent) 的值无法转换为其他类型
- 类型 [UnitTypes.Limit](xref:StockSharp.Messages.UnitTypes.Limit) 的值不能用于算术运算
- 不同类型的值之间的比较并不总是可能的

[Unit](xref:StockSharp.Messages.Unit) 类型提供了一种在交易应用中处理各种数值的一致方法，使表达与价格和交易量相关的计算更加容易。
