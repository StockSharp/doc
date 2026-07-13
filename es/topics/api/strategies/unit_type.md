# Tipo Unit

Para simplificar el trabajo con operaciones aritméticas sobre valores como porcentajes y valores absolutos, puede usar el tipo de datos [Unit](xref:StockSharp.Messages.Unit). Permite operaciones transparentes de suma, resta, multiplicación y división. [Unit](xref:StockSharp.Messages.Unit) se puede convertir a [Decimal](xref:System.Decimal) (excepto cuando el valor es un porcentaje, es decir, el tipo [Unit.Type](xref:StockSharp.Messages.Unit.Type) está establecido en [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent)) y viceversa (en este caso, siempre se crea un valor absoluto, es decir, el tipo [Unit.Type](xref:StockSharp.Messages.Unit.Type) se establece en [UnitTypes.Absolute](xref:StockSharp.Messages.UnitTypes.Absolute)).

## Uso de Unit

- Puede crear un [Unit](xref:StockSharp.Messages.Unit) mediante constructores especiales o usar una notación más corta con ayuda de [UnitHelper](xref:StockSharp.Messages.UnitHelper):

  ```csharp
  // crear un valor absoluto
  var absolute = new Unit(30);

  // crear un valor porcentual
  var percent = 30.0.Percents();
  ```

- [Unit](xref:StockSharp.Messages.Unit) tiene salida formateada. Por lo tanto, al convertir valores a string según el tipo [UnitTypes](xref:StockSharp.Messages.UnitTypes):

  ```csharp
  Console.WriteLine("absolute = " + absolute);
  Console.WriteLine("percent = " + percent);
  ```

  se mostrarán las siguientes líneas:

  ```none
  absolute = 30
  percent = 30%
  ```

- Las operaciones aritméticas sobre [Unit](xref:StockSharp.Messages.Unit) se realizan de la misma forma que sobre números habituales:

  ```csharp
  // suma de valores
  Console.WriteLine("absolute + percent = " + (absolute + percent));

  // multiplicación de valores
  Console.WriteLine("absolute * percent = " + (absolute * percent));

  // resta de valores
  Console.WriteLine("absolute - percent = " + (absolute - percent));

  // división de valores
  Console.WriteLine("absolute / percent = " + (absolute / percent));
  ```

- El resultado de operaciones aritméticas sobre [Unit](xref:StockSharp.Messages.Unit) también es un [Unit](xref:StockSharp.Messages.Unit), cuyo tipo es igual al tipo del primer operando. Por ejemplo, si suma un valor absoluto y un porcentaje, el resultado estará en valor absoluto:

  ```csharp
  // suma de valor absoluto y porcentaje
  var resultAbsolutePercents = absolute + percent;
  // y conversión de valor absoluto a decimal
  var resultAbsolutePercentsDecimal = (decimal)resultAbsolutePercents;
  Console.WriteLine("absolute + percent = " + resultAbsolutePercents);
  Console.WriteLine("(decimal)(absolute + percent) = " + resultAbsolutePercentsDecimal);
  ```

  La salida de estas operaciones será la siguiente:

  ```none
  absolute + percent = 39
  (decimal)(absolute + percent) = 39
  ```

- También puede usar operadores de comparación habituales para comparar valores:

  ```csharp
  if (absolute > percent)
      Console.WriteLine("El valor absoluto es mayor que el porcentaje");

  if (absolute == percent)
      Console.WriteLine("Los valores son iguales");
  ```

- Para convertir un valor en positivo, puede usar el método `Abs()`:

  ```csharp
  var negative = new Unit(-10);
  var positive = negative.Abs(); // 10
  ```

- Para multiplicar un valor por un factor específico, puede usar el método `Times()`:

  ```csharp
  var multiplied = absolute.Times(5); // 150
  ```

## Limitaciones

- Los valores de tipo [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent) no se pueden convertir a otros tipos
- Los valores de tipo [UnitTypes.Limit](xref:StockSharp.Messages.UnitTypes.Limit) no se pueden usar en operaciones aritméticas
- La comparación entre valores de tipos distintos no siempre es posible

El tipo [Unit](xref:StockSharp.Messages.Unit) proporciona una forma coherente de trabajar con varios tipos de valores en aplicaciones de negociación, lo que facilita expresar cálculos relacionados con precio y volumen.
