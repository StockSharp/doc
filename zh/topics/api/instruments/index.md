# Index

[IndexSecurity](xref:StockSharp.Algo.IndexSecurity) 是由多个交易品种构建的指数。例如，它可用于定义套利或配对交易中的价差。它有以下实现：

1. [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) 是通过 [ExpressionIndexSecurity.Formula](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Formula) 中的数学公式，将多个交易品种组合构建出的指数。

2. [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) 是使用 [WeightedIndexSecurity.Weights](xref:StockSharp.Algo.WeightedIndexSecurity.Weights) 中的权重因子，由多个交易品种构建出的指数。

## 创建 ExpressionIndexSecurity

1. 声明将包含在 [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) 中的组成交易品种，并声明指数交易品种本身：

   ```cs
   private Security _instr1;
   private Security _instr2;
   private ExpressionIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";
   							
   ```
2. 创建 [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity)：

   ```cs
   _indexInstr = new ExpressionIndexSecurity
   {
       Id = "IndexInstr@NASDAQ",
       Expression = "AAPL@NASDAQ/MSFT@NASDAQ",
       Board = ExchangeBoard.Nasdaq,
   };
   							
   ```

## 创建 WeightedIndexSecurity

1. 声明将包含在 [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) 中的组成交易品种，并声明指数交易品种本身：

   ```cs
   private Security _instr1;
   private Security _instr2;
   private WeightedIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";
   							
   ```
2. 创建 [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity)：

   ```cs
   _indexInstr = new WeightedIndexSecurity() { Board = ExchangeBoard.Nasdaq, Id = "IndexInstr" };
   							
   ```
3. 添加组成交易品种：

   ```cs
   _indexInstr.Weights.Add(_instr1.ToSecurityId(), 1);
   _indexInstr.Weights.Add(_instr2.ToSecurityId(), -1);
   							
   ```
