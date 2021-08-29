# Index

[IndexSecurity](xref:StockSharp.Algo.IndexSecurity) \- the index built from instruments. For example, to set a spread in arbitrage or pair trading. It has the following implementations.

1. [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) \- is the index built from a combination of several instruments through the [Formula](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Formula) mathematical formula.

2. [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) \- is the index built from instruments using weighting factors [Weights](xref:StockSharp.Algo.WeightedIndexSecurity.Weights).

## The ExpressionIndexSecurity creating

1. To declare the compound instruments that will be included in [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) and in the [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) itself:

   ```cs
   ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
   // Or
   //ConfigManager.RegisterService<ICompilerService>(new Fw40CompilerService(Directory.GetCurrentDirectory(), Directory.GetCurrentDirectory()));
   private Security _instr1;
   private Security _instr2;
   private ExpressionIndexSecurity _indexInstr;
   private const string _secCode1 = "GZM5";
   private const string _secCode2 = "LKM5";
   							
   ```
2. To create the [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity):

   ```cs
   _indexInstr = new ExpressionIndexSecurity
   {
       Id = "IndexInstr@NYSE",
       Expression = "ESM5@NYSE/APM5@NYSE",
       Board = ExchangeBoard.Nyse,
   };
   							
   ```

## The WeightedIndexSecurity creating

1. To declare the compound instruments that will be included in [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) and in the [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) itself:

   ```cs
   private Security _instr1;
   private Security _instr2;
   private WeightedIndexSecurity _indexInstr;
   private const string _secCode1 = "GZM5";
   private const string _secCode2 = "LKM5";
   							
   ```
2. To create the [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity):

   ```cs
   _indexInstr = new WeightedIndexSecurity() { ExchangeBoard = ExchangeBoard.Nyse, Id = "IndexInstr" };
   							
   ```
3. To add the compound instruments to it:

   ```cs
   _indexInstr.Weights.Add(_instr1, 1);
   _indexInstr.Weights.Add(_instr2, -1);
   							
   ```
