# CKS

**stop Chande Kroll (CKS)** es un indicador para determinar los niveles de stop-loss, desarrollado por Tushar Chande y Stanley Kroll, que se adapta a la volatilidad del mercado y ayuda a los operadores a establecer puntos de salida de posiciones.

Para utilizar el indicador, debe utilizar la clase [ChandeKrollStop](xref:StockSharp.Algo.Indicators.ChandeKrollStop).

## Descripción

El indicador stop Chande Kroll fue desarrollado como una herramienta dinámica para establecer niveles de límite de pérdidas que responde a los cambios en la volatilidad y la tendencia del mercado. Consta de dos líneas: una línea de stop superior (para posiciones cortas) y una línea de stop inferior (para posiciones largas).

La principal ventaja de CKS radica en su capacidad de adaptarse a las condiciones actuales del mercado. Durante períodos de alta volatilidad, las líneas stop se colocan más alejadas del precio, lo que ayuda a evitar el cierre prematuro de posiciones debido al ruido del mercado. Durante períodos de baja volatilidad, las líneas de parada se acercan al precio, lo que permite un seguimiento más estricto de la tendencia.

CKS es particularmente útil para:
- Determinación de niveles de stop-loss para posiciones largas y cortas
- Siguiendo la tendencia con control de riesgos adaptativo
- Identificar posibles puntos de inversión de tendencia
- Crear sistemas de negociación mecánicos con reglas de salida claras

## Parámetros

El indicador tiene los siguientes parámetros:
- **Period** - período principal para calcular los extremos (valor predeterminado: 10)
- **Multiplier** - multiplicador para ATR, que determina la distancia desde los extremos (valor predeterminado: 1,5)
- **StopPeriod** - período para calcular los niveles de parada (valor predeterminado: 20)

## Cálculo

El cálculo de stop Chande Kroll implica los siguientes pasos:

1. Determinación de los extremos alto y bajo sobre el Period:
   ```
   HighestHigh = valor High más alto durante Period
   LowestLow = valor Low más bajo durante Period
   ```

2. Calcular el rango verdadero promedio (ATR) durante Period:
   ```
   ATR = valor TR promedio durante Period
   ```

3. Calculando las bandas superior e inferior:
   ```
   Banda superior = HighestHigh - (Multiplicador * ATR)
   Banda inferior = LowestLow + (Multiplicador * ATR)
   ```

4. Determinación de líneas de parada final basadas en StopPeriod:
   ```
   tope superior = valor más alto de la banda superior durante StopPeriod
   tope inferior = valor más bajo de la banda inferior durante StopPeriod
   ```

## Interpretación

- **tope superior** se utiliza para posiciones cortas. Si el precio de cierre excede el tope superior, esto puede considerarse una señal para cerrar una posición corta o abrir una posición larga.

- **tope inferior** se utiliza para posiciones largas. Si el precio de cierre cae por debajo del tope inferior, esto puede considerarse una señal para cerrar una posición larga o abrir una posición corta.

- **Cruce del precio con las líneas de stop** puede indicar un posible cambio de tendencia o el comienzo de un nuevo impulso.

- **Cambios bruscos en las líneas de stop** pueden ocurrir con cambios significativos en la volatilidad del mercado.

- **Usar con otros indicadores**: CKS funciona mejor en combinación con otros indicadores de tendencia e impulso que ayudan a determinar la dirección de entrada al mercado.

![CKS](../../../../images/indicator_chande_kroll_stop.png)

## Véase también

[ATR](atr.md)
[ParabolicSAR](parabolic_sar.md)
[DonchianChannels](donchian_channels.md)
