# Options quoting

![Designer Quoting 00](~/images/Designer_Quoting_00.png)

The cube is used to quote options according to the specified parameters.

#### Incoming sockets

Incoming sockets

- **Options** \- the list of options from the **Derivative** cube.
- **Volume** \- the volume of quoting.

#### Outgoing sockets

Outgoing sockets

- **Order** \- the registered order that can be used to obtain trades on it by using the **Trades** element by the order and displaying it on the chart using the **Chart panel** cube.

#### Parameters

Parameters

- **Quoting** \- by what parameter the quoting will be conducted, can volatility take values (the volume quoting will be conducted by the specified limits of volatility) and the theoretical price (the volume quoting will be conducted by the specified limits of the theoretical price).
- **Direction** \- the direction of quoting can take the values of Purchase and Sell.
- **Minimum** – the minimum value of volatility or theoretical price.
- **Maximum** \- the maximum value of volatility or theoretical price.

## Recommended content

[Derivatives](Designer_Derivatives.md)
