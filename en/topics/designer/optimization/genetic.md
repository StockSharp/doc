# Genetics

The **Designer** supports optimization both by the [brute force method](brute_force.md) and based on genetic algorithms. Genetic optimization significantly speeds up the process of finding optimal parameters.

To enable **Genetic** optimization, you need to:

- switch the mode:

  ![Designer_Optimization_Genetic_00](../../../images/designer_optimization_genetic_00.png)

- set the optimization parameters:

  ![Designer_Optimization_Genetic_01](../../../images/designer_optimization_genetic_01.png)

- as the target function (Fitness), you can specify an extended formula:

  ![Designer_Optimization_Genetic_02](../../../images/designer_optimization_genetic_02.png)

  For example, make calculations not only by **Profit** but also relative to its **Maximum Drawdown**. Available mathematical functions are similar to the [Formula](../strategies/using_visual_designer/elements/common/formula.md) block.

> [!TIP]
> Optimization through genetics is not deterministic. Therefore, determining the exact number of iterations and, consequently, the necessary total time is impossible, unlike [brute-force search](brute_force.md).