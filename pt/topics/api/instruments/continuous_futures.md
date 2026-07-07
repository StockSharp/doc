# Futuros contínuos

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) é um instrumento contínuo, normalmente um contrato de futuros, que contém instrumentos sujeitos a expiração.

Por exemplo, considere dois futuros E-mini S&P 500: **ESM5** e **ESU5**. Quando **ESM5** expira, o instrumento contínuo muda automaticamente para o contrato seguinte, **ESU5**.

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) pode ser negociado da mesma forma que [Security](xref:StockSharp.BusinessEntities.Security). Antes de **ESM5** expirar, a negociação é efetuada através desse instrumento. Após a expiração, a negociação é efetuada através de **ESU5**, e assim por diante.

## Criar ExpirationContinuousSecurity

1. Declare os instrumentos componentes que serão incluídos em [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) e declare o próprio [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity):

   ```cs
   private Security _esm5;
   private Security _esu5;
   private ExpirationContinuousSecurity _es;
   							
   ```
2. Crie o [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity):

   ```cs
   _es = new ExpirationContinuousSecurity { Board = ExchangeBoard.Cme, Id = "ES" };
   							
   ```
3. Adicione os instrumentos componentes e especifique a data e hora de expiração de cada um:

   ```cs
   _es.ExpirationJumps.Add(_esm5.ToSecurityId(), new DateTime(2015, 6, 15, 18, 45, 00));
   _es.ExpirationJumps.Add(_esu5.ToSecurityId(), new DateTime(2015, 9, 15, 18, 45, 00));

   ```

## VolumeContinuousSecurity

Além de [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity), o StockSharp também fornece [VolumeContinuousSecurity](xref:StockSharp.Algo.VolumeContinuousSecurity). Este tipo de instrumento contínuo alterna entre contratos com base no volume de negociação em vez da data de expiração. A transição para o contrato seguinte acontece quando o volume de negociação do novo contrato excede o volume do contrato atual.
