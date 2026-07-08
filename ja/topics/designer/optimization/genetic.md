# 遺伝的最適化

**Designer** は、[総当たり方式](brute_force.md)による最適化と、遺伝的アルゴリズムに基づく最適化の両方をサポートしています。遺伝的最適化により、最適なパラメーターを見つけるプロセスが大幅に高速化されます。

**Genetic** 最適化を有効にするには、次の手順を実行します。

- モードを切り替えます。

  ![Designer_Optimization_Genetic_00](../../../images/designer_optimization_genetic_00.png)

- 最適化パラメーターを設定します。

  ![Designer_Optimization_Genetic_01](../../../images/designer_optimization_genetic_01.png)

- 目的関数（Fitness）として、拡張式を指定できます。

  ![Designer_Optimization_Genetic_02](../../../images/designer_optimization_genetic_02.png)

  たとえば、**Profit** だけでなく、その **Maximum Drawdown** との相対関係でも計算できます。利用可能な数学関数は、[Formula](../strategies/using_visual_designer/elements/common/formula.md) ブロックと同様です。

> [!TIP]
> 遺伝的最適化は決定論的ではありません。そのため、[総当たり検索](brute_force.md)とは異なり、正確な反復回数、したがって必要な総時間を決定することはできません。

