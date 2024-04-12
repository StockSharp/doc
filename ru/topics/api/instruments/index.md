# Индекс

[IndexSecurity](xref:StockSharp.Algo.IndexSecurity) \- индекс, построенный из инструментов. Например, для задания спреда при арбитраже или парном трейдинге. Имеет следующие реализации.

1. [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) \- индекс, построенный из комбинации нескольких инструментов через математическую формулу [ExpressionIndexSecurity.Formula](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Formula).

2. [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) \- индекс, построенный из инструментов с применением весовых коэффициентов [WeightedIndexSecurity.Weights](xref:StockSharp.Algo.WeightedIndexSecurity.Weights).

## Создание ExpressionIndexSecurity

1. Объявить составные инструменты, которые будут входить в [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity):

   ```cs
   // необходимо для компиляции формулы
   ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
   // или стандартный .NET компилятор, если нет последних обновлений
   //ConfigManager.RegisterService<ICompilerService>(new Fw40CompilerService(Directory.GetCurrentDirectory(), Directory.GetCurrentDirectory()));
   private Security _instr1;
   private Security _instr2;
   private ExpressionIndexSecurity _indexInstr;
   private const string _secCode1 = "GZM5";
   private const string _secCode2 = "LKM5";
   							
   ```
2. Создать [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity):

   ```cs
   _indexInstr = new ExpressionIndexSecurity
   {
       Id = "IndexInstr@FORTS",
       Expression = "GZM5@FORTS/LKM5@FORTS",
       Board = ExchangeBoard.Forts,
   };
   							
   ```

## Создание WeightedIndexSecurity

1. Объявить составные инструменты, которые будут входить в [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) и сам [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity):

   ```cs
   private Security _instr1;
   private Security _instr2;
   private WeightedIndexSecurity _indexInstr;
   private const string _secCode1 = "GZM5";
   private const string _secCode2 = "LKM5";
   							
   ```
2. Создать [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity):

   ```cs
   _indexInstr = new WeightedIndexSecurity() { ExchangeBoard = ExchangeBoard.Forts, Id = "IndexInstr" };
   							
   ```
3. Добавить в него составные инструменты:

   ```cs
   _indexInstr.Weights.Add(_instr1, 1);
   _indexInstr.Weights.Add(_instr2, -1);
   							
   ```
