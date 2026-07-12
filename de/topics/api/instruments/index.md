# Index

[IndexSecurity](xref:StockSharp.Algo.IndexSecurity) ist ein aus Instrumenten aufgebauter Index. Er kann beispielsweise verwendet werden, um einen Spread für Arbitrage oder Paarhandel zu definieren. Es gibt folgende Implementierungen:

1. [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) ist ein Index, der aus einer Kombination mehrerer Instrumente mithilfe einer mathematischen Formel in [ExpressionIndexSecurity.Formula](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Formula) gebildet wird.

2. [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) ist ein Index, der aus Instrumenten mithilfe von Gewichtungsfaktoren aus [WeightedIndexSecurity.Weights](xref:StockSharp.Algo.WeightedIndexSecurity.Weights) gebildet wird.

## Erstellen von ExpressionIndexSecurity

1. Deklarieren Sie die Komponenteninstrumente, die in [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) enthalten sein werden, und deklarieren Sie das Indexinstrument selbst:

   ```cs
   private Security _instr1;
   private Security _instr2;
   private ExpressionIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";

   ```
2. Erstellen Sie [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity):

   ```cs
   _indexInstr = new ExpressionIndexSecurity
   {
       Id = "IndexInstr@NASDAQ",
       Expression = "AAPL@NASDAQ/MSFT@NASDAQ",
       Board = ExchangeBoard.Nasdaq,
   };

   ```

## Erstellen von WeightedIndexSecurity

1. Deklarieren Sie die Komponenteninstrumente, die in [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) enthalten sein werden, und deklarieren Sie das Indexinstrument selbst:

   ```cs
   private Security _instr1;
   private Security _instr2;
   private WeightedIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";

   ```
2. Erstellen Sie [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity):

   ```cs
   _indexInstr = new WeightedIndexSecurity() { Board = ExchangeBoard.Nasdaq, Id = "IndexInstr" };

   ```
3. Fügen Sie die Komponenteninstrumente hinzu:

   ```cs
   _indexInstr.Weights.Add(_instr1.ToSecurityId(), 1);
   _indexInstr.Weights.Add(_instr2.ToSecurityId(), -1);

   ```

