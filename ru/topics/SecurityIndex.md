# Индекс

[IndexSecurity](../api/StockSharp.Algo.IndexSecurity.html) \- индекс, построенный из инструментов. Например, для задания спреда при арбитраже или парном трейдинге. Имеет следующие реализации.

1. [ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html) \- индекс, построенный из комбинации нескольких инструментов через математическую формулу [Formula](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.Formula.html).

2. [WeightedIndexSecurity](../api/StockSharp.Algo.WeightedIndexSecurity.html) \- индекс, построенный из инструментов с применением весовых коэффициентов [Weights](../api/StockSharp.Algo.WeightedIndexSecurity.Weights.html).

### Создание ExpressionIndexSecurity

Создание ExpressionIndexSecurity

1. Объявить составные инструменты, которые будут входить в [ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html):

   ```cs
   \/\/ необходимо для компиляции формулы
   ConfigManager.RegisterService\<ICompilerService\>(new RoslynCompilerService());
   \/\/ или стандартный .NET компилятор, если нет последних обновлений
   \/\/ConfigManager.RegisterService\<ICompilerService\>(new Fw40CompilerService(Directory.GetCurrentDirectory(), Directory.GetCurrentDirectory()));
   private Security \_instr1;
   private Security \_instr2;
   private ExpressionIndexSecurity \_indexInstr;
   private const string \_secCode1 \= "GZM5";
   private const string \_secCode2 \= "LKM5";
   							
   ```
2. Создать [ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html):

   ```cs
   \_indexInstr \= new ExpressionIndexSecurity
   {
       Id \= "IndexInstr@FORTS",
       Expression \= "GZM5@FORTS\/LKM5@FORTS",
       Board \= ExchangeBoard.Forts,
   };
   							
   ```

### Создание WeightedIndexSecurity

Создание WeightedIndexSecurity

1. Объявить составные инструменты, которые будут входить в [WeightedIndexSecurity](../api/StockSharp.Algo.WeightedIndexSecurity.html) и сам [WeightedIndexSecurity](../api/StockSharp.Algo.WeightedIndexSecurity.html):

   ```cs
   private Security \_instr1;
   private Security \_instr2;
   private WeightedIndexSecurity \_indexInstr;
   private const string \_secCode1 \= "GZM5";
   private const string \_secCode2 \= "LKM5";
   							
   ```
2. Создать [WeightedIndexSecurity](../api/StockSharp.Algo.WeightedIndexSecurity.html):

   ```cs
   \_indexInstr \= new WeightedIndexSecurity() { ExchangeBoard \= ExchangeBoard.Forts, Id \= "IndexInstr" };
   							
   ```
3. Добавить в него составные инструменты:

   ```cs
   \_indexInstr.Weights.Add(\_instr1, 1);
   \_indexInstr.Weights.Add(\_instr2, \-1);
   							
   ```
