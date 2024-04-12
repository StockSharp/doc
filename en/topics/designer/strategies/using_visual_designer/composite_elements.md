# Composite elements

When composing schemas, there often sets of elements appear that are a complete functional and can be used in different schemas or in one schema many times with different values of properties. Such sets of elements can be put into a separate composite element, which will be used further as any ordinary cube.

A composite element is a usual schema that is saved\/loaded\/edited as any strategy schema.

When you add a composite element to a schema, all unconnected parameters of all inner cubes are automatically added to it. Unconnected parameters at the input of cubes are added as input, unconnected parameters at the output as output. Each added parameter is named in the same way as the source element and its parameter. In addition, for this element, the properties of all elements for which the **Parameters** property was specified are added.

We will consider the use of composite elements in the example of the moving average intersection strategy, which illustrates the use of the [Crossing](elements/common/crossing.md) composite element several times. The strategy can open as a long position when the short moving average crosses the long one bottom\-up, and the short position at the when the short moving average crosses the long one top\-down. The schema of the part of the moving averages intersection strategy, where the moment of moving averages intersection is determined, is shown in the figure below:

![Designer Creating a composite elements 00](../../../../images/designer_creating_composite_elements_00.png)

Because the moving averages intersection differs only in the possible direction of intersection (short crosses from top to bottom or from bottom to top), then the part of the schema that determines the moment of intersection can be taken out into a separate composite element. When you add a strategy to the schema, you will specify the properties for determining the algorithm for moving averages intersection. The schema of the composite element by which the intersection is determined is shown in the figure below:

![Designer Crossing 01](../../../../images/designer_crossing_01.png)

The diagram of the composite element consists of simple elements and is based on memorization the current values (Prev In 1 and Prev In 2) and comparing the pairs, current (CurrComparison) and previous (PrevComparison) values with each other. Because each of the input values is used in two elements of the diagram, the elements of [Combination](elements/common/combination.md) (In 1, In 2) are placed at the input of the composite element, and they allow one input to be divided into two elements and pass the input value to the [Comparison](elements/common/comparison.md) and [Prev value](elements/common/prev_value.md) elements. When a new value arrives at the input, the current values are compared and a new value is passed to the [Prev value](elements/common/prev_value.md) element, from which the previous value for the current input is passed, then the previous values are compared. If both conditions are fulfilled, that is checked using the And [Logical condition](elements/common/logical_condition.md), then the value of the raised flag is passed to the output of the composite element, which can be used as a trigger for further action.

For the CurrComparison and PrevComparison cubes, the **Parameters** flag of the **Common** properties group is set. Therefore, the properties of these cubes were taken into the properties of the composite element [Crossing](elements/common/crossing.md), which will be further specified when using a composite element in the strategy schema.

![Designer Crossing 00](../../../../images/designer_crossing_00.png)

## Recommended content

[Display candles on chart](schema_samples/display_candles_on_chart.md)
