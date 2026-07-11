# GMMA

**Media móvil múltiple de Guppy (GMMA)** es un indicador técnico desarrollado por Daryl Guppy que utiliza dos grupos de medias móviles exponenciales (EMA) para revelar la interacción entre los operadores a corto plazo y los inversores a largo plazo.

Para utilizar el indicador, debe utilizar la clase [GuppyMultipleMovingAverage](xref:StockSharp.Algo.Indicators.GuppyMultipleMovingAverage).

## Descripción

La media móvil múltiple de Guppy (GMMA) consta de dos grupos de medias móviles exponenciales (EMA):
1. **grupo de corto plazo** (normalmente 3, 5, 8, 10, 12 y 15 períodos): representa la actividad de los operadores a corto plazo
2. **grupo de largo plazo** (normalmente 30, 35, 40, 45, 50 y 60 períodos): representa la actividad de los inversores a largo plazo.

GMMA permite visualizar la interacción entre estos dos grupos de participantes del mercado y determina si el mercado se encuentra en un estado de tendencia o de consolidación. El indicador también ayuda a identificar momentos en los que los operadores a corto plazo comienzan a seguir la misma dirección que los inversores a largo plazo, lo que a menudo indica la formación o el fortalecimiento de una tendencia.

GMMA es particularmente útil para:
- Determinar la fuerza y dirección de la tendencia actual
- Identificar posibles cambios de tendencia
- Reconocer cuándo el mercado pasa de una consolidación a una tendencia
- Determinar los puntos de entrada óptimos a una tendencia existente

## Cálculo

El cálculo de GMMA implica calcular dos grupos de medias móviles exponenciales:

1. Grupo EMA de corta duración:
   ```
   EMA_3 = EMA(Price, 3)
   EMA_5 = EMA(Price, 5)
   EMA_8 = EMA(Price, 8)
   EMA_10 = EMA(Price, 10)
   EMA_12 = EMA(Price, 12)
   EMA_15 = EMA(Price, 15)
   ```

2. Grupo EMA a largo plazo:
   ```
   EMA_30 = EMA(Price, 30)
   EMA_35 = EMA(Price, 35)
   EMA_40 = EMA(Price, 40)
   EMA_45 = EMA(Price, 45)
   EMA_50 = EMA(Price, 50)
   EMA_60 = EMA(Price, 60)
   ```

donde:
- EMA - media móvil exponencial
- Price - precio (normalmente precio de cierre)

## Interpretación

La interpretación de GMMA implica analizar tanto los grupos individuales como su interacción:

1. **Group Positioning**:
   - Cuando el grupo de corto plazo está por encima del grupo de largo plazo, indica una tendencia al alza.
   - Cuando el grupo de corto plazo está por debajo del grupo de largo plazo, indica una tendencia a la baja.

2. **Distancia entre grupos**:
   - La gran distancia entre grupos indica una fuerte tendencia
   - Una pequeña distancia o un cruce de grupo indica una tendencia débil o una consolidación

3. **Compresión y expansión**:
   - La compresión (convergencia) de líneas dentro de un grupo indica incertidumbre y posible consolidación.
   - La expansión (divergencia) de las líneas dentro de un grupo indica un fortalecimiento de la tendencia

4. **Cruces**:
   - grupo de corto plazo cruza el grupo a largo plazo de abajo hacia arriba - fuerte señal alcista
   - grupo de corto plazo cruza el grupo de largo plazo de arriba a abajo - fuerte señal bajista

5. **Cambios de dirección**:
   - Cuando el grupo de largo plazo comienza a cambiar de dirección, indica un cambio significativo en el sentimiento de los inversores a largo plazo.
   - La reversión grupo de corto plazo sin cambios en el grupo a largo plazo a menudo indica una corrección temporal

6. **Optimal Entry Points**:
   - Después de una fuerte expansión, puede ocurrir compresión, lo que indica una corrección dentro de la tendencia.
   - El fin de dicha compresión (nueva expansión) puede ser un buen punto de entrada en la dirección de la tendencia principal.

7. **Advertencia de reversión temprana**:
   - Los promedios de corto plazo cambian de dirección primero, luego comienzan a aparecer cambios en los promedios de largo plazo.
   - El cruce entre grupos puede servir como confirmación del cambio de tendencia

![indicator_guppy_multiple_moving_average](../../../../images/indicator_guppy_multiple_moving_average.png)

## Véase también

[EMA](ema.md)
[MovingAverageRibbon](moving_average_ribbon.md)
[RainbowCharts](rainbow_charts.md)
[MACD](macd.md)
