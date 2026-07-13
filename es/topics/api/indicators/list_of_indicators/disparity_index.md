# DPI

**índice de disparidad (DPI)** es un indicador técnico que mide la desviación relativa del precio actual de un promedio móvil durante un período específico, expresada como porcentaje.

Para utilizar el indicador, debe utilizar la clase [DisparityIndex](xref:StockSharp.Algo.Indicators.DisparityIndex).

## Descripción

El índice de disparidad (DPI) está diseñado para medir el grado de desviación del precio con respecto a su media móvil. Este indicador ayuda a determinar hasta qué punto se "estira" el precio en relación con su valor promedio y puede usarse para identificar posibles condiciones de sobrecompra o sobreventa.

DPI se basa en el supuesto de que el precio tiende a volver a su valor medio después de una desviación significativa. Cuanto mayor sea la desviación, mayor será la probabilidad de que el precio se mueva posteriormente en la dirección opuesta, acercándolo al promedio.

El índice de disparidad es útil para:
- Identificar desviaciones extremas del precio respecto de su media
- Detectar posibles puntos de reversión
- Midiendo la fuerza de la tendencia actual
- Creación de estrategias de negociación basadas en la reversión a la media.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período para calcular la media móvil (valor predeterminado: 14)

## Cálculo

La fórmula de cálculo de índice de disparidad es bastante sencilla:

```
DPI = ((Price / MA) - 1) * 100
```

donde:
- Price - precio actual (normalmente precio de cierre)
- MA: media móvil del precio durante el período Length
- El resultado se expresa como porcentaje.

Los valores positivos de DPI indican que el precio está por encima de su media móvil, mientras que los valores negativos indican que el precio está por debajo de su media móvil.

## Interpretación

El índice de disparidad se puede interpretar de la siguiente manera:

1. **Niveles extremos**:
   - Los valores positivos altos (por ejemplo, por encima de +10%) pueden indicar condiciones de sobrecompra en el mercado
   - Los valores negativos extremos (por ejemplo, por debajo de -10%) pueden indicar condiciones de sobreventa del mercado

2. **Cruces de línea cero**:
   - Cruzar de abajo hacia arriba (de valores negativos a positivos) indica que el precio ha cruzado su promedio móvil de abajo hacia arriba, lo que puede verse como una señal alcista.
   - Cruzar de arriba a abajo (de valores positivos a negativos) indica que el precio ha cruzado su media móvil de arriba a abajo, lo que puede verse como una señal bajista.

3. **Divergencias**:
   - Divergencia alcista: el precio alcanza un nuevo mínimo, pero DPI forma un mínimo más alto
   - Divergencia bajista: el precio alcanza un nuevo máximo, pero DPI forma un máximo más bajo

4. **Análisis de tendencias**:
   - Los valores DPI consistentemente positivos indican una fuerte tendencia alcista
   - Los valores DPI consistentemente negativos indican una fuerte tendencia a la baja
   - Las oscilaciones alrededor de cero pueden indicar una tendencia lateral o una consolidación

5. **Estrategias de reversión a la media**:
   - Se pueden utilizar valores extremos de DPI para abrir posiciones contra el movimiento actual del precio, esperando un retorno a la media.

![Gráfico del indicador DPI](../../../../images/indicator_disparity_index.png)

## Véase también

[SMA](sma.md)
[RSI](rsi.md)
[Oscilador estocástico](stochastic_oscillator.md)
[BollingerBands](bollinger_bands.md)