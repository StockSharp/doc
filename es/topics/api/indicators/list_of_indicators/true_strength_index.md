# índice de fuerza verdadera

El **índice de fuerza verdadera (TSI)** es un oscilador de impulso creado por William Blau. Aplica doble suavizado a la diferencia.
entre precios de cierre consecutivos, lo que ayuda a identificar tendencias y puntos de inflexión con menos retraso en comparación con muchos clásicos
osciladores.

Utilice la clase [TrueStrengthIndex](xref:StockSharp.Algo.Indicators.TrueStrengthIndex) para acceder al indicador.

## Cálculo

1. Calcule el cambio de precio `m = Close − PreviousClose`.
2. Aplique dos medias móviles exponenciales con períodos `Length1` y `Length2` a `m` y `|m|`.
3. Calcule la relación entre el impulso doblemente suavizado y el impulso absoluto doblemente suavizado:
   `TSI = 100 × EMA(EMA(m, Length1), Length2) / EMA(EMA(|m|, Length1), Length2)`.
4. Opcionalmente, obtenga una línea de señal tomando un EMA del TSI con un período **Señal**.

## Parámetros

- **Length1**: primer período de suavizado.
- **Length2**: segundo período de suavizado.
- **Señal** — período de la línea de señal (opcional).

## Interpretación

- **TSI > 0** — impulso alcista.
- **TSI < 0** — impulso bajista.
- **Cruces de línea de señal** proporciona entradas de trading.
- **Divergencias** entre TSI y el precio advierten sobre posibles reversiones.

Debido al doble suavizado y normalización, el indicador filtra el ruido pero sigue respondiendo en comparación con los simples.
cálculos de impulso.

![indicator_true_strength_index](../../../../images/indicator_true_strength_index.png)

## Véase también

[Momentum](momentum.md)
[MACD](macd.md)
[RSI](rsi.md)
