# BMP

﻿# BMP

**Equilíbrio de poder do mercado (BMP)** é um indicador que mede a força dos compradores em relação aos vendedores, com base numa análise dos movimentos de preço e dos volumes de negociação.

Para usar o indicador, é necessário usar a classe [BalanceOfMarketPower](xref:StockSharp.Algo.Indicators.BalanceOfMarketPower).

## Descrição

O indicador Equilíbrio de poder do mercado foi concebido para avaliar a distribuição atual de forças entre compradores e vendedores no mercado. Analisa quanto o preço de fecho se desvia do seu intervalo (máximo-mínimo) e correlaciona isto com o volume de negociação.

O BMP ajuda os operadores a:
- Determinar o lado dominante do mercado (compradores ou vendedores)
- Identificar potenciais reversões de tendência
- Detetar divergências entre o preço e o indicador
- Encontrar níveis de sobrecompra e sobrevenda

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de suavização (valor predefinido: 14)

## Cálculo

O cálculo do BMP ocorre em duas etapas:

1. Calcular o BMP para cada vela individual:
   ```
   BMP bruto = ((Preço de fecho - Preço de abertura) / (High - Low)) * Volume
   ```
   Se (High - Low) for zero, o BMP bruto é definido como zero.

2. Suavizar o BMP usando uma média móvel simples (SMA):
   ```
   BMP = SMA(BMP bruto, Length)
   ```

Onde:
- Preço de fecho - preço de fecho da vela atual
- Preço de abertura - preço de abertura da vela atual
- High - preço mais alto da vela atual
- Low - preço mais baixo da vela atual
- Volume - volume de negociação para o período da vela atual
- Length - período de suavização selecionado

## Interpretação

- **Valores positivos de BMP** indicam dominância de compradores (compradores) no mercado
- **Valores negativos de BMP** indicam dominância de vendedores (vendedores) no mercado
- **Cruzamento da linha zero** pode ser considerado um sinal de alteração de tendência
- **Valores extremos** (acima ou abaixo de certos níveis) podem indicar condições de sobrecompra ou sobrevenda no mercado
- **Divergências** entre BMP e preço podem sinalizar uma potencial reversão de tendência

![Gráfico do indicador BMP](../../../../images/indicator_balance_of_market_power.png)

## Ver também

[BalanceOfPower](balance_of_power.md)
[ForceIndex](force_index.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
