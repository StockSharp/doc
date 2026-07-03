# Identificador de instrumento

En [S#](../../api.md), los instrumentos de distintas fuentes usan un [Security.Id](xref:StockSharp.BusinessEntities.Security.Id) unificado. Esto mantiene el código del algoritmo de trading independiente del tipo de conexión, como [OpenECry](../connectors/stock_market/openecry.md), [Rithmic](../connectors/stock_market/rithmic.md) o [Interactive Brokers](../connectors/stock_market/interactive_brokers.md).

Los identificadores de instrumentos usan la siguiente sintaxis: **\[código del instrumento\]@\[código de plaza\]**. Para las acciones de Apple Inc., el identificador es **AAPL@NASDAQ**. Para derivados, el código de plaza es la plaza en la que se negocia el contrato. Por ejemplo, el contrato de futuros de junio sobre el índice ES se puede identificar como **ESM5@NYSE**.

> [!TIP]
> [Hydra](../../hydra.md) usa el mismo mecanismo para nombrar carpetas con datos históricos de mercado.

## Sustitución del algoritmo de generación del identificador

1. Para generar identificadores de instrumentos con su propio algoritmo, cree un descendiente de la clase [SecurityIdGenerator](xref:StockSharp.Messages.SecurityIdGenerator) y sobrescriba el método [SecurityIdGenerator.GenerateId](xref:StockSharp.Messages.SecurityIdGenerator.GenerateId(System.String,System.String))**(**[System.String](xref:System.String) secCode, [System.String](xref:System.String) boardCode **)**:

   ```cs
   class CustomSecurityIdGenerator : SecurityIdGenerator
   {
      public override string GenerateId(string secCode, string boardCode)
      {
         // generar identificadores en formato CODE--BOARD
         return secCode + "--" + boardCode;
      }
   }
   ```

2. Pase el generador creado al conector:

   ```cs
   connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
   ```
