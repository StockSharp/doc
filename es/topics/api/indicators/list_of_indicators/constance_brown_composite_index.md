# CBCI

**Constance Brown Composite Index (CBCI)** es un indicador desarrollado por Constance Brown que combina elementos de varios indicadores técnicos para crear una herramienta integral de análisis de mercado.

Para utilizar el indicador, debe utilizar la clase [ConstanceBrownCompositeIndex](xref:StockSharp.Algo.Indicators.ConstanceBrownCompositeIndex).

## Descripción

El Constance Brown Composite Index (CBCI) fue creado para fusionar las fortalezas de varios indicadores en una herramienta integral. Incorpora elementos del oscilador estocástico, RSI, y otros osciladores para proporcionar señales más precisas sobre posibles reversiones del mercado y movimientos de tendencias.

CBCI está diseñado para:
- Identificar posibles puntos de inversión de tendencia
- Determinar los niveles de sobrecompra y sobreventa
- Detectar divergencias ocultas
- Confirmando la fuerza de la tendencia actual

El indicador funciona bien en varios períodos de tiempo y tipos de mercado, incluidos los mercados de acciones, divisas y materias primas.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo principal para el índice (valor predeterminado: 14)
- **StochasticKPeriod** - período para calcular el oscilador estocástico %K (valor predeterminado: 5)
- **StochasticDPeriod** - período para calcular el oscilador estocástico %D (valor predeterminado: 3)

## Cálculo

El cálculo de CBCI implica los siguientes pasos:

1. Calcule RSI durante el período Length:
   ```
   RSI = 100 - (100 / (1 + RS))
   RS = Average Gain / Average Loss
   ```

2. Calcule el oscilador estocástico:
   ```
   %K = ((Close - Lowest Low) / (Highest High - Lowest Low)) * 100
   %D = SMA(%K, StochasticDPeriod)
   ```

3. Combine RSI y un oscilador estocástico:
   ```
   CBCI = (RSI + %K + %D) / 3
   ```

Este índice combinado puede luego suavizarse para reducir el ruido.

## Interpretación

- **Niveles de sobrecompra y sobreventa**: 
  - Los valores superiores a 80 pueden indicar condiciones de sobrecompra en el mercado
  - Los valores por debajo de 20 pueden indicar condiciones de sobreventa del mercado.

- **Cruces de línea central**:
  - Cruzar desde abajo hacia arriba la línea 50 puede verse como una señal alcista.
  - Cruzar desde arriba hacia abajo la línea 50 puede verse como una señal bajista.

- **Divergencias**:
  - Divergencias clásicas: cuando el precio y CBCI se mueven en direcciones opuestas
  - Divergencias ocultas: cuando el precio y CBCI crean diferentes tipos de máximos o mínimos

- **Movimiento de tendencia**:
  - Si CBCI se mantiene consistentemente por encima de 50, puede indicar fuerza de tendencia alcista
  - Si CBCI permanece constantemente por debajo de 50, puede indicar fuerza de tendencia bajista

![indicator_constance_brown_composite_index](../../../../images/indicator_constance_brown_composite_index.png)

## Véase también

[RSI](rsi.md)
[StochasticOscillator](stochastic_oscillator.md)
[StochasticK](stochastic_oscillator_k.md)
[CCI](cci.md)