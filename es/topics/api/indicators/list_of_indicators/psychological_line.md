# PSY

**Psychological Line (PSY)** es un indicador técnico que mide la proporción de períodos ascendentes (velas, barras) en relación con el número total de períodos durante un intervalo de tiempo específico.

Para utilizar el indicador, debe utilizar la clase [PsychologicalLine](xref:StockSharp.Algo.Indicators.PsychologicalLine).

## Descripción

El Psychological Line (PSY) es un indicador sencillo pero eficaz que refleja el sentimiento del mercado calculando el porcentaje de periodos de subida de precios respecto al número total de periodos considerados. El indicador se basa en el supuesto de que la psicología del mercado y el sentimiento de los inversores desempeñan un papel crucial en los movimientos de precios.

PSY es un oscilador con valores que van de 0 a 100, donde:
- Un valor de 100 significa que el precio subió en todos los períodos considerados.
- Un valor de 0 significa que el precio cayó en todos los períodos considerados.
- Un valor de 50 significa un número igual de períodos ascendentes y descendentes.

El indicador PSY ayuda a determinar si el mercado se encuentra en una condición de sobrecompra o sobreventa y puede predecir posibles cambios de tendencia.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor predeterminado: 12-14)

## Cálculo

El cálculo de Psychological Line es muy sencillo:

```
PSY = (número de periodos alcistas durante periodos Length / Length) * 100
```

donde:
- Un período creciente se define como un período en el que el precio de cierre es mayor que el precio de cierre del período anterior.
- Length - número de períodos considerados

## Interpretación

El Psychological Line se puede interpretar de la siguiente manera:

1. **Niveles de sobrecompra y sobreventa**:
   - Los valores superiores a 70-80 indican condiciones de sobrecompra en el mercado (demasiados períodos estaban subiendo)
   - Los valores por debajo de 20-30 indican condiciones de sobreventa del mercado (demasiados períodos estaban cayendo)
   - Valores extremos suele preceder a cambios de tendencia

2. **Centerline (50)**:
   - Cruzar el nivel 50 de abajo hacia arriba puede verse como una señal alcista.
   - Cruzar el nivel 50 de arriba a abajo puede verse como una señal bajista.
   - El movimiento sostenido por encima de 50 indica dominio alcista
   - El movimiento sostenido por debajo de 50 indica dominio bajista

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que PSY forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que PSY forma un máximo más bajo

4. **Rebotes de niveles extremos**:
   - La reversión desde la zona de sobrecompra puede indicar una posible reversión bajista
   - La reversión desde la zona de sobreventa puede indicar una posible reversión alcista

5. **Análisis de tendencias**:
   - En una fuerte tendencia alcista, PSY a menudo se mantiene por encima de 50, con rebotes periódicos desde la zona de sobrecompra.
   - En una fuerte tendencia a la baja, PSY a menudo permanece por debajo de 50, con rebotes periódicos desde la zona de sobreventa.

6. **Length Parameter Tuning**:
   - Períodos más cortos (por ejemplo, 5-8) hacen que PSY sea más sensible y adecuado para operaciones a corto plazo
   - Períodos más largos (por ejemplo, 20-25) hacen que PSY sea más fluido y adecuado para operaciones a largo plazo

7. **Combinando con otros indicadores**:
   - PSY se utiliza a menudo en combinación con otros indicadores para confirmar señales
   - Particularmente útil cuando se combina con indicadores de tendencia e indicadores de volumen.

![indicator_psychological_line](../../../../images/indicator_psychological_line.png)

## Véase también

[RSI](rsi.md)
[StochasticOscillator](stochastic_oscillator.md)
[UltimateOscillator](uo.md)
[MomentumOscillator](momentum.md)
