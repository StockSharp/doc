# Futuros continuos

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) es un instrumento continuo, normalmente un contrato de futuros, que contiene instrumentos sujetos a vencimiento.

Por ejemplo, considere dos futuros E-mini S&P 500: **ESM5** y **ESU5**. Cuando **ESM5** vence, el instrumento continuo cambia automáticamente al siguiente contrato, **ESU5**.

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) se puede negociar del mismo modo que [Security](xref:StockSharp.BusinessEntities.Security). Antes de que venza **ESM5**, la negociación se realiza mediante ese instrumento. Después del vencimiento, la negociación se realiza mediante **ESU5**, y así sucesivamente.

## Creación de ExpirationContinuousSecurity

1. Declare los instrumentos componentes que se incluirán en [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) y declare el propio [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity):

   ```cs
   private Security _esm5;
   private Security _esu5;
   private ExpirationContinuousSecurity _es;

   ```
2. Cree el [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity):

   ```cs
   _es = new ExpirationContinuousSecurity { Board = ExchangeBoard.Cme, Id = "ES" };

   ```
3. Agregue los instrumentos componentes y especifique la fecha y hora de vencimiento de cada uno:

   ```cs
   _es.ExpirationJumps.Add(_esm5.ToSecurityId(), new DateTime(2015, 6, 15, 18, 45, 00));
   _es.ExpirationJumps.Add(_esu5.ToSecurityId(), new DateTime(2015, 9, 15, 18, 45, 00));

   ```

## VolumeContinuousSecurity

Además de [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity), StockSharp también proporciona [VolumeContinuousSecurity](xref:StockSharp.Algo.VolumeContinuousSecurity). Este tipo de instrumento continuo cambia entre contratos en función del volumen de negociación en lugar de la fecha de vencimiento. La transición al siguiente contrato ocurre cuando el volumen de negociación del nuevo contrato supera el volumen del contrato actual.
