# WCCI

**Woodies CCI (WCCI)** es una modificación del estándar Commodity Channel Index (CCI), desarrollado por el operador Ken Wood (conocido como "Woodies"). Esta variación CCI incluye suavizado adicional y se utiliza como parte de un sistema comercial integral Woodies CCI.

Para utilizar el indicador, debe utilizar la clase [WoodiesCCI](xref:StockSharp.Algo.Indicators.WoodiesCCI).

## Descripción

Woodies CCI es una versión modificada del indicador clásico CCI que incluye dos líneas:
- La línea principal CCI con un período seleccionado (normalmente 14)
- Una línea CCI suavizada, que es una media móvil simple de la línea principal CCI

El sistema Woodies CCI utiliza estas dos líneas, junto con varios niveles clave para generar señales de trading. Los niveles principales incluyen:
- +100 y -100 (niveles tradicionales de sobrecompra y sobreventa)
- +200 y -200 (fuertes condiciones de sobrecompra y sobreventa)
- Línea cero (importante para la determinación de tendencias)

Señales clave en el sistema Woodies CCI:
- "Rechazo de línea cero": cuando CCI se acerca a la línea cero y luego rebota en ella, continuando en la dirección anterior
- "Rotura de línea de tendencia": cuando CCI rompe una línea de tendencia significativa
- "Reverse divergencia": un tipo específico de divergencia entre el precio y CCI

## Parámetros

- **Length** - período de cálculo para la línea principal CCI (normalmente 14)
- **SMALength** - período para suavizar la línea principal CCI para obtener la segunda línea (normalmente 9)

## Cálculo

El cálculo de Woodies CCI se realiza en varios pasos:

1. Primero, calcule el CCI estándar:
   ```
   Typical Price (TP) = (High + Low + Close) / 3
   Average Value (SMA) = SMA(TP, Length)
   Mean Deviation (MD) = Sum(|TP - SMA|) / Length
   CCI = (TP - SMA) / (0.015 * MD)
   ```

2. Luego calcule la línea CCI suavizada:
   ```
   Smooth CCI = SMA(CCI, SMALength)
   ```

Woodies CCI utiliza una combinación de estas dos líneas para crear señales de trading. En el sistema Woodies clásico, el cruce de estas líneas, su interacción con niveles clave y varios patrones forman la base para las decisiones de trading.

![IndicatorWoodiesCCI](../../../../images/indicator_woodies_cci.png)

## Véase también

[CCI](cci.md)