# Lines

The strategy in the designer is a schema of a set of elements and links between them, called connections. Each connection goes from the output parameter of one cube to the input parameter of another cube. Usually, all connection lines are colored gray, but when you point to the cube to which they belong, the lines are painted black.

![Designer Line 00](../../../../images/designer_line_00.png)

Each connection can be highlighted by pointing over it and clicking the left mouse button. The selected connection will be marked with circles at the ends of the line, taking for which, you can redirect the line. If you press the Del button on the selected line, it will be deleted.

You can connect to each other the parameters of the same colors (the same data types), except for the following types of parameters:

- The **black** parameter can accept any data. Most often, these parameters are used to pass signals for any actions within the element. For example, the [Variable](elements/data_sources/variable.md) element stores some value and, when it receives a signal, it passes the value to the output.
- The **green** parameter can accept different compared data types. For example, numeric, indicator values, strings, etc.

## Recommended content

[Event model](event_model.md)
