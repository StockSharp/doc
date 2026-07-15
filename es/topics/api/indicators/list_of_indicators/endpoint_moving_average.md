# EPMA

**Media móvil de punto final (EPMA)** es un indicador técnico que es una modificación de la media móvil estándar, diseñado para reducir el retraso en la identificación de tendencias.

Para utilizar el indicador, debe utilizar la clase [EndpointMovingAverage](xref:StockSharp.Algo.Indicators.EndpointMovingAverage).

## Descripción

La media móvil de punto final (EPMA) es una forma especial de media móvil que se centra en los puntos de datos del punto final. A diferencia de los promedios móviles estándar que ponderan uniformemente todos los puntos en un período determinado, EPMA otorga más peso a los puntos finales, lo que permite una respuesta más rápida a los cambios de tendencia.

El objetivo principal de EPMA es reducir el retraso inherente a las medias móviles tradicionales manteniendo al mismo tiempo la capacidad de filtrar el ruido del mercado. Debido a su metodología de cálculo, EPMA a menudo reacciona más rápidamente a los cambios de dirección del precio, lo que lo convierte en una herramienta valiosa para los operadores que buscan identificar cambios de tendencia antes.

EPMA es particularmente útil para:
- Identificación temprana de cambios de tendencia.
- Reducir el retraso de la señal
- Crear sistemas de negociación más sensibles
- Confirmar señales de otros indicadores con menos demora

## Parámetros

El indicador tiene los siguientes parámetros:
- **Longitud** - período de cálculo (valor predeterminado: 14)

## Cálculo

El cálculo de la media móvil de punto final se basa en el método de regresión lineal, centrándose en los puntos finales del período considerado:

1. Determinación de la tendencia lineal entre el punto inicial y final del período:
   ```
   valor inicial = Price[current - Length + 1]
   valor final = Price[current]
   ```

2. Calculando la pendiente de la línea de tendencia:
   ```
   Slope = (valor final - valor inicial) / (Length - 1)
   ```

3. Calcular EPMA como una proyección de la línea de tendencia hasta el punto actual:
   ```
   EPMA = valor inicial + Slope * (Length - 1)
   ```

De hecho, EPMA es igual al último valor del precio (valor final) en el período considerado, pero conceptualmente es una proyección de la tendencia lineal definida por los puntos finales.

## Interpretación

La media móvil de punto final se interpreta de manera similar a otras medias móviles, pero teniendo en cuenta su mayor sensibilidad:

1. **EPMA Dirección**:
   - Hacia arriba EPMA indica una tendencia alcista
   - Downward EPMA indica una tendencia a la baja

2. **Cruces con precio**:
   - Cuando el precio cruza EPMA de abajo hacia arriba, puede verse como una señal alcista.
   - Cuando el precio cruza EPMA de arriba a abajo, puede verse como una señal bajista.

3. **Múltiples cruces EPMA**:
   - El cruce de un EPMA corto con un EPMA largo de abajo hacia arriba puede indicar el inicio de una tendencia alcista
   - El cruce de un EPMA corto con un EPMA largo de arriba a abajo puede indicar el inicio de una tendencia a la baja.

4. **Divergencia con otras medias móviles**:
   - Debido a su mayor sensibilidad, EPMA puede reaccionar a los cambios de tendencia antes que los promedios móviles tradicionales.
   - Divergencia entre EPMA y otros tipos de medias móviles pueden servir como una advertencia temprana de un posible cambio de tendencia

5. **Filtrado de señal**:
   - Debido a su mayor sensibilidad, EPMA puede generar más señales falsas durante los períodos de consolidación lateral
   - Se recomienda utilizar filtros adicionales o confirmaciones de otros indicadores para mejorar la confiabilidad de la señal.

![Gráfico del indicador EPMA](../../../../images/indicator_endpoint_moving_average.png)

## Véase también

[SMA](sma.md)
[EMA](ema.md)
[ZLEMA](zero_lag_exponential_moving_average.md)
[DEMA](dema.md)
