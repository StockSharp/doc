# 索引

[IndexSecurity](xref:StockSharp.Algo.IndexSecurity) - 由工具构建的指数。例如，用于在套利或配对交易中设置价差。它具有以下实现方式。

1. [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) - 是通过 [ExpressionIndexSecurity.Formula](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Formula) 数学公式，从多个工具组合构建的指数。

2. [加权指数证券](xref:StockSharp.Algo.WeightedIndexSecurity) - 是使用权重因子[WeightedIndexSecurity.Weights](xref:StockSharp.Algo.WeightedIndexSecurity.Weights) 从工具构建的指数。

## 创建 ExpressionIndexSecurity

1. 声明将包含在 [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) 和 [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) 本身中的复合工具：

   ```cs
   ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
   // Or
   //ConfigManager.RegisterService<ICompilerService>(new Fw40CompilerService(Directory.GetCurrentDirectory(), Directory.GetCurrentDirectory()));
   private Security _instr1;
   private Security _instr2;
   private ExpressionIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";
   							
   ```
2. 要创建 [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity)：

   ```cs
   _indexInstr = new ExpressionIndexSecurity
   {
       Id = "IndexInstr@NYSE",
       Expression = "ESM5@NYSE/APM5@NYSE",
       Board = ExchangeBoard.Nyse,
   };
   							
   ```

## 创建加权指数证券

1. 声明将包含在 [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) 和 [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) 本身中的复合工具：

   ```cs
   private Security _instr1;
   private Security _instr2;
   private WeightedIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";
   							
   ```
2. 要创建 [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity)：

   ```cs
   _indexInstr = new WeightedIndexSecurity() { ExchangeBoard = ExchangeBoard.Nyse, Id = "IndexInstr" };
   							
   ```
3. 将复合乐器添加到其中：

   ```cs
   _indexInstr.Weights.Add(_instr1, 1);
   _indexInstr.Weights.Add(_instr2, -1);
   							
   ```
