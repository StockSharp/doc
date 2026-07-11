# BBP

﻿# BBP

**percentual de Bollinger B (BBP)** é um indicador desenvolvido por John Bollinger como complemento do indicador bandas de Bollinger. O BBP mostra a localização do preço em relação às bandas de Bollinger superior e inferior.

Para usar o indicador, é necessário usar a classe [BollingerPercentB](xref:StockSharp.Algo.Indicators.BollingerPercentB).

## Descrição

O indicador percentual de Bollinger B determina a posição do preço em relação às bandas de Bollinger superior e inferior como um valor percentual de 0 a 1 (ou de 0% a 100%). Isto permite uma determinação mais precisa da posição do preço no contexto das bandas de Bollinger:

- Um valor de 1 (ou 100%) significa que o preço está na banda de Bollinger superior.
- Um valor de 0 (ou 0%) significa que o preço está na banda de Bollinger inferior.
- Um valor de 0.5 (ou 50%) significa que o preço está na banda de Bollinger intermédia (SMA).

O BBP também pode assumir valores fora do intervalo 0-1:
- Valores acima de 1 indicam que o preço está acima da banda de Bollinger superior.
- Valores abaixo de 0 indicam que o preço está abaixo da banda de Bollinger inferior.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo da SMA (valor predefinido: 20)
- **StdDevMultiplier** - multiplicador do desvio padrão para calcular bandas de Bollinger (valor predefinido: 2)

## Cálculo

O cálculo do percentual de Bollinger B baseia-se na fórmula:

```
BBP = (Price - banda inferior de Bollinger) / (banda superior de Bollinger - banda inferior de Bollinger)
```

Onde:
- Price - preço atual (normalmente preço de fecho)
- Banda inferior Bollinger = SMA - (StdDevMultiplier * desvio padrão)
- Banda superior Bollinger = SMA + (StdDevMultiplier * desvio padrão)
- SMA - média móvel simples ao longo do período Length
- Desvio-padrão - desvio padrão do preço ao longo do período Length

## Utilização

percentual de Bollinger B pode ser usado de várias formas:

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
