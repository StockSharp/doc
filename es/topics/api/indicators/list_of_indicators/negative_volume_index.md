# NVI

**Negative Volume Index (NVI)** es un indicador técnico desarrollado por Paul Dysart que se centra en los cambios de precios en los días en que el volumen de operaciones disminuye en comparación con el día anterior.

Para utilizar el indicador, debe utilizar la clase [NegativeVolumeIndex](xref:StockSharp.Algo.Indicators.NegativeVolumeIndex).

## Descripción

El Negative Volume Index (NVI) se basa en la idea de que el "dinero inteligente" está activo en los días de bajo volumen, mientras que la "multitud" (operadores no profesionales) está más activa en los días de alto volumen. NVI cambia solo en los días en que el volumen actual es menor que el volumen del día anterior.

El concepto del indicador sugiere que los movimientos de precios en un volumen reducido son más significativos y a menudo reflejan las acciones de inversores informados. NVI busca rastrear estos movimientos, ignorando los cambios de precios en los días con mayor volumen.

NVI se utiliza a menudo junto con el complementario Positive Volume Index (PVI), que, por el contrario, considera sólo los días en los que aumenta el volumen.

## Cálculo

El cálculo de Negative Volume Index implica los siguientes pasos:

1. Establezca el valor inicial NVI (normalmente 1000):
   ```
   NVI[initial] = 1000
   ```

2. Para cada período posterior:
   ```
   Si Volume[current] < Volume[previous], entonces:
       NVI[current] = NVI[previous] * (1 + (Price[current] - Price[previous]) / Price[previous])
   Otherwise:
       NVI[current] = NVI[previous]
   ```

donde:
- Price - precio (normalmente precio de cierre)
- Volume - volumen de operaciones

En otras palabras, NVI cambia sólo en los días en que el volumen de operaciones disminuye y permanece sin cambios en los días en que el volumen aumenta o no cambia.

## Interpretación

El Negative Volume Index se puede interpretar de la siguiente manera:

1. **Análisis de tendencias**:
   - El aumento de NVI indica que se está comprando "dinero inteligente", lo que puede predecir un futuro aumento de precios
   - La caída de NVI indica que se está vendiendo "dinero inteligente", lo que puede predecir una futura caída de precios.

2. **Cruces de media móvil**:
   - NVI a menudo se compara con su promedio móvil de 255 días (aproximadamente un año de negociación)
   - Cuando NVI está por encima de su SMA de 255 días, se considera una señal alcista.
   - Cuando NVI está por debajo de su SMA de 255 días, se considera una señal bajista.

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que NVI forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que NVI forma un máximo más bajo

4. **Combinando con PVI**:
   - Cuando tanto NVI como PVI están subiendo, es una fuerte señal alcista.
   - Cuando tanto NVI como PVI están cayendo, es una fuerte señal bajista.
   - Cuando NVI sube y PVI cae, puede indicar una compra de "dinero inteligente" mientras que la "multitud" vende (escenario potencialmente alcista).
   - Cuando NVI cae y PVI sube, puede indicar que se vende "dinero inteligente" mientras que la "multitud" compra (escenario potencialmente bajista).

5. **Long-Term Changes**:
   - NVI a menudo se considera un indicador a largo plazo
   - El cambio sostenido en la dirección NVI puede indicar un cambio significativo en el sentimiento del mercado

6. **Confirmando otros indicadores**:
   - NVI funciona mejor cuando se combina con otros indicadores técnicos y métodos de análisis
   - Las señales NVI se vuelven más confiables cuando son confirmadas por otros indicadores

7. **Setting Thresholds**:
   - Algunos operadores establecen niveles de umbral para NVI (por ejemplo, 5% por encima o por debajo de la media móvil)
   - Cruzar estos umbrales puede considerarse una señal más fuerte que los simples cruces.

![indicator_negative_volume_index](../../../../images/indicator_negative_volume_index.png)

## Véase también

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)