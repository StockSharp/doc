# MGD

**Dinâmica de McGinley (MGD)** é um indicador técnico desenvolvido por John R. McGinley que representa uma forma avançada de média móvel, ajustando automaticamente a sua velocidade com base em alterações da velocidade do mercado.

Para utilizar o indicador, é necessário usar a classe [McGinleyDynamic](xref:StockSharp.Algo.Indicators.McGinleyDynamic).

## Descrição

O Dinâmica de McGinley (MGD) foi criado por John McGinley para superar algumas desvantagens das médias móveis tradicionais, como o atraso e a incapacidade de se adaptar a alterações da velocidade do mercado. O indicador ajusta automaticamente o seu período de reacção consoante a velocidade do movimento do preço, tornando-o mais sensível a alterações rápidas e menos propenso a sinais falsos.

Ao contrário das médias móveis simples e exponenciais, o MGD incorpora uma constante de ajuste e o rácio entre o preço e o valor anterior do indicador. Isto permite que o MGD responda mais rapidamente a alterações significativas de preço, mantendo estabilidade durante movimentos mais lentos.

A ideia principal é que o MGD "acelera" durante movimentos rápidos do mercado e "abranda" durante períodos de consolidação, proporcionando um acompanhamento de preço mais preciso em comparação com médias móveis tradicionais.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 14)

## Cálculo

O cálculo do Dinâmica de McGinley é efectuado recursivamente usando a seguinte fórmula:

```
MGD = MGD[previous] + (Price - MGD[previous]) / (Length * ((Price / MGD[previous])^4))
```

Onde:
- Price - preço actual (normalmente o preço de fecho)
- MGD[previous] - valor anterior do indicador
- Length - parâmetro de período

Para o valor inicial do MGD, normalmente é usada uma média móvel simples durante o período especificado:

```
Primeiro cálculo: MGD = SMA(Price, Length)
```

## Interpretação

O Dinâmica de McGinley pode ser interpretado de forma semelhante a outras médias móveis, mas com as suas características melhoradas:

1. **Determinação da Tendência**:
   - Quando o preço está acima do MGD, indica uma tendência ascendente
   - Quando o preço está abaixo do MGD, indica uma tendência descendente
   - Uma inclinação acentuada do MGD indica uma tendência forte

2. **Cruzamentos de Preço**:
   - O preço cruzar o MGD de baixo para cima pode ser visto como um sinal altista
   - O preço cruzar o MGD de cima para baixo pode ser visto como um sinal baixista
   - Devido à sua natureza adaptativa, estes cruzamentos normalmente formam-se mais cedo do que com médias móveis tradicionais

3. **Cruzamentos de Múltiplos MGD**:
   - Podem ser usados vários MGD com períodos diferentes (por exemplo, MGD(14) e MGD(30))
   - Um MGD curto cruzar um MGD longo de baixo para cima pode ser visto como confirmação de tendência altista
   - Um MGD curto cruzar um MGD longo de cima para baixo pode ser visto como confirmação de tendência baixista

4. **Níveis de Suporte e Resistência**:
   - O MGD serve frequentemente como nível de suporte dinâmico numa tendência ascendente
   - O MGD serve frequentemente como nível de resistência dinâmico numa tendência descendente
   - Vários ressaltos no MGD confirmam a força da tendência

5. **Relação com o Preço**:
   - A distância entre o preço e o MGD pode indicar condições de sobrecompra ou sobrevenda do mercado
   - Quando o preço se desvia significativamente do MGD, pode sinalizar uma potencial inversão ou correcção

6. **Combinação com Outros Indicadores**:
   - O MGD funciona bem com osciladores (RSI, Estocástico)
   - Pode ser usado como filtro de tendência para outros sistemas de negociação

7. **Selecção do Parâmetro Length**:
   - Valores Length menores (por exemplo, 8-12) tornam o MGD mais sensível a alterações de preço e adequam-se à negociação de curto prazo
   - Valores Length maiores (por exemplo, 20-50) tornam o MGD mais suave e adequam-se à negociação de longo prazo

![MGD](../../../../images/indicator_mcginley_dynamic.png)

## Ver Também

[SMA](sma.md)
[EMA](ema.md)
[DEMA](dema.md)
[HMA](hma.md)

