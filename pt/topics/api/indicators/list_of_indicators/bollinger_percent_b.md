# BBP

﻿# BBP

**Bollinger Percent B (BBP)** é um indicador desenvolvido por John Bollinger como complemento do indicador Bollinger Bands. O BBP mostra a localização do preço em relação às Bollinger Bands superior e inferior.

Para usar o indicador, é necessário usar a classe [BollingerPercentB](xref:StockSharp.Algo.Indicators.BollingerPercentB).

## Descrição

O indicador Bollinger Percent B determina a posição do preço em relação às Bollinger Bands superior e inferior como um valor percentual de 0 a 1 (ou de 0% a 100%). Isto permite uma determinação mais precisa da posição do preço no contexto das Bollinger Bands:

- Um valor de 1 (ou 100%) significa que o preço está na Bollinger Band superior.
- Um valor de 0 (ou 0%) significa que o preço está na Bollinger Band inferior.
- Um valor de 0.5 (ou 50%) significa que o preço está na Bollinger Band intermédia (SMA).

O BBP também pode assumir valores fora do intervalo 0-1:
- Valores acima de 1 indicam que o preço está acima da Bollinger Band superior.
- Valores abaixo de 0 indicam que o preço está abaixo da Bollinger Band inferior.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo da SMA (valor predefinido: 20)
- **StdDevMultiplier** - multiplicador do desvio padrão para calcular Bollinger Bands (valor predefinido: 2)

## Cálculo

O cálculo do Bollinger Percent B baseia-se na fórmula:

```
BBP = (Price - Lower Bollinger Band) / (Upper Bollinger Band - Lower Bollinger Band)
```

Onde:
- Price - preço atual (normalmente preço de fecho)
- Lower Bollinger Band = SMA - (StdDevMultiplier * Standard Deviation)
- Upper Bollinger Band = SMA + (StdDevMultiplier * Standard Deviation)
- SMA - média móvel simples ao longo do período Length
- Standard Deviation - desvio padrão do preço ao longo do período Length

## Utilização

Bollinger Percent B pode ser usado de várias formas:

1. **Identificar condições de sobrecompra/sobrevenda**:
   - Valores acima de 1 indicam um mercado em sobrecompra
   - Valores abaixo de 0 indicam um mercado em sobrevenda

2. **Sinais de reversão**:
   - Quando o BBP regressa ao intervalo 0-1 depois de sair dele
   - Divergências entre BBP e preço

3. **Determinação de tendência**:
   - Valores de BBP consistentemente acima de 0.5 indicam uma tendência ascendente
   - Valores de BBP consistentemente abaixo de 0.5 indicam uma tendência descendente

4. **Encontrar níveis ocultos de suporte e resistência**:
   - Os níveis 0.8 e 0.2 são frequentemente usados como níveis adicionais de suporte e resistência

![indicator_bollinger_percent_b](../../../../images/indicator_bollinger_percent_b.png)

## Ver também

[BollingerBands](bollinger_bands.md)
[StdDev](standard_deviation.md)
[RSI](rsi.md)
