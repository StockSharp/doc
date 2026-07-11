# MGD

**Dinámica de McGinley (MGD)** es un indicador técnico desarrollado por John R. McGinley que representa una forma avanzada de media móvil, ajustando automáticamente su velocidad en función de los cambios de velocidad del mercado.

Para utilizar el indicador, debe utilizar la clase [McGinleyDynamic](xref:StockSharp.Algo.Indicators.McGinleyDynamic).

## Descripción

Dinámica de McGinley (MGD) fue creado por John McGinley para superar algunos inconvenientes de las medias móviles tradicionales, como el retraso y la incapacidad de adaptarse a los cambios de velocidad del mercado. El indicador ajusta automáticamente su período de reacción dependiendo de la velocidad del movimiento del precio, lo que lo hace más sensible a cambios rápidos y menos propenso a señales falsas.

A diferencia de las medias móviles simples y exponenciales, MGD incorpora una constante de ajuste y la relación entre el precio y el valor del indicador anterior. Esto permite que MGD responda más rápidamente a cambios de precios significativos mientras mantiene la estabilidad durante movimientos más lentos.

La idea principal es que MGD "acelera" durante los movimientos rápidos del mercado y "se desacelera" durante los períodos de consolidación, proporcionando un seguimiento de precios más preciso en comparación con los promedios móviles tradicionales.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor predeterminado: 14)

## Cálculo

McGinley El cálculo dinámico se realiza de forma recursiva utilizando la siguiente fórmula:

```
MGD = MGD[previous] + (Price - MGD[previous]) / (Length * ((Price / MGD[previous])^4))
```

donde:
- Price - precio actual (normalmente precio de cierre)
- MGD[anterior] - valor del indicador anterior
- Length - parámetro de período

Para el valor inicial MGD, normalmente se utiliza una media móvil simple durante el período especificado:

```
Primer cálculo: MGD = SMA(Price, Length)
```

## Interpretación

Dinámica de McGinley se puede interpretar de manera similar a otras medias móviles, pero con sus características mejoradas:

1. **Determinación de tendencias**:
   - Cuando el precio está por encima de MGD, indica una tendencia alcista.
   - Cuando el precio está por debajo de MGD, indica una tendencia a la baja.
   - Una pendiente pronunciada MGD indica una fuerte tendencia

2. **Cruces de precios**:
   - El precio cruza MGD de abajo hacia arriba puede verse como una señal alcista
   - El precio cruza MGD de arriba a abajo puede verse como una señal bajista
   - Debido a su naturaleza adaptativa, estos cruces suelen formarse antes que con las medias móviles tradicionales.

3. **Múltiples cruces MGD**:
   - Se pueden utilizar múltiples MGD con diferentes períodos (por ejemplo, MGD(14) y MGD(30))
   - MGD corto cruzando MGD largo de abajo hacia arriba puede verse como una confirmación de la tendencia alcista
   - MGD corto cruzando MGD largo de arriba a abajo puede verse como una confirmación de la tendencia bajista

4. **Niveles de soporte y resistencia**:
   - MGD suele servir como nivel de soporte dinámico en una tendencia alcista
   - MGD a menudo sirve como nivel de resistencia dinámica en una tendencia a la baja.
   - Múltiples rebotes en MGD confirman la fortaleza de la tendencia

5. **Relación de precio**:
   - La distancia entre el precio y MGD puede indicar condiciones de sobrecompra o sobreventa del mercado.
   - Cuando el precio se desvía significativamente de MGD, puede indicar una posible reversión o corrección.

6. **Combinando con otros indicadores**:
   - MGD funciona bien con osciladores (RSI, estocástico)
   - Puede utilizarse como filtro de tendencias para otros sistemas de trading.

7. **Length Parameter Selection**:
   - Los valores más pequeños de Length (por ejemplo, 8-12) hacen que MGD sea más sensible a los cambios de precios y se adapta al comercio a corto plazo.
   - Los valores Length más altos (por ejemplo, 20-50) hacen que MGD sea más fluido y adecuado para el comercio a largo plazo.

![indicator_mcginley_dynamic](../../../../images/indicator_mcginley_dynamic.png)

## Véase también

[SMA](sma.md)
[EMA](ema.md)
[DEMA](dema.md)
[HMA](hma.md)