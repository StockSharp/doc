# Padrões

**Padrão** (do inglês: pattern - modelo, amostra) - em análise técnica, refere-se a combinações estáveis e recorrentes de dados de preço, volume ou indicadores. A análise de padrões baseia-se num dos axiomas da análise técnica: "a história repete-se" - acredita-se que combinações recorrentes de dados conduzem a resultados semelhantes.

Os padrões também são chamados de "**modelos**" ou "**figuras**" da análise técnica.

Os padrões são convencionalmente divididos em:

- Indeterminados (podem levar tanto à continuação como à alteração da tendência atual).
- Padrões de continuação da tendência atual.
- Padrões de reversão da tendência existente.

## Utilizar Padrões

### No Designer

O [Designer](../designer.md) tem padrões de velas predefinidos integrados que podem ser utilizados na sua estratégia de negociação. Os padrões são chamados através do cubo [indicador](../designer/strategies/using_visual_designer/elements/common/indicator.md), com a seleção subsequente do valor correspondente. O próprio padrão é selecionado na lista pendente na janela à direita.

![IndicatorPatternCommon](../../images/indicatorpatterncommon00.png)

Também é possível editar os padrões existentes e adicionar os seus próprios padrões personalizados. Para isso, tem de clicar no botão ![Designer edit button](../../images/designer_creating_repository_of_historical_data_01.png), após o que será apresentada a janela de edição de padrões.

![Captura de tela de Padrões](../../images/indicatorpatterncommon01.png)

Para criar o seu próprio padrão, clique no botão ![Designer botão Mais](../../images/designer_panel_circuits_01_button.png) na parte superior da janela. Clicar no botão ![Designer botão Eliminar](../../images/designer_delete_button.png) elimina o padrão.

### No Terminal

No [Terminal](../terminal.md), os padrões são adicionados ao gráfico como qualquer outro indicador. Para isso, basta clicar com o botão direito do rato no gráfico e selecionar o indicador apropriado na lista dos disponíveis.

### Na API StockSharp

Ao utilizar [S#](../api.md) (ou ao criar [estratégias a partir de código](../designer/strategies/using_code.md) no Designer), o trabalho com padrões é feito como com qualquer outro indicador. Exemplo de utilização:

```cs
// Criar um indicador de padrão
var patternIndicator = new CandlePatternIndicator
{
	// Definir o padrão pretendido
	Pattern = new ExpressionCandlePattern("Meu padrão", new[]
	{
		new CandleExpressionCondition(Paths.FileSystem, "C > O"), // A vela atual é ascendente
		new CandleExpressionCondition(Paths.FileSystem, "pC < pO") // A vela anterior é descendente
	})
};

// Adicionar o indicador à coleção
Indicators.Add(patternIndicator);

// Processar uma vela
var result = patternIndicator.Process(candle);

// Verificar o resultado
if (result.GetValue<bool>())
{
	// Padrão detetado, executar as ações necessárias
}
```

## Formato de Descrição do Padrão

Ao editar um padrão, cada linha representa uma vela separada. A linha superior é a vela atual; consequentemente, a segunda linha é uma vela atrás, a terceira e as seguintes são menos 2 e mais velas.

O editor utiliza os seguintes parâmetros:
- O - preço de abertura,
- H - máximo,
- L - mínimo,
- C - preço de fecho,
- V - volume,
- OI - interesse aberto,
- B - corpo da vela,
- LEN - comprimento da vela (do máximo ao mínimo),
- BS - sombra inferior da vela,
- TS - sombra superior da vela.

Com os parâmetros, é possível utilizar os seguintes índices (referências) para os valores pretendidos. Por exemplo, para o preço de fecho:
- C: preço de fecho da vela atual,
- C1: preço de fecho da 1.ª vela após a atual,
- C2: preço de fecho da 2.ª vela após a atual,
- pC: preço de fecho da vela anterior,
- pC1: preço de fecho da vela antes da anterior,
Todas as referências têm de estar dentro do intervalo do padrão atual. Por exemplo, o intervalo do padrão 3 Black Crows é composto pela vela atual e pelas duas velas anteriores, pelo que não é permitido referir a terceira vela anterior.

Para verificação adicional de parâmetros em correlação, é utilizada a expressão &&, que representa um AND lógico.

Ao descrever um padrão, também é possível utilizar as seguintes funções: abs, acos, asin, atan, ceiling, cos, exp, floor, log, log10, max, min, pow, round, sign, sin, sqrt, tan, truncate. Mais informações sobre a utilização de funções são explicadas na descrição do cubo [Fórmula](../designer/strategies/using_visual_designer/elements/common/formula.md).

Ao utilizar [ExpressionCandlePattern](xref:StockSharp.Algo.Candles.Patterns.ExpressionCandlePattern) em código, as fórmulas são criadas pelas mesmas regras descritas acima e utilizam as mesmas variáveis.

## Padrões Standard

Para criar rapidamente padrões com base nos existentes, pode utilizar a secção na parte inferior da janela do editor de padrões. Clicar no botão ![Designer botão Mais](../../images/designer_panel_circuits_01_button.png) na parte inferior da janela adiciona à janela de edição a lógica do padrão selecionado na lista pendente oposta. O botão ![Designer botão Eliminar](../../images/designer_delete_button.png) na parte inferior da janela elimina a linha selecionada na janela de edição.

## Funcionalidades Avançadas

- [ComplexCandlePattern](xref:StockSharp.Algo.Candles.Patterns.ComplexCandlePattern) - permite combinar vários padrões num único padrão composto para análises mais complexas.
- [ICandlePatternProvider](xref:StockSharp.Algo.Candles.Patterns.ICandlePatternProvider) - interface de fornecedor de padrões que permite carregar e guardar padrões personalizados.
