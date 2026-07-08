# Fortlaufende Futures

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) ist ein kontinuierliches Instrument, üblicherweise ein Futures-Kontrakt, das Instrumente enthält, die einem Verfall unterliegen.

Betrachten Sie zum Beispiel zwei E-mini S&P 500 Futures: **ESM5** und **ESU5**. Wenn **ESM5** verfällt, wechselt das kontinuierliche Instrument automatisch zum nächsten Kontrakt, **ESU5**.

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) kann genauso gehandelt werden wie [Security](xref:StockSharp.BusinessEntities.Security). Vor dem Verfall von **ESM5** wird der Handel über dieses Instrument ausgeführt. Nach dem Verfall erfolgt der Handel über **ESU5** und so weiter.

## Erstellen von ExpirationContinuousSecurity

1. Deklarieren Sie die Komponenteninstrumente, die in [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) enthalten sein werden, und deklarieren Sie [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) selbst:

   ```cs
   private Security _esm5;
   private Security _esu5;
   private ExpirationContinuousSecurity _es;

   ```
2. Erstellen Sie [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity):

   ```cs
   _es = new ExpirationContinuousSecurity { Board = ExchangeBoard.Cme, Id = "ES" };

   ```
3. Fügen Sie Komponenteninstrumente hinzu und geben Sie für jedes Instrument Datum und Uhrzeit des Verfalls an:

   ```cs
   _es.ExpirationJumps.Add(_esm5.ToSecurityId(), new DateTime(2015, 6, 15, 18, 45, 00));
   _es.ExpirationJumps.Add(_esu5.ToSecurityId(), new DateTime(2015, 9, 15, 18, 45, 00));

   ```

## VolumeContinuousSecurity

Zusätzlich zu [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) stellt StockSharp auch [VolumeContinuousSecurity](xref:StockSharp.Algo.VolumeContinuousSecurity) bereit. Dieser Typ eines kontinuierlichen Instruments wechselt zwischen Kontrakten auf Basis des Handelsvolumens statt anhand des Verfallsdatums. Der Übergang zum nächsten Kontrakt erfolgt, wenn das Handelsvolumen des neuen Kontrakts das Volumen des aktuellen Kontrakts überschreitet.

