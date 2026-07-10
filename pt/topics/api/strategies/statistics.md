# Estatísticas da Estratégia

## Visão Geral

A plataforma StockSharp fornece um sistema abrangente para análise estatística de estratégias de negociação, que ajuda os traders a avaliar a eficácia, otimizar parâmetros e tomar decisões informadas. O sistema de estatísticas recolhe e processa dados de vários aspetos da negociação, incluindo ordens, transações, posições e indicadores de lucro/perda.

## Objetivo e Benefícios

A análise estatística em estratégias de negociação cumpre várias funções importantes:

1. **Medição de desempenho**: avaliação quantitativa do sucesso da sua estratégia usando métricas como lucro líquido, drawdown máximo e fator de recuperação.

2. **Gestão de risco**: compreensão do perfil de risco da sua estratégia através de métricas como percentagem de drawdown máximo e estatísticas do tamanho da posição.

3. **Otimização**: encontrar parâmetros ideais da estratégia comparando indicadores estatísticos entre diferentes conjuntos de parâmetros.

4. **Análise da qualidade das transações**: análise da distribuição das transações, rácio entre transações lucrativas e perdedoras, lucro médio por transação.

5. **Métricas operacionais**: acompanhamento de métricas operacionais como estatísticas de latência e taxas de erro de ordens para identificar problemas de execução.

## Indicadores Estatísticos Disponíveis

A interface [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager) em StockSharp fornece acesso a numerosos parâmetros estatísticos organizados em várias categorias:

### Estatísticas de Lucro e Perda

- Lucro Líquido
- Lucro Líquido (%)
- Lucro Máximo
- Drawdown Máximo
- Drawdown Máximo (%)
- Drawdown Relativo Máximo
- Fator de Recuperação

### Estatísticas de Transações

- Número de Transações Lucrativas
- Número de Transações Perdedoras
- Número Total de Transações
- Lucro Médio por Transação
- Transação Lucrativa Média
- Transação Perdedora Média
- Número de Transações por Mês/Dia

### Estatísticas de Posição

- Posição Longa Máxima
- Posição Curta Máxima

### Estatísticas de Ordens

- Número de Ordens
- Número de Erros de Ordens
- Atraso Máximo/Mínimo de Registo
- Atraso Máximo/Mínimo de Cancelamento

## Integração com a Classe Strategy

A classe [Strategy](xref:StockSharp.Algo.Strategies.Strategy) recolhe e calcula automaticamente estatísticas durante a execução. O gestor de estatísticas está disponível através da propriedade `StatisticManager`, que implementa a interface [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager).

Os principais valores estatísticos também são representados diretamente como propriedades da classe Strategy:

- `PnL`: valor de lucro e perda
- `Commission`: comissão total paga
- `Slippage`: slippage total
- `Latency`: latência média das operações de ordens

## Visualização

StockSharp fornece um componente gráfico especial para visualizar estatísticas de estratégia chamado `StatisticParameterGrid`, disponível no namespace `StockSharp.Xaml`. Esta grelha apresenta todos os parâmetros estatísticos num formato fácil de usar.

Para mais informações sobre o componente gráfico, consulte a documentação sobre [Estatísticas](../graphical_user_interface/strategies/statistics.md).

## Exemplo de Utilização

Segue-se um exemplo de trabalho com estatísticas de estratégia no seu código:

```csharp
// Criar uma estratégia
var strategy = new SmaStrategy
{
	// Configurar parâmetros da estratégia
	Security = security,
	Portfolio = portfolio,
	Volume = 1,
	// Definir parâmetros SMA
	LongSma = 200,
	ShortSma = 50,
};

// Ligar a estratégia a um gráfico para visualização
var chart = new ChartPanel();
strategy.SetChart(chart);

// Aceder ao gestor de estatísticas
var statisticManager = strategy.StatisticManager;

// Apresentar estatísticas da estratégia na interface de utilizador
// Assumindo que tem um StatisticParameterGrid definido em XAML como 'StatisticsGrid'
StatisticsGrid.Parameters.Clear();
StatisticsGrid.Parameters.AddRange(statisticManager.Parameters);

// Iniciar a estratégia
strategy.Start();

// Quando precisar de reagir a alterações nas estatísticas
strategy.PnLChanged += () =>
{
	Console.WriteLine($"Current PnL: {strategy.PnL}");
	
	// Também pode aceder a parâmetros estatísticos individuais
	var netProfit = statisticManager.Parameters
		.OfType<NetProfitParameter>()
		.FirstOrDefault();
		
	if (netProfit != null)
	{
		Console.WriteLine($"Net Profit: {netProfit.Value}");
	}
};

// Para acompanhar estatísticas de posição
strategy.PositionChanged += () =>
{
	Console.WriteLine($"Current Position: {strategy.Position}");
};
```

## Estatísticas Personalizadas

Também pode criar os seus próprios parâmetros estatísticos implementando as interfaces apropriadas:

- [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter): interface base para todos os parâmetros estatísticos
- [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter): para parâmetros relacionados com lucro/perda
- [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter): para parâmetros relacionados com transações
- [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter): para parâmetros relacionados com posições
- [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter): para parâmetros relacionados com ordens

Segue-se um exemplo simples de um parâmetro estatístico personalizado:

```csharp
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = "Meu indicador personalizado",
	Description = "Descrição do meu indicador personalizado",
	GroupName = "Custom Parameters",
	Order = 1000
)]
public class MyCustomParameter : BasePnLStatisticParameter<decimal>
{
	public MyCustomParameter()
		: base(StatisticParameterTypes.Custom)
	{
	}

	public override void Add(DateTimeOffset marketTime, decimal pnl, decimal? commission)
	{
		// Lógica de cálculo personalizada
		Value = /* your custom calculation */;
	}
}

// Depois adicioná-lo ao StatisticManager da sua estratégia
strategy.StatisticManager.Parameters.Add(new MyCustomParameter());
```

## Conclusão

O sistema de análise estatística em StockSharp fornece aos traders ferramentas poderosas para avaliar e otimizar as suas estratégias de negociação. Ao usar estas estatísticas, pode obter informações valiosas sobre o desempenho da sua estratégia, identificar áreas de melhoria e tomar decisões baseadas em dados para melhorar os resultados de negociação.
