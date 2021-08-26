# Combination

![Designer Association 00](~/images/Designer_Association_00.png)

The cube is used to combine the same type of data coming from different cubes into one output parameter for further passing to some element. Or dividing the received data into several elements.

#### Incoming sockets

Incoming sockets

- **Any data** \- specifies the type of data received and passed.

#### Outgoing sockets

Outgoing sockets

- **Any data** \- specifies the type of data received and passed.

#### Parameters

Parameters

- **Type** \- specifies the type of data received and passed.

An example of using a cube is given in [Crossing](Designer_Crossing.md), [Conditional operator](Designer_Conditional_operator.md) sections. For example, in the **Intersection** element, the values of two lines are passed to input, which are used by different inner cubes. In order for the cube does not have duplicate parameters for the same data, there are **Combination** cubes at the input that allow you to divide the incoming data into several cubes of elements.

## Recommended content

[Position](Designer_Position.md)
