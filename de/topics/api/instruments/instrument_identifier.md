# Instrumentenbezeichner

In [S#](../../api.md) verwenden Instrumente aus unterschiedlichen Quellen eine einheitliche [Security.Id](xref:StockSharp.BusinessEntities.Security.Id). Dadurch bleibt der Code von Handelsalgorithmen unabhängig vom Verbindungstyp, etwa [OpenECry](../connectors/stock_market/openecry.md), [Rithmic](../connectors/stock_market/rithmic.md) oder [Interactive Brokers](../connectors/stock_market/interactive_brokers.md).

Instrumentenbezeichner verwenden die folgende Syntax: **\[instrument code\]@\[board code\]**. Für Aktien von Apple Inc. lautet der Bezeichner **AAPL@NASDAQ**. Bei Derivaten ist der Board-Code das Board, an dem der Kontrakt gehandelt wird. Der Juni-Futures-Kontrakt auf den ES-Index kann beispielsweise als **ESM5@NYSE** identifiziert werden.

> [!TIP]
> [Hydra](../../hydra.md) verwendet denselben Mechanismus, um Ordner mit historischen Marktdaten zu benennen.

## Überschreiben des Algorithmus zur Bezeichnergenerierung

1. Um Instrumentenbezeichner mit einem eigenen Algorithmus zu generieren, erstellen Sie einen Nachfolger der Klasse [SecurityIdGenerator](xref:StockSharp.Messages.SecurityIdGenerator) und überschreiben Sie die Methode [SecurityIdGenerator.GenerateId](xref:StockSharp.Messages.SecurityIdGenerator.GenerateId(System.String,System.String))**(**[System.String](xref:System.String) secCode, [System.String](xref:System.String) boardCode **)**:

   ```cs
   class CustomSecurityIdGenerator : SecurityIdGenerator
   {
      public override string GenerateId(string secCode, string boardCode)
      {
         // Bezeichner im Format CODE--BOARD erzeugen
         return secCode + "--" + boardCode;
      }
   }
   ```

2. Übergeben Sie den erstellten Generator an den Connector:

   ```cs
   connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
   ```

