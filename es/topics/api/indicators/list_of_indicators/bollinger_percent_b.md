# BBP

**Bollinger Percent B (BBP)** es un indicador desarrollado por John Bollinger como complemento del indicador Bollinger Bands. BBP muestra la ubicación del precio en relación con el Bollinger Bands superior e inferior.

Para utilizar el indicador, debe utilizar la clase [BollingerPercentB](xref:StockSharp.Algo.Indicators.BollingerPercentB).

## Descripción

El indicador Bollinger Percent B determina la posición del precio en relación con el Bollinger Bands superior e inferior como un valor porcentual de 0 a 1 (o de 0% a 100%). Esto permite una determinación más precisa de la posición del precio en el contexto de Bollinger Bands:

- Un valor de 1 (o 100%) significa que el precio está en la banda Bollinger superior.
- Un valor de 0 (o 0%) significa que el precio está en la banda Bollinger inferior.
- Un valor de 0,5 (o 50%) significa que el precio está en la banda Bollinger media (SMA).

BBP también puede tomar valores fuera del rango 0-1:
- Los valores superiores a 1 indican que el precio está por encima de la banda superior Bollinger.
- Los valores inferiores a 0 indican que el precio está por debajo de la banda Bollinger inferior.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - Período de cálculo SMA (valor predeterminado: 20)
- **StdDevMultiplier** - multiplicador de desviación estándar para calcular Bollinger Bands (valor predeterminado: 2)

## Cálculo

El cálculo del porcentaje B Bollinger se basa en la fórmula:

```
BBP = (Price - Lower Bollinger Band) / (Upper Bollinger Band - Lower Bollinger Band)
```

donde:
- Price - precio actual (normalmente precio de cierre)
- Banda inferior Bollinger = SMA - (StdDevMultiplier * Standard Deviation)
- Banda superior Bollinger = SMA + (StdDevMultiplier * Standard Deviation)
- SMA - media móvil simple durante el período Length
- Desviación estándar: desviación estándar del precio durante el período Length

## Uso

Bollinger El porcentaje B se puede utilizar de varias maneras:

1. **Identificación de condiciones Sobrecompra/Sobreventa**:
   - Los valores superiores a 1 indican un mercado sobrecomprado
   - Los valores inferiores a 0 indican un mercado sobrevendido.

2. **Señales de inversión**:
   - Cuando BBP vuelve al rango 0-1 después de salir de él
   - Divergencias entre BBP y precio

3. **Determinación de tendencias**:
   - Los valores de BBP consistentemente por encima de 0,5 indican una tendencia alcista
   - Los valores de BBP constantemente por debajo de 0,5 indican una tendencia a la baja

4. **Encontrar niveles de soporte y resistencia ocultos para H**:
   - Los niveles 0,8 y 0,2 se utilizan a menudo como niveles adicionales de soporte y resistencia.

![indicator_bollinger_percent_b](../../../../images/indicator_bollinger_percent_b.png)

## Véase también

[BollingerBands](bollinger_bands.md)
[StdDev](standard_deviation.md)
[RSI](rsi.md)