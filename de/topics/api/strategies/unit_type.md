# Unit-Typ

Um die Arbeit mit arithmetischen Operationen auf Werten wie Prozentangaben und absoluten Werten zu vereinfachen, können Sie den Datentyp [Unit](xref:StockSharp.Messages.Unit) verwenden. Er ermöglicht transparente Operationen mit Addition, Subtraktion, Multiplikation und Division. [Unit](xref:StockSharp.Messages.Unit) kann in [Decimal](xref:System.Decimal) konvertiert werden, außer wenn der Wert ein Prozentwert ist, d. h. wenn der Typ [Unit.Type](xref:StockSharp.Messages.Unit.Type) auf [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent) gesetzt ist. Die Konvertierung zurück ist ebenfalls möglich; in diesem Fall wird immer ein Wert mit absolutem Wert erstellt, d. h. der Typ [Unit.Type](xref:StockSharp.Messages.Unit.Type) wird auf [UnitTypes.Absolute](xref:StockSharp.Messages.UnitTypes.Absolute) gesetzt.

## Verwendung von Unit

- Sie können eine [Unit](xref:StockSharp.Messages.Unit) mit speziellen Konstruktoren erstellen oder mithilfe von [UnitHelper](xref:StockSharp.Messages.UnitHelper) eine kürzere Schreibweise verwenden:

  ```csharp
  // absoluten Wert erstellen
  var absolute = new Unit(30);

  // Prozentwert erstellen
  var percent = 30.0.Percents();
  ```

- [Unit](xref:StockSharp.Messages.Unit) verfügt über eine formatierte Ausgabe. Daher werden Werte abhängig vom Typ [UnitTypes](xref:StockSharp.Messages.UnitTypes) in eine Zeichenfolge umgewandelt:

  ```csharp
  Console.WriteLine("absolute = " + absolute);
  Console.WriteLine("percent = " + percent);
  ```

  gibt die folgenden Zeilen aus:

  ```none
  absolute = 30
  percent = 30%
  ```

- Arithmetische Operationen mit [Unit](xref:StockSharp.Messages.Unit) werden genauso ausgeführt wie mit normalen Zahlen:

  ```csharp
  // Addition von Werten
  Console.WriteLine("absolute + percent = " + (absolute + percent));

  // Multiplikation von Werten
  Console.WriteLine("absolute * percent = " + (absolute * percent));

  // Subtraktion von Werten
  Console.WriteLine("absolute - percent = " + (absolute - percent));

  // Division von Werten
  Console.WriteLine("absolute / percent = " + (absolute / percent));
  ```

- Das Ergebnis arithmetischer Operationen mit [Unit](xref:StockSharp.Messages.Unit) ist selbst wieder eine [Unit](xref:StockSharp.Messages.Unit), deren Typ dem Typ des ersten Operanden entspricht. Wenn Sie beispielsweise einen absoluten Wert und einen Prozentwert addieren, liegt das Ergebnis als absoluter Wert vor:

  ```csharp
  // Addition von absolutem Wert und Prozentwert
  var resultAbsolutePercents = absolute + percent;
  // und Konvertierung vom absoluten Wert in decimal
  var resultAbsolutePercentsDecimal = (decimal)resultAbsolutePercents;
  Console.WriteLine("absolute + percent = " + resultAbsolutePercents);
  Console.WriteLine("(decimal)(absolute + percent) = " + resultAbsolutePercentsDecimal);
  ```

  Die Ausgabe solcher Operationen sieht wie folgt aus:

  ```none
  absolute + percent = 39
  (decimal)(absolute + percent) = 39
  ```

- Sie können auch vertraute Vergleichsoperatoren verwenden, um Werte zu vergleichen:

  ```csharp
  if (absolute > percent)
      Console.WriteLine("Absoluter Wert ist größer als Prozentwert");

  if (absolute == percent)
      Console.WriteLine("Werte sind gleich");
  ```

- Um einen Wert positiv zu machen, können Sie die Methode `Abs()` verwenden:

  ```csharp
  var negative = new Unit(-10);
  var positive = negative.Abs(); // 10
  ```

- Um einen Wert mit einem bestimmten Faktor zu multiplizieren, können Sie die Methode `Times()` verwenden:

  ```csharp
  var multiplied = absolute.Times(5); // 150
  ```

## Einschränkungen

- Werte vom Typ [UnitTypes.Percent](xref:StockSharp.Messages.UnitTypes.Percent) können nicht in andere Typen konvertiert werden.
- Werte vom Typ [UnitTypes.Limit](xref:StockSharp.Messages.UnitTypes.Limit) können nicht in arithmetischen Operationen verwendet werden.
- Vergleiche zwischen Werten unterschiedlicher Typen sind nicht immer möglich.

Der Typ [Unit](xref:StockSharp.Messages.Unit) bietet eine konsistente Möglichkeit, mit verschiedenen Arten von Werten in Handelsanwendungen zu arbeiten, und erleichtert dadurch die Darstellung von Berechnungen in Bezug auf Preis und Volumen.
