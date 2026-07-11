# PVI

**índice de volumen positivo (PVI)** es un indicador técnico desarrollado por Paul Dysart que se centra en los cambios de precios en los días en que el volumen de operaciones aumenta en comparación con el día anterior.

Para utilizar el indicador, debe utilizar la clase [PositiveVolumeIndex](xref:StockSharp.Algo.Indicators.PositiveVolumeIndex).

## Descripción

El índice de volumen positivo (PVI) se basa en la idea de que la "multitud" (operadores no profesionales) es más activa en los días de gran volumen, mientras que el "dinero inteligente" actúa en los días de bajo volumen. PVI cambia solo en los días en que el volumen actual es mayor que el volumen del día anterior.

El indicador sugiere que los movimientos de precios ante un mayor volumen son significativos y a menudo reflejan el sentimiento del mercado masivo. PVI rastrea estos movimientos, ignorando los cambios de precios en los días con menor volumen.

PVI se utiliza a menudo junto con el complementario índice de volumen negativo (NVI), que, por el contrario, considera sólo los días en los que el volumen disminuye.

## Cálculo

El cálculo de índice de volumen positivo implica los siguientes pasos:

1. Establezca el valor inicial PVI (normalmente 1000):
   ```
   PVI[initial] = 1000
   ```

2. Para cada período posterior:
   ```
   Si Volume[current] > Volume[previous], entonces:
       PVI[current] = PVI[previous] * (1 + (Price[current] - Price[previous]) / Price[previous])
   Otherwise:
       PVI[current] = PVI[previous]
   ```

donde:
- Price - precio (normalmente precio de cierre)
- Volume - volumen de operaciones

En otras palabras, PVI cambia sólo en los días en que el volumen de operaciones aumenta y permanece sin cambios en los días en que el volumen disminuye o no cambia.

## Interpretación

El índice de volumen positivo se puede interpretar de la siguiente manera:

1. **Análisis de tendencias**:
   - El aumento de PVI indica que la "multitud" está comprando, lo que puede predecir un futuro aumento de precios.
   - La caída de PVI indica que la "multitud" está vendiendo, lo que puede predecir una futura caída de precios.

2. **Cruces de media móvil**:
   - PVI a menudo se compara con su promedio móvil de 255 días (aproximadamente un año de negociación)
   - Cuando PVI está por encima de su SMA de 255 días, se considera una señal alcista.
   - Cuando PVI está por debajo de su SMA de 255 días, se considera una señal bajista.

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que PVI forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que PVI forma un máximo más bajo

4. **Combinando con NVI**:
   - Cuando tanto PVI como NVI están subiendo, es una fuerte señal alcista.
   - Cuando tanto PVI como NVI están cayendo, es una fuerte señal bajista.
   - Cuando PVI sube y NVI cae, puede indicar que la "multitud" compra mientras que el "dinero inteligente" vende (escenario potencialmente alcista).
   - Cuando PVI cae y NVI sube, puede indicar que la "multitud" vende mientras que el "dinero inteligente" compra (escenario potencialmente bajista).

5. **Cambios a largo plazo**:
   - PVI a menudo se considera un indicador a largo plazo
   - El cambio sostenido en la dirección PVI puede indicar un cambio significativo en el sentimiento del mercado

6. **Confirmando otros indicadores**:
   - PVI funciona mejor cuando se combina con otros indicadores técnicos y métodos de análisis
   - Las señales PVI se vuelven más confiables cuando son confirmadas por otros indicadores

7. **Ajuste de umbrales**:
   - Algunos operadores establecen niveles de umbral para PVI (por ejemplo, 5% por encima o por debajo de la media móvil)
   - Cruzar estos umbrales puede considerarse una señal más fuerte que los simples cruces.

![Gráfico del indicador PVI](../../../../images/indicator_positive_volume_index.png)

## Véase también

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)
[NegativeVolumeIndex](negative_volume_index.md)
