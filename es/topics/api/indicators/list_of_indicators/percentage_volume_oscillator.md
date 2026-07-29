# PVO

**Oscilador porcentual de volumen (PVO)** es un indicador técnico similar a MACD, pero se aplica al volumen de operaciones en lugar del precio, expresando la diferencia entre promedios móviles exponenciales rápidos y lentos de volumen como porcentaje.

Para utilizar el indicador, debe utilizar la clase [PercentageVolumeOscillator](xref:StockSharp.Algo.Indicators.PercentageVolumeOscillator).

## Descripción

El oscilador porcentual de volumen (PVO) es una modificación del indicador MACD (convergencia/divergencia de medias móviles), que se aplica al volumen de operaciones en lugar del precio. Similar a PPO (oscilador porcentual de precio), PVO expresa la diferencia entre promedios móviles exponenciales rápidos y lentos como un porcentaje, en lugar de en unidades absolutas. Esto hace que PVO sea particularmente útil al comparar diferentes instrumentos con diferentes niveles de volumen o al analizar un solo instrumento durante un período prolongado.

PVO consta de tres componentes:
1. **Línea PVO** - diferencia porcentual entre las EMA de volumen rápida y lenta
2. **Línea de señal** - EMA de la línea PVO
3. **Histograma** - diferencia entre la línea PVO y la línea de señal

El indicador PVO ayuda a identificar anomalías de volumen que pueden preceder a movimientos significativos de precios. También es útil para confirmar tendencias de precios e identificar posibles puntos de reversión.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Período corto** - período para calcular el volumen corto EMA (valor predeterminado: 12)
- **Período largo** - período para calcular el volumen largo EMA (valor predeterminado: 26)

## Cálculo

El cálculo del oscilador porcentual de volumen implica los siguientes pasos:

1. Calcule las medias móviles exponenciales cortas y largas de volumen:
   ```
   EMA corta = EMA(Volumen, ShortPeriod)
   EMA larga = EMA(Volumen, LongPeriod)
   ```

2. Calcule la línea PVO como diferencia porcentual entre las EMA corta y larga:
   ```
   Línea PVO = ((EMA corta - EMA larga) / EMA larga) * 100
   ```

3. Calcule la línea de señal (normalmente EMA de 9 períodos de la línea PVO):
   ```
   Línea de señal = EMA(Línea PVO, 9)
   ```

4. Calcule el histograma:
   ```
   Histograma = Línea PVO - Línea de señal
   ```

donde:
- Volume - volumen de operaciones
- EMA - media móvil exponencial
- ShortPeriod - período corto EMA
- LongPeriod - período largo EMA

## Interpretación

El oscilador porcentual de volumen se puede interpretar de la siguiente manera:

1. **Cruces de línea cero**:
   - La línea PVO que cruza la línea cero de abajo hacia arriba indica una aceleración del volumen por encima del promedio, lo que puede presagiar un movimiento alcista.
   - La línea PVO que cruza la línea cero de arriba a abajo indica una desaceleración del volumen por debajo del promedio, lo que puede presagiar un movimiento bajista.

2. **Cruces de línea de señal**:
   - La línea PVO que cruza la línea de señal de abajo hacia arriba puede verse como una señal alcista
   - La línea PVO que cruza la línea de señal de arriba a abajo puede verse como una señal bajista

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que PVO forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que PVO forma un máximo más bajo

4. **Valores extremos**:
   - Los valores muy altos de PVO pueden indicar un volumen excesivo, que a menudo ocurre durante los picos del mercado o durante el pánico.
   - Los valores muy bajos de PVO pueden indicar un volumen insuficiente, lo que a menudo ocurre durante las pausas del mercado.

5. **Análisis del histograma**:
   - El aumento del histograma positivo indica un fortalecimiento del impulso del volumen alcista
   - El aumento del histograma negativo indica un fortalecimiento del impulso del volumen bajista
   - La contracción del histograma indica un debilitamiento del impulso del volumen actual

6. **Confirmación de tendencia de precios**:
   - Un PVO ascendente confirma una tendencia alcista de precios
   - La caída del PVO confirma una tendencia a la baja del precio
   - Divergencia entre la dirección PVO y el precio puede indicar una posible reversión

7. **Picos de volumen**:
   - Los saltos bruscos de PVO indican cambios de volumen significativos, que a menudo acompañan a eventos importantes del mercado
   - Estos picos pueden preceder o acompañar a las rupturas de niveles de precios clave.

![Gráfico del indicador PVO](../../../../images/indicator_percentage_volume_oscillator.png)

## Véase también

[PPO](percentage_price_oscillator.md)
[OBV](on_balance_volume.md)
[MACD](macd.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
