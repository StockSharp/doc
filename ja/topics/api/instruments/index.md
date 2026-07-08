# 指数

[IndexSecurity](xref:StockSharp.Algo.IndexSecurity) は、金融商品から構築される指数です。たとえば、裁定取引やペアトレードのためのスプレッドを定義するために使用できます。次の実装があります。

1. [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) は、[ExpressionIndexSecurity.Formula](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Formula) の数式を使用して、複数の金融商品の組み合わせから構築される指数です。

2. [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) は、[WeightedIndexSecurity.Weights](xref:StockSharp.Algo.WeightedIndexSecurity.Weights) の重み係数を使用して、金融商品から構築される指数です。

## ExpressionIndexSecurity の作成

1. [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) に含める構成金融商品を宣言し、指数金融商品自体も宣言します。

   ```cs
   private Security _instr1;
   private Security _instr2;
   private ExpressionIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";
   							
   ```
2. [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) を作成します。

   ```cs
   _indexInstr = new ExpressionIndexSecurity
   {
       Id = "IndexInstr@NASDAQ",
       Expression = "AAPL@NASDAQ/MSFT@NASDAQ",
       Board = ExchangeBoard.Nasdaq,
   };
   							
   ```

## WeightedIndexSecurity の作成

1. [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) に含める構成金融商品を宣言し、指数金融商品自体も宣言します。

   ```cs
   private Security _instr1;
   private Security _instr2;
   private WeightedIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";
   							
   ```
2. [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) を作成します。

   ```cs
   _indexInstr = new WeightedIndexSecurity() { Board = ExchangeBoard.Nasdaq, Id = "IndexInstr" };
   							
   ```
3. 構成金融商品を追加します。

   ```cs
   _indexInstr.Weights.Add(_instr1.ToSecurityId(), 1);
   _indexInstr.Weights.Add(_instr2.ToSecurityId(), -1);
   							
   ```
