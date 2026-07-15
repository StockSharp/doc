# KVO

**Oscilador de volumen de Klinger (KVO)** es un indicador técnico desarrollado por Stephen Klinger que utiliza el volumen y el precio para identificar tendencias a largo plazo y reversiones a corto plazo en el mercado.

Para utilizar el indicador, debe utilizar la clase [KlingerVolumeOscillator](xref:StockSharp.Algo.Indicators.KlingerVolumeOscillator).

## Descripción

El Oscilador de volumen de Klinger (KVO) fue creado por Stephen Klinger para medir la divergencia entre volumen y precio. El indicador se basa en el concepto de que el movimiento del precio se confirma por el volumen. KVO busca determinar no sólo la dirección de la tendencia sino también su fuerza y ​​posibles puntos de reversión.

KVO combina información de precios con volumen utilizando un indicador de fuerza de volumen que considera tanto la dirección como la magnitud del movimiento de precios, así como el volumen de operaciones. Luego aplica medias móviles exponenciales (EMA) con dos períodos diferentes a este flujo de dinero y calcula la diferencia entre ellos.

El indicador es un oscilador que fluctúa por encima y por debajo de la línea cero. Los valores positivos de KVO indican que los compradores controlan el mercado, mientras que los valores negativos indican que los vendedores tienen una ventaja.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Período corto** - período para calcular EMA corto (valor predeterminado: 34)
- **Período largo** - período para calcular EMA largo (valor predeterminado: 55)

## Cálculo

El cálculo de Oscilador de volumen de Klinger implica varios pasos:

1. Determinar la tendencia para cada período:
   ```
   Trend = +1, si (High + Low + Close) > (High[previous] + Low[previous] + Close[previous])
   Trend = -1, en caso contrario
   ```

2. Calcular el indicador de fuerza de volumen:
   ```
   fuerza de volumen = Volume * Trend * abs(2 * ((Close - Low) - (High - Close)) / (High - Low))
   ```
   Si (High - Low) es cero, fuerza de volumen se establece en volumen multiplicado por tendencia.

3. Calcule EMA para dos períodos:
   ```
   EMA corta = EMA(Fuerza de volumen, ShortPeriod)
   EMA larga = EMA(Fuerza de volumen, LongPeriod)
   ```

4. Cálculo final de KVO:
   ```
   KVO = EMA corta - EMA larga
   ```

5. Calcular línea de señal (opcional):
   ```
   Línea de señal = EMA(KVO, 13)
   ```

donde:
- High, Low, Close: precios más alto, más bajo y de cierre
- Volume - volumen de operaciones
- EMA - media móvil exponencial
- ShortPeriod - período corto EMA
- LongPeriod - período largo EMA

## Interpretación

El Oscilador de volumen de Klinger se puede interpretar de la siguiente manera:

1. **Cruces de línea cero**:
   - KVO cruzar la línea cero de abajo hacia arriba puede verse como una señal alcista
   - KVO cruzar la línea cero de arriba a abajo puede verse como una señal bajista

2. **Cruces de línea de señal**:
   - KVO cruzar la línea de señal de abajo hacia arriba puede verse como una señal de entrada alcista
   - KVO cruzar la línea de señal de arriba a abajo puede verse como una señal de entrada bajista

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que KVO forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que KVO forma un máximo más bajo

4. **Confirmación de tendencia**:
   - Los valores positivos de KVO confirman una tendencia alcista
   - Los valores negativos de KVO confirman una tendencia a la baja

5. **Fuerza de la tendencia**:
   - El aumento del valor KVO (tanto positivo como negativo) indica un fortalecimiento de la tendencia actual
   - La disminución del valor KVO indica un debilitamiento de la tendencia actual

6. **Reversiones potenciales**:
   - Los valores extremos de KVO pueden indicar condiciones de sobrecompra o sobreventa del mercado y una posible reversión.
   - La desaceleración en la subida o bajada de KVO puede preceder a un cambio de tendencia

7. **Volumen y precio**:
   - KVO permite evaluar la coherencia del movimiento de precios y volúmenes
   - Un volumen fuerte en la dirección de la tendencia conduce a valores KVO más extremos

![Gráfico del indicador KVO](../../../../images/indicator_klinger_volume_oscillator.png)

## Véase también

[OBV](on_balance_volume.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ADL](accumulation_distribution_line.md)
[ForceIndex](force_index.md)
