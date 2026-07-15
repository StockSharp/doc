# TMF

**flujo de dinero de Twiggs (TMF)** es un indicador de volumen desarrollado por Colin Twiggs como una versión mejorada del indicador flujo de dinero de Chaikin. TMF es más sensible a los cambios en el sentimiento del mercado y tiene menos señales falsas.

Para utilizar el indicador, debe utilizar la clase [TwiggsMoneyFlow](xref:StockSharp.Algo.Indicators.TwiggsMoneyFlow).

## Descripción

flujo de dinero de Twiggs analiza la relación entre precio y volumen para determinar la dirección del flujo de dinero dentro o fuera del mercado. A diferencia de los indicadores de volumen tradicionales, TMF elimina el ruido normalizando valores entre -1 y +1.

Características clave de TMF:
- Los valores positivos indican entrada de dinero en el instrumento (sentimiento alcista)
- Los valores negativos indican una salida de dinero del instrumento (sentimiento bajista)
- Un valor de 0 muestra el equilibrio entre la oferta y la demanda.

El indicador es útil para:
- Confirmar la tendencia actual o identificar la debilidad de la tendencia.
- Detectar divergencias entre precio y flujo de dinero.
- Identificar posibles puntos de reversión del mercado

## Parámetros

- **Longitud**: período de cálculo para la media móvil exponencial, que normalmente utiliza un valor de 21.

## Cálculo

El cálculo de flujo de dinero de Twiggs se realiza en varios pasos:

1. Calcular el rango verdadero:
   ```
   TR = Max(High - Low, |High - cierre anterior|, |Low - cierre anterior|)
   ```

2. Determinar el volumen de flujo monetario de Twiggs (TMFV):
   ```
   TMFV = Volume * ((Close - Low - (High - Close)) / TR)
   ```
   Cuando (High - Low = 0), TMFV = 0

3. Calcule la media móvil exponencial de TMFV y el volumen:
   ```
   EMA_TMFV = EMA(TMFV, Length)
   EMA_Volume = EMA(Volume, Length)
   ```

4. Valor final TMF:
   ```
   TMF = EMA_TMFV / EMA_Volume
   ```

Los valores de TMF oscilan entre -1 (fuerte señal bajista) y +1 (fuerte señal alcista).

![Gráfico del indicador TMF](../../../../images/indicator_twiggs_money_flow.png)

## Véase también

[ADL](accumulation_distribution_line.md)
[Índice de flujo de dinero](money_flow_index.md)
[OBV](on_balance_volume.md)
