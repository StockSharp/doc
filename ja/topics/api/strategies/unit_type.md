# Unit 型

パーセンテージや絶対値などの値に対する算術演算を簡略化するために、[Unit](xref:StockSharp.Messages.Unit) データ型を使用できます。これにより、加算、減算、乗算、除算を透過的に行えます。[Unit](xref:StockSharp.Messages.Unit) は [Decimal](xref:System.Decimal) に変換でき（値がパーセンテージの場合、つまり [Unit.Type](xref:StockSharp.Messages.Unit.Type) 型が [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent) に設定されている場合を除く）、またその逆にも変換できます（この場合、常に絶対値の値が作成されます。つまり [Unit.Type](xref:StockSharp.Messages.Unit.Type) 型は [UnitTypes.Absolute](xref:StockSharp.Messages.UnitTypes.Absolute) に設定されます）。

## Unit の使用

- 特殊なコンストラクターを使用して [Unit](xref:StockSharp.Messages.Unit) を作成できます。または [UnitHelper](xref:StockSharp.Messages.UnitHelper) を利用した短い表記を使用できます。

  ```csharp
  // 絶対値を作成
  var absolute = new Unit(30);
  
  // パーセンテージ値を作成
  var percent = 30.0.Percents();
  ```

- [Unit](xref:StockSharp.Messages.Unit) には書式付き出力があります。したがって、[UnitTypes](xref:StockSharp.Messages.UnitTypes) 型に応じて値を文字列に変換すると、次のようになります。

  ```csharp
  Console.WriteLine("absolute = " + absolute);
  Console.WriteLine("percent = " + percent);
  ```

  次の行が出力されます。

  ```none
  absolute = 30
  percent = 30%
  ```

- [Unit](xref:StockSharp.Messages.Unit) に対する算術演算は、通常の数値と同じ方法で実行されます。

  ```csharp
  // 値の加算
  Console.WriteLine("absolute + percent = " + (absolute + percent));
  
  // 値の乗算
  Console.WriteLine("absolute * percent = " + (absolute * percent));
  
  // 値の減算
  Console.WriteLine("absolute - percent = " + (absolute - percent));
  
  // 値の除算
  Console.WriteLine("absolute / percent = " + (absolute / percent));
  ```

- [Unit](xref:StockSharp.Messages.Unit) に対する算術演算の結果は、それ自体も [Unit](xref:StockSharp.Messages.Unit) になり、その型は最初のオペランドの型と等しくなります。たとえば、絶対値とパーセンテージを加算した場合、結果は絶対値になります。

  ```csharp
  // 絶対値とパーセンテージの加算
  var resultAbsolutePercents = absolute + percent;
  // 絶対値から decimal への変換
  var resultAbsolutePercentsDecimal = (decimal)resultAbsolutePercents;
  Console.WriteLine("absolute + percent = " + resultAbsolutePercents);
  Console.WriteLine("(decimal)(absolute + percent) = " + resultAbsolutePercentsDecimal);
  ```

  このような演算の出力は次のようになります。

  ```none
  absolute + percent = 39
  (decimal)(absolute + percent) = 39
  ```

- 値を比較するには、よく使われる比較演算子も使用できます。

  ```csharp
  if (absolute > percent)
      Console.WriteLine("Absolute value is greater than percentage");
      
  if (absolute == percent)
      Console.WriteLine("Values are equal");
  ```

- 値を正にするには、`Abs()` メソッドを使用できます。

  ```csharp
  var negative = new Unit(-10);
  var positive = negative.Abs(); // 10
  ```

- 値に特定の係数を掛けるには、`Times()` メソッドを使用できます。

  ```csharp
  var multiplied = absolute.Times(5); // 150
  ```

## 制限事項

- [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent) 型の値は他の型に変換できません
- [UnitTypes.Limit](xref:StockSharp.Messages.UnitTypes.Limit) 型の値は算術演算で使用できません
- 異なる型の値同士の比較は、常に可能とは限りません

[Unit](xref:StockSharp.Messages.Unit) 型は、取引アプリケーションでさまざまな種類の値を扱うための一貫した方法を提供し、価格や数量に関連する計算を表現しやすくします。
