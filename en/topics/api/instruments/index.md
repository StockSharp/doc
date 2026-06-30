# Index

[IndexSecurity](xref:StockSharp.Algo.IndexSecurity) is an index built from instruments. For example, it can be used to define a spread for arbitrage or pairs trading. It has the following implementations:

1. [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) is an index built from a combination of several instruments using the mathematical formula in [ExpressionIndexSecurity.Formula](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Formula).

2. [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) is an index built from instruments using weighting factors from [WeightedIndexSecurity.Weights](xref:StockSharp.Algo.WeightedIndexSecurity.Weights).

## Creating ExpressionIndexSecurity

1. Declare the component instruments that will be included in [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity), and declare the index instrument itself:

   ```cs
   private Security _instr1;
   private Security _instr2;
   private ExpressionIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";
   							
   ```
2. Create the [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity):

   ```cs
   _indexInstr = new ExpressionIndexSecurity
   {
       Id = "IndexInstr@NASDAQ",
       Expression = "AAPL@NASDAQ/MSFT@NASDAQ",
       Board = ExchangeBoard.Nasdaq,
   };
   							
   ```

## Creating WeightedIndexSecurity

1. Declare the component instruments that will be included in [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity), and declare the index instrument itself:

   ```cs
   private Security _instr1;
   private Security _instr2;
   private WeightedIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";
   							
   ```
2. Create the [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity):

   ```cs
   _indexInstr = new WeightedIndexSecurity() { Board = ExchangeBoard.Nasdaq, Id = "IndexInstr" };
   							
   ```
3. Add the component instruments:

   ```cs
   _indexInstr.Weights.Add(_instr1.ToSecurityId(), 1);
   _indexInstr.Weights.Add(_instr2.ToSecurityId(), -1);
   							
   ```
