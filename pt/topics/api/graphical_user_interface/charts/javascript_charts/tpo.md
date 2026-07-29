# TPO (Perfil de mercado)

Um gráfico TPO (*Time Price Opportunity*), também denominado perfil de mercado (*Market Profile*), mostra durante quanto tempo o preço foi negociado em cada nível de uma sessão, empilhando uma letra ou um bloco por intervalo de tempo. Revela a área de valor justo da sessão, o ponto de controlo (*point of control*) e os níveis onde o preço permaneceu pouco tempo (*single prints*).

## Demonstração em direto

```chart-demo tpo
```

## Configuração

Adicione uma `TpoSeries` e forneça-lhe barras OHLC, cada uma com um `sessionId`; a série constrói de forma autónoma a distribuição de letras ou blocos por sessão:

```js
const series = chart.addSeries(SSChart.TpoSeries, {
  displayMode: SSChart.TpoDisplayMode.Auto,  // modo de exibição: Auto | Letters | Blocks
  showPoc: true,
  showValueArea: true,
  showInitialBalance: true,
  showSinglePrints: true,
});

series.setData(tpoBars);  // { time, open, high, low, close, sessionId }

chart.timeScale().fitContent();
```

`displayMode` alterna entre letras e blocos sólidos (`Auto` escolhe em função do zoom). As sobreposições — ponto de controlo, área de valor, *initial balance* e *single prints* — podem ser ativadas e desativadas de forma independente.

## Veja também

- [Gráficos em JavaScript](../javascript_charts.md)
- [Perfil de volume](volume_profile.md)
- [Footprint](footprint.md)
