# APZ

﻿# APZ

**Adaptive Price Zone (APZ)** é um indicador técnico desenvolvido por Lee Leibfarth que cria zonas dinâmicas de suporte e resistência, adaptando-se à volatilidade do mercado.

Para usar o indicador, é necessário usar a classe [AdaptivePriceZone](xref:StockSharp.Algo.Indicators.AdaptivePriceZone).

## Descrição

O indicador APZ é composto por duas linhas (superior e inferior) que formam uma zona de preço em torno do preço médio. Esta zona expande-se e contrai-se consoante a volatilidade atual do mercado. Quando o mercado se torna mais volátil, a zona expande-se; quando a volatilidade diminui, a zona estreita-se.

O APZ é especialmente útil para:
- Identificar potenciais níveis de suporte e resistência
- Detetar possíveis pontos de reversão de tendência
- Revelar períodos de maior e menor volatilidade
- Criar sistemas de negociação baseados em ruturas da zona de preço

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Period** - período de cálculo (valor predefinido: 5)
- **BandPercentage** - percentagem do intervalo para definir a largura da banda (valor predefinido: 2%)

## Cálculo

O cálculo do APZ baseia-se na média móvel exponencial (EMA) e no intervalo verdadeiro médio (ATR):

1. Primeiro, calcule a EMA do preço para o período especificado:
   ```
   EMA = média móvel exponencial do preço durante Period
   ```

2. Depois calcule a volatilidade usando ATR:
   ```
   Volatility = média móvel exponencial do ATR durante Period
   ```

3. As linhas superior e inferior do APZ são calculadas da seguinte forma:
   ```
   Upper Line = EMA + (Volatility * BandPercentage)
   Lower Line = EMA - (Volatility * BandPercentage)
   ```

Quando o preço está acima da linha superior do APZ, isto pode ser considerado uma tendência ascendente. Quando o preço está abaixo da linha inferior do APZ, isto pode indicar uma tendência descendente. Quando o preço se move dentro da zona APZ, o mercado pode estar numa fase de consolidação ou movimento lateral.

![indicator_adaptive_price_zone](../../../../images/indicator_adaptive_price_zone.png)

## Ver também

[BollingerBands](bollinger_bands.md)
[KeltnerChannels](keltner_channels.md)
[DonchianChannels](donchian_channels.md)
