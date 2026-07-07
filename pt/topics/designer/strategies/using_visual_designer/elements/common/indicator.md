# Indicator

![Designer Indicator 00](../../../../../../images/designer_indicator_00.png)

Este bloco é usado para calcular valores de indicadores.

## Sockets de entrada

- **Any Data** - um tipo específico de dados com base no qual o indicador selecionado deve ser calculado (dependendo do indicador, pode ser um valor numérico, uma vela, etc.).

## Sockets de saída

- **Indicator** - o valor calculado do indicador, que pode ser usado para apresentação no painel de gráfico ou para cálculos posteriores.

## Parâmetros

- **Indicator Type** - um parâmetro usado para selecionar o indicador pretendido e vários parâmetros adicionais que correspondem ao tipo de indicador selecionado. O conjunto destes parâmetros muda quando o tipo de indicador selecionado muda.
- **Final** - passar apenas [valores finais](../../../../../api/indicators.md) do indicador.
- **Formed** - passar apenas valores quando o indicador estiver totalmente [formado](../../../../../api/indicators.md).

![Designer Indicator 01](../../../../../../images/designer_indicator_01.png)

## Ver também

[List of Indicators](../../../../../api/indicators/list_of_indicators.md)
[Logical Condition](logical_condition.md)
