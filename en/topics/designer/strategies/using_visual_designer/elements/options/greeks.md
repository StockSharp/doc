# Greeks

![Designer Greek 00](../../../../../../images/designer_greek_00.png)

This block is used to calculate the main "Greeks": Delta, Gamma, Vega, Theta, Rho at the current moment.

### Incoming Sockets

Incoming Sockets

- **Model** � the calculation model (for example, Black-Scholes).
- **Price of the Underlying Asset** � the price of the underlying asset.
- **Maximum Deviation** � the maximum deviation.

### Outgoing Sockets

Outgoing Sockets

- **Result** � the result of calculating the main "Greeks": Delta, Gamma, Vega, Theta, Rho at the current moment.

### Parameters

Parameters

- **Value** � can take the type of "Greek" Delta, Gamma, Vega, Theta, Rho and determines what value will be output from the block.

## See Also

[Hedging](black_scholes.md)