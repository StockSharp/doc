# Options quoting

![Designer Quoting 00](../../../../../../images/designer_quoting_00.png)

The cube is used to quote options according to the specified parameters.

### Incoming sockets

Incoming sockets

- **Model** – the calculation model (for example, Black-Scholes).
- **Volume** \- the volume of quoting.

### Outgoing sockets

Outgoing sockets

- **Order** \- the registered order that can be used to obtain trades on it by using the **Trades** element by the order and displaying it on the chart using the **Chart panel** cube.

### Parameters

Parameters

- **Quoting** \- the parameter by which quoting will be performed. It can take the values **Volatility** (the quoting volume will follow the specified volatility limits) or **Theoretical price** (the quoting volume will follow the specified limits of the theoretical price).
- **Direction** \- the direction of quoting can take the values of Purchase and Sell.
- **Minimum** – the minimum value of volatility or theoretical price.
- **Maximum** \- the maximum value of volatility or theoretical price.

## Recommended content

[Derivatives](strikes.md)
