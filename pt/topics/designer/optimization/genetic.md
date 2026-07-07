# Genética

O **Designer** suporta otimização tanto pelo [método de força bruta](brute_force.md) como com base em algoritmos genéticos. A otimização genética acelera significativamente o processo de procura dos parâmetros ótimos.

Para ativar a otimização **Genetic**, é necessário:

- mudar o modo:

  ![Designer_Optimization_Genetic_00](../../../images/designer_optimization_genetic_00.png)

- definir os parâmetros de otimização:

  ![Designer_Optimization_Genetic_01](../../../images/designer_optimization_genetic_01.png)

- como função objetivo (Fitness), pode especificar uma fórmula alargada:

  ![Designer_Optimization_Genetic_02](../../../images/designer_optimization_genetic_02.png)

  Por exemplo, efetuar cálculos não apenas por **Profit**, mas também em relação ao seu **Maximum Drawdown**. As funções matemáticas disponíveis são semelhantes às do bloco [Formula](../strategies/using_visual_designer/elements/common/formula.md).

> [!TIP]
> A otimização através de genética não é determinística. Por isso, é impossível determinar o número exato de iterações e, consequentemente, o tempo total necessário, ao contrário da [procura por força bruta](brute_force.md).
