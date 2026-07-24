# TPO (Perfil de mercado)

Um gráfico TPO (Time Price Opportunity), também chamado de Perfil de Mercado (Market Profile), mostra por quanto tempo o preço foi negociado em cada nível durante uma sessão, empilhando uma letra ou bloco para cada intervalo de tempo. Ele revela a área de valor justo da sessão, seu point of control e onde o preço passou pouco tempo (single prints).

## Demonstração ao vivo

```chart-demo tpo
```

## Configuração

Adicione uma `TpoSeries` e alimente-a com barras OHLC que carreguem cada uma um `sessionId`; a série constrói por conta própria a distribuição de letras/blocos por sessão:

```js
const series = chart.addSeries(SSChart.TpoSeries, {
  displayMode: SSChart.TpoDisplayMode.Auto,  // Auto | Letters | Blocks
  showPoc: true,
  showValueArea: true,
  showInitialBalance: true,
  showSinglePrints: true,
});

series.setData(tpoBars);  // { time, open, high, low, close, sessionId }

chart.timeScale().fitContent();
```

`displayMode` alterna entre letras e blocos sólidos (`Auto` escolhe conforme o zoom). Os overlays — point of control, área de valor, initial balance e single prints — podem ser ativados e desativados de forma independente.

## Veja também

- [Gráficos JavaScript](../javascript_charts.md)
- [Perfil de volume](volume_profile.md)
- [Footprint](footprint.md)
