# VI

**Vortex Indicator (VI)** é um indicador técnico desenvolvido por Etienne e Julia Boisse em 2009. O indicador é composto por duas linhas, VI+ e VI-, que mostram o movimento ascendente e descendente do preço, ajudando a identificar o início de novas tendências e a confirmar as existentes.

Para usar o indicador, é necessário usar a classe [VortexIndicator](xref:StockSharp.Algo.Indicators.VortexIndicator).

## Descrição

O Vortex Indicator é inspirado nos princípios do movimento em vórtice na natureza e pretende refletir a natureza cíclica dos movimentos do mercado. É composto por duas linhas:

- **VI+** (Vortex Indicator positivo) - mede o movimento ascendente do preço
- **VI-** (Vortex Indicator negativo) - mede o movimento descendente do preço

Principais sinais do indicador:
- Comprar quando VI+ cruza VI- de baixo para cima
- Vender quando VI- cruza VI+ de baixo para cima
- O grau de separação entre as linhas indica a força da tendência

O Vortex Indicator é particularmente útil para:
- Determinar o início de novas tendências
- Avaliar a força de uma tendência existente
- Identificar potenciais pontos de inversão

## Parâmetros

- **Length** - período de cálculo, usando tipicamente um valor de 14.

## Cálculo

O cálculo do Vortex Indicator é efetuado em vários passos:

1. Calcular o movimento positivo e negativo:
   ```
   VM+ = |Current High - Previous Low|
   VM- = |Current Low - Previous High|
   ```

2. Calcular o True Range:
   ```
   TR = Max(High - Low, |High - Previous Close|, |Low - Previous Close|)
   ```

3. Somar os valores VM+ e VM- ao longo do período Length:
   ```
   Sum_VM+ = Sum(VM+, Length)
   Sum_VM- = Sum(VM-, Length)
   ```

4. Somar o True Range ao longo do período Length:
   ```
   Sum_TR = Sum(TR, Length)
   ```

5. Calcular os valores normalizados de VI+ e VI-:
   ```
   VI+ = Sum_VM+ / Sum_TR
   VI- = Sum_VM- / Sum_TR
   ```

O cruzamento destas duas linhas gera sinais de negociação: quando VI+ sobe acima de VI-, sinaliza uma tendência altista; inversamente, quando VI- sobe acima de VI+, sinaliza uma tendência baixista.

![IndicatorVortexIndicator](../../../../images/indicator_vortex_indicator.png)

## Ver também

[ADX](adx.md)
[DMI](dmi.md)
