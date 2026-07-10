# DC

**Donchian Channels (DC)** es un indicador técnico desarrollado por el operador Richard Donchian, que consta de una banda superior e inferior (límites del canal) basada en los valores de precio máximo y mínimo durante un período específico.

Para utilizar el indicador, debe utilizar la clase [DonchianChannels](xref:StockSharp.Algo.Indicators.DonchianChannels).

## Descripción

Donchian Channels es un indicador de tendencia y volatilidad simple pero efectivo. El indicador consta de tres líneas:
- Línea superior: máximo más alto durante el período seleccionado
- Línea inferior: mínimo más bajo durante el período seleccionado
- Línea media: valor promedio entre las líneas superior e inferior

Este indicador fue utilizado por primera vez por Richard Donchian en su regla del canal de 4 semanas, según la cual se produce una señal de compra cuando el precio supera el máximo de 4 semanas y una señal de venta cuando el precio cae por debajo del mínimo de 4 semanas.

Donchian Channels son útiles para:
- Identificar la volatilidad del mercado
- Determinar los niveles de soporte y resistencia.
- Generando señales de ruptura
- Definición del rango de negociación actual

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor predeterminado: 20)

## Cálculo

El cálculo de Donchian Channels es bastante simple:

1. Línea del canal superior:
   ```
   Upper = Highest High durante el periodo Length
   ```

2. Línea de canal inferior:
   ```
   Lower = Lowest Low durante el periodo Length
   ```

3. Línea del canal medio:
   ```
   Middle = (Upper + Lower) / 2
   ```

## Interpretación

Donchian Channels se puede utilizar de varias maneras:

1. **Estrategias de ruptura**:
   - Romper por encima de la línea superior del canal puede verse como una señal de compra.
   - Una ruptura por debajo de la línea inferior del canal puede verse como una señal de venta.

2. **Determinación de tendencias**:
   - Si el precio está en la mitad superior del canal (por encima de la línea media), se puede inferir una tendencia alcista.
   - Si el precio está en la mitad inferior del canal (debajo de la línea media), se puede inferir una tendencia a la baja.

3. **Niveles de soporte y resistencia**:
   - La línea superior del canal puede servir como nivel de resistencia.
   - La línea del canal inferior puede servir como nivel de soporte.

4. **Volatility Measurement**:
   - El ancho del canal (diferencia entre las líneas superior e inferior) indica la volatilidad del mercado.
   - La expansión del canal indica una mayor volatilidad
   - La contracción del canal indica una menor volatilidad

5. **Estrategias contratendencias**:
   - Algunos traders utilizan señales opuestas, esperando que el precio vuelva a la línea media después de alcanzar los bordes del canal.

![indicator_donchian_channels](../../../../images/indicator_donchian_channels.png)

## Véase también

[BollingerBands](bollinger_bands.md)
[KeltnerChannels](keltner_channels.md)
[Highest](highest.md)
[Lowest](lowest.md)