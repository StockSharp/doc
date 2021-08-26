# Index

[IndexSecurity](../api/StockSharp.Algo.IndexSecurity.html) \- the index built from instruments. For example, to set a spread in arbitrage or pair trading. It has the following implementations.

1. [ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html) \- is the index built from a combination of several instruments through the [Formula](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.Formula.html) mathematical formula.

2. [WeightedIndexSecurity](../api/StockSharp.Algo.WeightedIndexSecurity.html) \- is the index built from instruments using weighting factors [Weights](../api/StockSharp.Algo.WeightedIndexSecurity.Weights.html).

### The ExpressionIndexSecurity creating

The ExpressionIndexSecurity creating

1. To declare the compound instruments that will be included in [ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html) and in the [ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html) itself:

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
2. To create the [ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html):

   ```cs
   _indexInstr = new ExpressionIndexSecurity
   {
       Id = "IndexInstr@NYSE",
       Expression = "ESM5@NYSE/APM5@NYSE",
       Board = ExchangeBoard.Nyse,
   };
   							
   ```

### The WeightedIndexSecurity creating

The WeightedIndexSecurity creating

1. To declare the compound instruments that will be included in [WeightedIndexSecurity](../api/StockSharp.Algo.WeightedIndexSecurity.html) and in the [WeightedIndexSecurity](../api/StockSharp.Algo.WeightedIndexSecurity.html) itself:

   ```cs
   private Security _instr1;
   private Security _instr2;
   private WeightedIndexSecurity _indexInstr;
   private const string _secCode1 = "GZM5";
   private const string _secCode2 = "LKM5";
   							
   ```
2. To create the [WeightedIndexSecurity](../api/StockSharp.Algo.WeightedIndexSecurity.html):

   ```cs
   _indexInstr = new WeightedIndexSecurity() { ExchangeBoard = ExchangeBoard.Nyse, Id = "IndexInstr" };
   							
   ```
3. To add the compound instruments to it:

   ```cs
   _indexInstr.Weights.Add(_instr1, 1);
   _indexInstr.Weights.Add(_instr2, -1);
   							
   ```
