# Identificador do instrumento

Em [S#](../../api.md), os instrumentos de diferentes fontes utilizam um [Security.Id](xref:StockSharp.BusinessEntities.Security.Id) unificado. Isto mantém o código do algoritmo de negociação independente do tipo de ligação, como [OpenECry](../connectors/stock_market/openecry.md), [Rithmic](../connectors/stock_market/rithmic.md) ou [Interactive Brokers](../connectors/stock_market/interactive_brokers.md).

Os identificadores de instrumentos utilizam a seguinte sintaxe: **\[código do instrumento\]@\[código da bolsa\]**. Para acções da Apple Inc., o identificador é **AAPL@NASDAQ**. Para derivados, o código da bolsa é a bolsa onde o contrato é negociado. Por exemplo, o contrato de futuros de Junho sobre o índice ES pode ser identificado como **ESM5@NYSE**.

> [!TIP]
> [Hydra](../../hydra.md) utiliza o mesmo mecanismo para nomear pastas com dados históricos de mercado.

## Substituir o algoritmo de geração do identificador

1. Para gerar identificadores de instrumentos com o seu próprio algoritmo, crie um descendente da classe [SecurityIdGenerator](xref:StockSharp.Messages.SecurityIdGenerator) e substitua o método [SecurityIdGenerator.GenerateId](xref:StockSharp.Messages.SecurityIdGenerator.GenerateId(System.String,System.String))**(**[System.String](xref:System.String) secCode, [System.String](xref:System.String) boardCode **)**:

   ```cs
   class CustomSecurityIdGenerator : SecurityIdGenerator
   {
      public override string GenerateId(string secCode, string boardCode)
      {
         // gerar identificadores no formato CODE--BOARD
         return secCode + "--" + boardCode;
      }
   }
   ```

2. Passe o gerador criado ao conector:

   ```cs
   connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
   ```
