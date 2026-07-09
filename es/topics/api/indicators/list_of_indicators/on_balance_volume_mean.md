# OBVM

**On Balance Volume Mean (OBVM)** es un indicador técnico que representa un promedio móvil del indicador On Balance Volume (OBV), lo que permite señales de tendencia más claras basadas en el volumen.

Para utilizar el indicador, debe utilizar la clase [OnBalanceVolumeMean](xref:StockSharp.Algo.Indicators.OnBalanceVolumeMean).

## Descripción

On Balance Volume Mean (OBVM) es una modificación del indicador clásico On Balance Volume (OBV) que aplica una media móvil a los valores OBV para suavizar las fluctuaciones e identificar tendencias más claras. El indicador mantiene el concepto central de OBV: acumulación de volumen basada en el cambio de dirección del precio, pero agrega una capa adicional de filtrado.

OBVM ayuda a eliminar el ruido presente en el OBV original y hace que las tendencias de flujo volumétrico a largo plazo sean más notorias. Esto es particularmente útil en mercados volátiles o cuando se analizan instrumentos con volúmenes de negociación irregulares.

La principal ventaja de OBVM es su capacidad para generar señales de trading más claras y menos propensas a falsas señales en comparación con el clásico OBV. El indicador también se puede utilizar para identificar cruces entre OBV y su valor medio, brindando oportunidades de trading adicionales.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período para el cálculo de la media móvil (valor predeterminado: 20)

## Cálculo

El cálculo de On Balance Volume Mean implica los siguientes pasos:

1. Calcule la base del indicador On Balance Volume (OBV):
   ```
   If Close[current] > Close[previous]:
       OBV[current] = OBV[previous] + Volume[current]
   If Close[current] < Close[previous]:
       OBV[current] = OBV[previous] - Volume[current]
   If Close[current] = Close[previous]:
       OBV[current] = OBV[previous]
   ```

2. Aplicar media móvil a los valores OBV:
   ```
   OBVM = SMA(OBV, Length)
   ```

donde:
- Close - precio de cierre
- Volume - volumen de operaciones
- OBV - En equilibrio Volume
- SMA - media móvil simple
- Length - período de media móvil

Nota: Se pueden utilizar otros tipos de medias móviles como EMA (media móvil exponencial), WMA (media móvil ponderada), etc., en lugar de SMA.

## Interpretación

On Balance Volume Mean se puede interpretar de la siguiente manera:

1. **Análisis de tendencias**:
   - El aumento de OBVM indica una tendencia alcista con un fuerte soporte de volumen
   - La caída de OBVM indica una tendencia bajista con un fuerte soporte de volumen
   - Flat OBVM indica que no hay una tendencia pronunciada

2. **Cruces OBV y OBVM**:
   - Cuando OBV cruza OBVM de abajo hacia arriba, puede verse como una señal alcista.
   - Cuando OBV cruza OBVM de arriba a abajo, puede verse como una señal bajista.
   - Estos cruces a menudo indican el inicio de nuevas tendencias o movimientos de precios significativos.

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que OBVM forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que OBVM forma un máximo más bajo
   - Divergencias a menudo precede a cambios de tendencia significativos

4. **Confirmación de tendencia de precios**:
   - Si OBVM se mueve en la misma dirección que el precio, esto confirma la tendencia actual del precio.
   - Si OBVM y el precio se mueven en direcciones opuestas, esto puede indicar un posible cambio de tendencia.

5. **Niveles de soporte y resistencia**:
   - El gráfico OBVM puede formar sus propios niveles de soporte y resistencia.
   - La ruptura de estos niveles puede preceder a rupturas similares en el gráfico de precios.

6. **Comparación con otros indicadores Volume**:
   - OBVM se puede comparar con otros indicadores de volumen para confirmar señales
   - La consistencia de las señales de múltiples indicadores de volumen aumenta su confiabilidad.

7. **Length Parameter Selection**:
   - Períodos más cortos (por ejemplo, 10-15) hacen que OBVM sea más sensible a cambios a corto plazo
   - Períodos más largos (por ejemplo, 30-50) identifican mejor las tendencias a largo plazo
   - El período óptimo depende del horizonte temporal de negociación y de las características específicas del instrumento.

![indicator_on_balance_volume_mean](../../../../images/indicator_on_balance_volume_mean.png)

## Véase también

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)