# Генетика

**Дизайнер** поддерживает как оптимизацию [методом перебора](brute_force.md), так и на основе генетических алгоритмов. Генетическая оптимизация позволяет значительно ускорить процесс поиска оптимальных параметров.

Чтобы включить **Генетическую** оптимизацию, необходимо:

- переключить режим:

  ![Designer_Optimization_Genetic_00](../../../images/designer_optimization_genetic_00.png)

- задать параметры оптимизации:

  ![Designer_Optimization_Genetic_01](../../../images/designer_optimization_genetic_01.png)

- в качестве целевой функции (Фитнес) можно указать расширенную формулу:

  ![Designer_Optimization_Genetic_02](../../../images/designer_optimization_genetic_02.png)

  Например, сделать расчет не только по **Прибыли**, но так же относительно ее к **Максимальной Просадке**. Доступные математические функции аналогичны кубику [Формула](../strategies/using_visual_designer/elements/common/formula.md).

> [!TIP]
> Оптимизация через генетику не детерминирована. Поэтому определение точного количества итераций и, как следствие, необходимого общего времени - невозможно в отличие от [перебора через брут-форс](brute_force.md).
