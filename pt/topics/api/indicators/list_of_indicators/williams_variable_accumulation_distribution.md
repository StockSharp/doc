# WVAD

**acumulação/distribuição variável de Williams (WVAD)** é um indicador de volume cumulativo desenvolvido por Larry Williams. Avalia a pressão compradora e vendedora analisando a relação entre o preço de abertura, o preço de fecho, o máximo, o mínimo e o volume de negociação.

Para usar o indicador, use a classe [WilliamsVariableAccumulationDistribution](xref:StockSharp.Algo.Indicators.WilliamsVariableAccumulationDistribution).

## Descrição

O indicador WVAD mede quanto os compradores ou vendedores controlam o movimento do preço em cada barra, e pondera este valor pelo volume. Se o preço de fecho for superior ao preço de abertura, indica domínio comprador, e vice-versa. O intervalo máximo-mínimo é usado como fator de normalização.

Principais aplicações do indicador:
- Confirmar a tendência atual
- Identificar divergências entre o indicador e o preço
- Determinar a pressão compradora ou vendedora
- Avaliar a força do movimento do preço tendo o volume em conta

## Cálculo

O indicador WVAD é calculado usando a seguinte fórmula:

```
WVAD = WVAD(previous) + ((Close - Open) / (High - Low)) * Volume
```

onde:
- Close - preço de fecho do período atual
- Open - preço de abertura do período atual
- High - preço mais alto do período atual
- Low - preço mais baixo do período atual
- Volume - volume de negociação do período atual
- WVAD(previous) - valor anterior do indicador

Se High = Low (o intervalo é zero), o valor desse período não é adicionado.

O indicador é cumulativo -- os valores acumulam-se a cada novo período.

## Ver também

[WAD](williams_accumulation_distribution.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
