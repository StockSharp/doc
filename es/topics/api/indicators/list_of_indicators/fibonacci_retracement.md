# FR

**retroceso de Fibonacci (FR)** es un indicador técnico basado en números de Fibonacci que ayuda a identificar posibles niveles de soporte y resistencia basados en movimientos de precios anteriores.

Para utilizar el indicador, debe utilizar la clase [FibonacciRetracement](xref:StockSharp.Algo.Indicators.FibonacciRetracement).

## Descripción

retroceso de Fibonacci es una herramienta técnica popular que utiliza líneas horizontales para indicar áreas de posible soporte o resistencia en un gráfico de precios. Estos niveles se basan en los números de Fibonacci y sus correspondientes porcentajes.

El indicador se basa en la secuencia matemática de Fibonacci, donde cada número es la suma de los dos anteriores (1, 1, 2, 3, 5, 8, 13, 21, 34...). De esta secuencia se derivan los ratios clave utilizados en el análisis técnico: 23,6%, 38,2%, 50%, 61,8% y 78,6%.

retroceso de Fibonacci se aplica a un movimiento de precio significativo (tendencia) y muestra los niveles donde puede ocurrir una corrección (retroceso) antes de continuar en la dirección de la tendencia principal.

El indicador es particularmente útil para:
- Identificar posibles niveles de soporte en una tendencia alcista
- Identificar niveles potenciales de resistencia en una tendencia a la baja.
- Establecer objetivos de entrada después de una corrección
- Determinación de niveles para la colocación de stop-loss

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período para determinar un movimiento de precio significativo (el valor predeterminado depende del período)

## Cálculo

Calcular los niveles retroceso de Fibonacci implica los siguientes pasos:

1. Determinación de un movimiento de precio significativo (tendencia):
   - En tendencia ascendente: de menor a mayor
   - En tendencia a la baja: de mayor a menor

2. Calcular los niveles de corrección en función del rango de este movimiento:
   ```
   Range = |High - Low|

   Nivel 0% = High (para tendencia alcista) o Low (para tendencia bajista)
   Nivel 23.6% = High - (Range * 0.236) o Low + (Range * 0.236)
   Nivel 38.2% = High - (Range * 0.382) o Low + (Range * 0.382)
   Nivel 50.0% = High - (Range * 0.5) o Low + (Range * 0.5)
   Nivel 61.8% = High - (Range * 0.618) o Low + (Range * 0.618)
   Nivel 78.6% = High - (Range * 0.786) o Low + (Range * 0.786)
   Nivel 100% = Low (para tendencia alcista) o High (para tendencia bajista)
   ```

## Interpretación

Los niveles retroceso de Fibonacci se interpretan de la siguiente manera:

1. **Principales niveles de corrección**:
   - 23,6%: nivel débil, a menudo superado durante una tendencia fuerte
   - 38,2% - nivel moderado, corresponde a una corrección típica
   - 50,0%: nivel psicológicamente significativo (aunque no es un número de Fibonacci)
   - 61,8% - "proporción áurea", el nivel de Fibonacci más importante
   - 78,6%: corrección profunda que puede indicar un posible cambio de tendencia

2. **En una tendencia alcista**:
   - Los niveles de Fibonacci sirven como posibles niveles de soporte.
   - Rebotar en un nivel de Fibonacci puede ser una señal para entrar en una posición larga
   - Romper varios niveles de Fibonacci a la baja puede indicar un debilitamiento de la tendencia

3. **En una tendencia a la baja**:
   - Los niveles de Fibonacci sirven como niveles de resistencia potenciales.
   - Rebotar en un nivel de Fibonacci puede ser una señal para entrar en una posición corta
   - Romper varios niveles de Fibonacci hacia arriba puede indicar un debilitamiento de la tendencia

4. **Coincidencia con otros niveles**:
   - Los niveles de Fibonacci son más significativos cuando coinciden con otros niveles importantes (máximos/mínimos anterior, medias móviles, etc.)

5. **Trazado en distintos marcos temporales**:
   - Los niveles de Fibonacci trazados en diferentes períodos de tiempo pueden crear zonas de agrupamiento donde aumenta la probabilidad de reversión.

![Gráfico del indicador FR](../../../../images/indicator_fibonacci_retracement.png)

## Véase también

[PivotPoints](pivot_points.md)
