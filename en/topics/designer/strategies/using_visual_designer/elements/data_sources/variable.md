# Variable

![Designer Variable 00](../../../../../../images/designer_variable_00.png)

The cube is used to store values, pass the previously stored value further along the chain of elements.

### Incoming sockets

Incoming sockets

- **Any data** \- the value of the selected type, which will be stored instead of the default value.
- **Trigger** – the signal that determines the point at which it is necessary to pass the stored value through the output socket.

### Outgoing sockets

Outgoing sockets

- **Any data** \- the value of the selected type of passed data.

### Parameters

Parameters

- **Data type** \- the type of stored data within the variable, the type of the input and output parameters depends on the selected data type.
- **Value** \- the default value that is stored in the variable. This value is used if no other values were received to the element input.
- **Raise on start** \- when the checkbox is selected, the value will be passed when the strategy is started.

If the **Instrument** or **Portfolio** data type is selected, the default value may be missing. In this case, if the **Parameters** flag is set in the properties, when the strategy is executed, these data will be taken from the corresponding properties of the strategy.

## Recommended content

[Indexer](../converters/indexer.md)
