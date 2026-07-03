# RMI

**Relative Momentum Index (RMI)** es una modificación del indicador tradicional RSI, propuesto por Roger Altman. A diferencia del clásico RSI, que calcula la relación entre aumentos y disminuciones de precios durante un período determinado, RMI tiene en cuenta el cambio relativo de precios durante un período de impulso seleccionado.

Para utilizar el indicador, debe utilizar la clase [RelativeMomentumIndex](xref:StockSharp.Algo.Indicators.RelativeMomentumIndex).

## Descripción

El Relative Momentum Index (RMI) mejora el clásico RSI agregando un parámetro de período de impulso. Esto permite a los operadores ajustar la sensibilidad del indicador sin cambiar el período de cálculo principal.

Al igual que RSI, RMI oscila entre 0 y 100:
- Los valores superiores a 70 suelen indicar un mercado sobrecomprado.
- Los valores por debajo de 30 indican un mercado sobrevendido.
- La línea central en 50 sirve como punto de referencia para determinar la dirección principal del movimiento del mercado.

RMI es particularmente útil para identificar posibles puntos de inversión de tendencia y confirmar la fuerza de la tendencia actual.

## Parámetros

- **MomentumPeriod**: período de impulso que define el desfase de tiempo para la comparación de precios.
- **Length** - período principal para calcular el indicador (similar al período en RSI).

## Cálculo

El cálculo de RMI se realiza en varios pasos:

1. Calcule el impulso como la diferencia entre el precio actual y el precio de hace n períodos:
   ```
   Momentum = Price(current) - Price(current - MomentumPeriod)
   ```

2. Divida los impulsos en positivos (U) y negativos (D):
   ```
   If Momentum > 0, then U = Momentum, D = 0
   If Momentum < 0, then U = 0, D = |Momentum|
   ```

3. Calcule los valores promedio de los impulsos positivos y negativos durante el período especificado:
   ```
   AverageU = SMA(U, Length)
   AverageD = SMA(D, Length)
   ```

4. Calcular la fuerza relativa:
   ```
   RS = AverageU / AverageD
   ```

5. Convertir a Relative Momentum Index:
   ```
   RMI = 100 - (100 / (1 + RS))
   ```

![IndicatorRelativeMomentumIndex](../../../../images/indicator_relative_momentum_index.png)

## Véase también

[RSI](rsi.md)
[Momentum](momentum.md)