# Cotización de opciones

![Designer Quoting 00](../../../../../../images/designer_quoting_00.png)

El cubo se usa para cotizar opciones según los parámetros especificados.

### Sockets de entrada

Sockets de entrada

- **Model** – modelo de cálculo (por ejemplo, Black-Scholes).
- **Volume** - volumen de cotización.

### Sockets de salida

Sockets de salida

- **Order** - orden registrada que puede usarse para obtener sus operaciones usando el elemento **Trades** por orden y mostrarla en el gráfico con el cubo **Chart panel**.

### Parámetros

Parámetros

- **Quoting** - parámetro por el que se realizará la cotización. Puede tomar los valores **Volatility** (el volumen de cotización seguirá los límites de volatilidad especificados) o **Theoretical price** (el volumen de cotización seguirá los límites especificados del precio teórico).
- **Direction** - la dirección de cotización puede tomar los valores Purchase y Sell.
- **Minimum** – valor mínimo de volatilidad o precio teórico.
- **Maximum** - valor máximo de volatilidad o precio teórico.

## Contenido recomendado

[Precios de ejercicio](strikes.md)
