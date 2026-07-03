# STC

**Schaff Trend Cycle (STC)** es un indicador de impulso desarrollado por Doug Schaff. STC se basa en el supuesto de que los ciclos del mercado se mueven con mayor frecuencia entre condiciones de sobrecompra y sobreventa que en una tendencia verdadera.

Para utilizar el indicador, debe utilizar la clase [SchaffTrendCycle](xref:StockSharp.Algo.Indicators.SchaffTrendCycle).

## Descripción

El Schaff Trend Cycle combina las ventajas del Stochastic Oscillator, MACD y el análisis cíclico. Este indicador puede reaccionar a los cambios de tendencia más rápido que los indicadores tradicionales como MACD o Estocástico.

STC oscila entre 0 y 100:
- Los valores superiores a 75 suelen indicar condiciones de sobrecompra.
- Los valores por debajo de 25 indican condiciones de sobreventa
- Cruzar el nivel 50 puede indicar un cambio de tendencia

Señales indicadoras principales:
- Compre cuando STC cruce el nivel 25 de abajo hacia arriba (saliendo de la zona de sobreventa)
- Vender cuando STC cruce el nivel 75 de arriba a abajo (saliendo de la zona de sobrecompra)

## Parámetros

- **Length** - período principal para el cálculo del indicador.

## Cálculo

El cálculo de STC se realiza en varios pasos:

1. Calcular MACD:
   ```
   MACD = EMA(Close, Fast) - EMA(Close, Slow)
   Signal = EMA(MACD, Signal)
   ```
   donde Rápido, Lento y Signal suelen ser 23, 50 y 10 respectivamente.

2. Calcule Stochastic Oscillator basado en MACD:
   ```
   Stoch_K = 100 * ((MACD - Lowest(MACD, Length)) / (Highest(MACD, Length) - Lowest(MACD, Length)))
   Stoch_D = EMA(Stoch_K, 3)
   ```

3. Repita el cálculo estocástico para obtener STC:
   ```
   STC = 100 * ((Stoch_D - Lowest(Stoch_D, Length)) / (Highest(Stoch_D, Length) - Lowest(Stoch_D, Length)))
   ```

El resultado es un oscilador más suave que el estocástico clásico y reacciona más rápido a los cambios de tendencia que MACD.

![IndicatorSchaffTrendCycle](../../../../images/indicator_schaff_trend_cycle.png)

## Véase también

[MACD](macd.md)
[Stochastic](stochastic_oscillator.md)