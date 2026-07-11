# CRSI

**Connors RSI (CRSI)** es un indicador técnico integral desarrollado por Larry Connors que combina tres componentes para medir las condiciones de sobrecompra y sobreventa del mercado.

Para utilizar el indicador, debe utilizar la clase [ConnorsRSI](xref:StockSharp.Algo.Indicators.ConnorsRSI).

## Descripción

Connors RSI es una versión avanzada del índice de fuerza relativa (RSI) tradicional, que agrega dos componentes adicionales para proporcionar señales de sobrecompra y sobreventa más precisas.

A diferencia del RSI estándar, que solo considera el cambio de precio, el Connors RSI también tiene en cuenta la racha (serie de movimientos de precios consecutivos en una dirección) y la tasa de cambio (ROC), lo que lo hace más sensible a los cambios a corto plazo y más confiable para identificar condiciones extremas del mercado.

CRSI es particularmente útil para:
- Identificar oportunidades de entrada y salida a corto plazo.
- Determinación de niveles extremos de sobrecompra y sobreventa
- Creación de sistemas de trading basados en la reversión a la media
- Filtrar señales de otros indicadores.

## Parámetros

El indicador tiene los siguientes parámetros:
- **RSIPeriod** - período para calcular el componente RSI (valor predeterminado: 3)
- **StreakRSIPeriod** - período para calcular el componente de racha RSI (valor predeterminado: 2)
- **ROCRSIPeriod** - período para calcular la tasa de cambio del componente RSI (valor predeterminado: 100)

## Cálculo

El cálculo de Connors RSI implica tres componentes que luego se promedian para obtener el valor final:

1. **Precio RSI Componente** - RSI estándar calculado en un período corto (normalmente 3 días):
   ```
   RSI = 100 - (100 / (1 + RS))
   donde RS = Cambio positivo medio / Cambio negativo medio
   ```

2. **Componente RSI de racha**:
   - Primero, calcule la racha (número de días consecutivos de subida o bajada de precios)
   - Luego aplique RSI a esta racha usando el StreakRSIPeriod

3. **Componente RSI de tasa de cambio (ROC RSI)**:
   - Calcule el rango percentil del ROC actual sobre el ROCRSIPeriod
   - Escale el rango percentil de 0 a 100

4. **Valor final de Connors RSI**:
   ```
   CRSI = (RSI + StreakRSI + ROCRSI) / 3
   ```

## Interpretación

Connors RSI oscila entre 0 y 100, similar al estándar RSI:

- **Valores extremadamente altos (por encima de 90)** indican fuertes condiciones de sobrecompra. Esto puede ser una señal para vender o tomar una posición corta.

- **Valores extremadamente bajos (por debajo de 10)** indican fuertes condiciones de sobreventa. Esto puede ser una señal para comprar o cerrar una posición corta.

- **Niveles estándar**:
  - Por encima de 70-80: sobrecompra
  - Por debajo de 20-30: sobreventa
  - 40-60: zona neutral

- **Divergencias**:
  - Divergencia alcista: el precio forma un nuevo mínimo, mientras que CRSI forma un mínimo más alto
  - Divergencia bajista: el precio forma un nuevo máximo, mientras que CRSI forma un máximo más bajo

Connors RSI funciona mejor en gráficos con períodos de tiempo diarios a semanales y en estrategias de trading orientadas a la reversión media.

![indicator_connors_rsi](../../../../images/indicator_connors_rsi.png)

## Véase también

[RSI](rsi.md)
[RMI](relative_momentum_index.md)
[LRSI](laguerre_rsi.md)
