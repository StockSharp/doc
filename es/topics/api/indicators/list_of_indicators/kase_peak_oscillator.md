# KPO

**Kase Peak Oscillator (KPO)** es un indicador técnico desarrollado por Celia Kase que combina impulso y volatilidad para identificar posibles picos y valles del mercado.

Para utilizar el indicador, debe utilizar la clase [KasePeakOscillator](xref:StockSharp.Algo.Indicators.KasePeakOscillator).

## Descripción

El Kase Peak Oscillator (KPO) es una herramienta para determinar las condiciones del mercado de sobrecompra y sobreventa e identificar posibles puntos de reversión. Fue desarrollado por la operador e ingeniera Celia Kase como parte de su metodología comercial.

KPO se basa en el concepto de que los picos y valles del mercado se forman cuando el impulso del movimiento de precios comienza a agotarse. El oscilador utiliza una combinación de indicadores de impulso y volatilidad para identificar estos puntos de inflexión clave.

El indicador es un oscilador adimensional que fluctúa alrededor de la línea cero. Los valores positivos indican un impulso ascendente, mientras que los valores negativos indican un impulso a la baja. Los valores extremos del oscilador a menudo coinciden con los máximos y mínimos del gráfico de precios.

## Parámetros

El indicador tiene los siguientes parámetros:
- **ShortPeriod** - período corto para el cálculo del impulso (valor predeterminado: 10)
- **LongPeriod** - largo período para el cálculo del impulso (valor predeterminado: 30)

## Cálculo

El cálculo de Kase Peak Oscillator implica varios pasos:

1. Calcule el impulso a corto plazo en función del período corto:
   ```
   Short Momentum = EMA(Price, ShortPeriod) - EMA(Price, ShortPeriod)[previous]
   ```

2. Calcule el impulso a largo plazo en función del período largo:
   ```
   Long Momentum = EMA(Price, LongPeriod) - EMA(Price, LongPeriod)[previous]
   ```

3. Calcule la volatilidad actual:
   ```
   Volatility = ATR(ShortPeriod)
   ```

4. Normalizar el impulso en relación con la volatilidad:
   ```
   Normalized Short Momentum = Short Momentum / Volatility
   Normalized Long Momentum = Long Momentum / Volatility
   ```

5. Cálculo final de KPO:
   ```
   KPO = Normalized Short Momentum - Normalized Long Momentum
   ```

donde:
- Price - precio de cierre habitual
- EMA - media móvil exponencial
- ATR - rango verdadero promedio
- ShortPeriod - período de cálculo corto
- LongPeriod - período de cálculo largo

## Interpretación

El Kase Peak Oscillator se puede interpretar de la siguiente manera:

1. **Cruces de línea cero**:
   - Cruzar de abajo hacia arriba puede verse como una señal alcista
   - Cruzar de arriba a abajo puede verse como una señal bajista

2. **Valores extremos**:
   - Los valores positivos altos pueden indicar condiciones de sobrecompra en el mercado y una posible reversión a la baja.
   - Los valores negativos extremos pueden indicar condiciones de sobreventa del mercado y una posible reversión al alza.

3. **Divergencias**:
   - La divergencia alcista (el precio forma un nuevo mínimo, mientras que KPO forma un mínimo más alto) puede indicar una próxima reversión alcista
   - Divergencia bajista (el precio forma un nuevo máximo, mientras que KPO forma un máximo más bajo) puede indicar una próxima reversión a la baja

4. **Cruces de componentes**:
   - Cuando el impulso a corto plazo cruza el impulso a largo plazo de abajo hacia arriba, puede verse como una señal alcista.
   - Cuando el impulso a corto plazo cruza el impulso a largo plazo de arriba a abajo, puede verse como una señal bajista.

5. **Aceleración y desaceleración**:
   - La pendiente KPO aumentada indica aceleración del impulso
   - La pendiente KPO disminuida indica una desaceleración del impulso, que puede preceder a una reversión

6. **Combinando con otros indicadores**:
   - KPO se utiliza a menudo con otros indicadores técnicos para confirmar señales
   - Particularmente efectivo cuando se combina con indicadores de tendencia y niveles de soporte/resistencia

![indicator_kase_peak_oscillator](../../../../images/indicator_kase_peak_oscillator.png)

## Véase también

[MomentumOscillator](momentum.md)
[MACD](macd.md)
[PrettyGoodOscillator](pretty_good_oscillator.md)
[ATR](atr.md)
