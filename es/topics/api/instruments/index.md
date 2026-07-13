# Índice

[IndexSecurity](xref:StockSharp.Algo.IndexSecurity) es un índice construido a partir de instrumentos. Por ejemplo, se puede usar para definir un spread para arbitraje o negociación de pares. Tiene las siguientes implementaciones:

1. [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) es un índice construido a partir de una combinación de varios instrumentos mediante la fórmula matemática en [ExpressionIndexSecurity.Formula](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Formula).

2. [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) es un índice construido a partir de instrumentos mediante factores de ponderación de [WeightedIndexSecurity.Weights](xref:StockSharp.Algo.WeightedIndexSecurity.Weights).

## Creación de ExpressionIndexSecurity

1. Declare los instrumentos componentes que se incluirán en [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) y declare el propio instrumento índice:

   ```cs
   private Security _instr1;
   private Security _instr2;
   private ExpressionIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";

   ```
2. Cree el [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity):

   ```cs
   _indexInstr = new ExpressionIndexSecurity
   {
       Id = "IndexInstr@NASDAQ",
       Expression = "AAPL@NASDAQ/MSFT@NASDAQ",
       Board = ExchangeBoard.Nasdaq,
   };

   ```

## Creación de WeightedIndexSecurity

1. Declare los instrumentos componentes que se incluirán en [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) y declare el propio instrumento índice:

   ```cs
   private Security _instr1;
   private Security _instr2;
   private WeightedIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";

   ```
2. Cree el [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity):

   ```cs
   _indexInstr = new WeightedIndexSecurity() { Board = ExchangeBoard.Nasdaq, Id = "IndexInstr" };

   ```
3. Agregue los instrumentos componentes:

   ```cs
   _indexInstr.Weights.Add(_instr1.ToSecurityId(), 1);
   _indexInstr.Weights.Add(_instr2.ToSecurityId(), -1);

   ```
