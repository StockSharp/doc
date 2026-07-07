# Índice

[IndexSecurity](xref:StockSharp.Algo.IndexSecurity) é um índice construído a partir de instrumentos. Por exemplo, pode ser utilizado para definir um spread para arbitragem ou negociação de pares. Tem as seguintes implementações:

1. [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) é um índice construído a partir de uma combinação de vários instrumentos utilizando a fórmula matemática em [ExpressionIndexSecurity.Formula](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Formula).

2. [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) é um índice construído a partir de instrumentos utilizando factores de ponderação de [WeightedIndexSecurity.Weights](xref:StockSharp.Algo.WeightedIndexSecurity.Weights).

## Criar ExpressionIndexSecurity

1. Declare os instrumentos componentes que serão incluídos em [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) e declare o próprio instrumento de índice:

   ```cs
   private Security _instr1;
   private Security _instr2;
   private ExpressionIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";
   							
   ```
2. Crie o [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity):

   ```cs
   _indexInstr = new ExpressionIndexSecurity
   {
       Id = "IndexInstr@NASDAQ",
       Expression = "AAPL@NASDAQ/MSFT@NASDAQ",
       Board = ExchangeBoard.Nasdaq,
   };
   							
   ```

## Criar WeightedIndexSecurity

1. Declare os instrumentos componentes que serão incluídos em [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) e declare o próprio instrumento de índice:

   ```cs
   private Security _instr1;
   private Security _instr2;
   private WeightedIndexSecurity _indexInstr;
   private const string _secCode1 = "AAPL";
   private const string _secCode2 = "MSFT";
   							
   ```
2. Crie o [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity):

   ```cs
   _indexInstr = new WeightedIndexSecurity() { Board = ExchangeBoard.Nasdaq, Id = "IndexInstr" };
   							
   ```
3. Adicione os instrumentos componentes:

   ```cs
   _indexInstr.Weights.Add(_instr1.ToSecurityId(), 1);
   _indexInstr.Weights.Add(_instr2.ToSecurityId(), -1);
   							
   ```
