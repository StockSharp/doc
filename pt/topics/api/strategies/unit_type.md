# Tipo Unit

Para simplificar o trabalho com operações aritméticas sobre valores como percentagens e valores absolutos, pode usar o tipo de dados [Unit](xref:StockSharp.Messages.Unit). Permite operações transparentes de adição, subtração, multiplicação e divisão. [Unit](xref:StockSharp.Messages.Unit) pode ser convertido para [Decimal](xref:System.Decimal) (exceto quando o valor é uma percentagem, isto é, quando o tipo [Unit.Type](xref:StockSharp.Messages.Unit.Type) está definido como [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent)) e de volta (neste caso, é sempre criado um valor com valor absoluto, isto é, o tipo [Unit.Type](xref:StockSharp.Messages.Unit.Type) fica definido como [UnitTypes.Absolute](xref:StockSharp.Messages.UnitTypes.Absolute)).

## Usar Unit

- Pode criar um [Unit](xref:StockSharp.Messages.Unit) usando construtores especiais ou usar uma notação mais curta com a ajuda de [UnitHelper](xref:StockSharp.Messages.UnitHelper):

  ```csharp
  // criar um valor absoluto
  var absolute = new Unit(30);
  
  // criar um valor percentual
  var percent = 30.0.Percents();
  ```

- [Unit](xref:StockSharp.Messages.Unit) tem saída formatada. Portanto, a conversão de valores para uma string depende do tipo [UnitTypes](xref:StockSharp.Messages.UnitTypes):

  ```csharp
  Console.WriteLine("absolute = " + absolute);
  Console.WriteLine("percent = " + percent);
  ```

  produzirá as seguintes linhas:

  ```none
  absolute = 30
  percent = 30%
  ```

- As operações aritméticas sobre [Unit](xref:StockSharp.Messages.Unit) são executadas da mesma forma que sobre números normais:

  ```csharp
  // adição de valores
  Console.WriteLine("absolute + percent = " + (absolute + percent));
  
  // multiplicação de valores
  Console.WriteLine("absolute * percent = " + (absolute * percent));
  
  // subtração de valores
  Console.WriteLine("absolute - percent = " + (absolute - percent));
  
  // divisão de valores
  Console.WriteLine("absolute / percent = " + (absolute / percent));
  ```

- O resultado das operações aritméticas sobre [Unit](xref:StockSharp.Messages.Unit) torna-se ele próprio um [Unit](xref:StockSharp.Messages.Unit), cujo tipo é igual ao tipo do primeiro operando. Por exemplo, se adicionar um valor absoluto e uma percentagem, o resultado estará num valor absoluto:

  ```csharp
  // adição de valor absoluto e percentagem
  var resultAbsolutePercents = absolute + percent;
  // e conversão de valor absoluto para decimal
  var resultAbsolutePercentsDecimal = (decimal)resultAbsolutePercents;
  Console.WriteLine("absolute + percent = " + resultAbsolutePercents);
  Console.WriteLine("(decimal)(absolute + percent) = " + resultAbsolutePercentsDecimal);
  ```

  A saída dessas operações será a seguinte:

  ```none
  absolute + percent = 39
  (decimal)(absolute + percent) = 39
  ```

- Também pode usar operadores de comparação familiares para comparar valores:

  ```csharp
  if (absolute > percent)
      Console.WriteLine("Absolute value is greater than percentage");
      
  if (absolute == percent)
      Console.WriteLine("Values are equal");
  ```

- Para tornar um valor positivo, pode usar o método `Abs()`:

  ```csharp
  var negative = new Unit(-10);
  var positive = negative.Abs(); // 10
  ```

- Para multiplicar um valor por um fator específico, pode usar o método `Times()`:

  ```csharp
  var multiplied = absolute.Times(5); // 150
  ```

## Limitações

- Valores do tipo [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent) não podem ser convertidos para outros tipos
- Valores do tipo [UnitTypes.Limit](xref:StockSharp.Messages.UnitTypes.Limit) não podem ser usados em operações aritméticas
- A comparação entre valores de tipos diferentes nem sempre é possível

O tipo [Unit](xref:StockSharp.Messages.Unit) fornece uma forma consistente de trabalhar com vários tipos de valores em aplicações de negociação, facilitando a expressão de cálculos relacionados com preço e volume.

