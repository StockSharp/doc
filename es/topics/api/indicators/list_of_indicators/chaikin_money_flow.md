# CMF

**flujo de dinero de Chaikin (CMF)** es un indicador técnico desarrollado por Mark Chaikin que mide la fuerza del flujo de dinero (acumulación y distribución) en el mercado durante un período específico.

Para utilizar el indicador, debe utilizar la clase [ChaikinMoneyFlow](xref:StockSharp.Algo.Indicators.ChaikinMoneyFlow).

## Descripción

flujo de dinero de Chaikin (CMF) amplía el concepto de la línea Accumulation/Distribution (Línea A/D), enfocándose en un período de tiempo específico. El indicador mide el volumen del flujo de dinero expresado como porcentaje del volumen total durante el período especificado.

CMF ayuda a los operadores:
- Determinar la fuerza de la presión de compra y venta.
- Identificar tendencias de acumulación (compra) y distribución (venta).
- Detectar divergencias entre el movimiento de precios y el flujo de dinero.
- Confirmar la tendencia actual o su debilidad.

La idea clave de CMF es que en una fuerte tendencia ascendente, el precio de cierre debería estar más cerca del máximo del período, mientras que en una fuerte tendencia a la baja, debería estar más cerca del mínimo del período.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor estándar: 20-21 días)

## Cálculo

El cálculo de CMF implica los siguientes pasos:

1. Calcule el flujo de dinero Multiplier para cada período:
   ```
   multiplicador de flujo de dinero = ((Close - Low) - (High - Close)) / (High - Low)
   ```

   Si (High - Low) = 0, entonces Flujo de dinero Multiplier = 0.

2. Calcule el volumen de flujo de dinero para el período:
   ```
   volumen de flujo de dinero = multiplicador de flujo de dinero * Volume
   ```

3. Calcular flujo de dinero de Chaikin:
   ```
   CMF = Sum(volumen de flujo de dinero durante el periodo Length) / Sum(volumen durante el periodo Length)
   ```

## Interpretación

CMF oscila alrededor de la línea cero y normalmente está dentro del rango de -1 a +1:

- **Valores positivos de CMF** (sobre cero):
  - Indicar presión del comprador (acumulación)
  - Cuanto mayor sea el valor, más fuerte será la presión del comprador.
  - Particularmente significativo si se mantiene durante un período prolongado

- **Valores CMF negativos** (bajo cero):
  - Indicar presión del vendedor (distribución)
  - Cuanto menor sea el valor, más fuerte será la presión del vendedor.
  - La permanencia prolongada en la zona negativa confirma una tendencia a la baja

- **Cruce de línea cero**:
  - Cruzar de abajo hacia arriba puede indicar el inicio de una tendencia alcista
  - Cruzar de arriba a abajo puede indicar el comienzo de una tendencia a la baja

- **Divergencias**:
  - Divergencia alcista: el precio baja mientras que CMF sube (posible reversión al alza)
  - Divergencia bajista: el precio sube mientras que CMF cae (posible reversión a la baja)

- **Niveles extremos**:
  - Los valores superiores a +0,25 pueden indicar una fuerte acumulación
  - Los valores inferiores a -0,25 pueden indicar una fuerte distribución

![Gráfico del indicador CMF](../../../../images/indicator_chaikin_money_flow.png)

## Véase también

[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
[ForceIndex](force_index.md)
[MFI](money_flow_index.md)