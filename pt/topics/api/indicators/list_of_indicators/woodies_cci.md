# WCCI

**CCI de Woodies (WCCI)** é uma modificação do índice de canal de commodities (CCI) padrão, desenvolvida pelo operador Ken Wood (conhecido como "Woodies"). Esta variação do CCI inclui suavização adicional e é usada como parte de um sistema de negociação CCI de Woodies abrangente.

Para usar o indicador, é necessário usar a classe [WoodiesCCI](xref:StockSharp.Algo.Indicators.WoodiesCCI).

## Descrição

CCI de Woodies é uma versão modificada do indicador CCI clássico que inclui duas linhas:
- A linha CCI principal com um período selecionado (tipicamente 14)
- Uma linha CCI suavizada, que é uma média móvel simples da linha CCI principal

O sistema CCI de Woodies usa estas duas linhas, juntamente com vários níveis-chave, para gerar sinais de negociação. Os níveis principais incluem:
- +100 e -100 (níveis tradicionais de sobrecompra e sobrevenda)
- +200 e -200 (condições fortes de sobrecompra e sobrevenda)
- Linha zero (importante para determinar a tendência)

Sinais principais no sistema CCI de Woodies:
- "Rejeição da linha zero" - quando o CCI se aproxima da linha zero e depois ressalta dela, continuando na direção anterior
- "rompimento de linha de tendência" - quando o CCI rompe uma linha de tendência significativa
- "divergência inversa" - um tipo específico de divergência entre preço e CCI

## Parâmetros

- **Período** - período de cálculo da linha CCI principal (tipicamente 14)
- **Período da SMA** - período para suavizar a linha CCI principal e obter a segunda linha (tipicamente 9)

## Cálculo

O cálculo do CCI de Woodies é efetuado em vários passos:

1. Primeiro, calcular o CCI padrão:
   ```
   Preço típico (TP) = (High + Low + Close) / 3
   Valor médio (SMA) = SMA(TP, Length)
   Desvio médio (MD) = Sum(|TP - SMA|) / Length
   CCI = (TP - SMA) / (0.015 * MD)
   ```

2. Depois, calcular a linha CCI suavizada:
   ```
   Smooth CCI = SMA(CCI, SMALength)
   ```

CCI de Woodies usa uma combinação destas duas linhas para criar sinais de negociação. No sistema Woodies clássico, o cruzamento destas linhas, a sua interação com níveis-chave e vários padrões formam a base das decisões de negociação.

![Gráfico do indicador WCCI](../../../../images/indicator_woodies_cci.png)

## Ver também

[CCI](cci.md)
