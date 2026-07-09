# Cobertura

![Designer Hedging 00](../../../../../../images/designer_hedging_00.png)

El cubo se usa para cubrir posiciones en opciones.

### Sockets de entrada

Sockets de entrada

- **Model** – modelo de cálculo (por ejemplo, Black-Scholes).
- **Instrumento** – instrumento, el activo subyacente.
- **Volume** - valor numérico del volumen.
- **Position by underlying asset** – posición por el activo subyacente.
- **Flag** – señal (bandera) que inicia el proceso de cobertura.

### Sockets de salida

Sockets de salida

- **Order** – orden registrada que puede usarse para obtener sus operaciones usando el elemento Trades por orden y mostrarla en el gráfico usando el cubo Chart panel.

### Parámetros

Parámetros

- **Hedging type** - tipo de cobertura; puede tomar los valores Delta, Gamma, Vega, Theta o Rho.

## Contenido recomendado

[Cotización de opciones](options_quoting.md)
