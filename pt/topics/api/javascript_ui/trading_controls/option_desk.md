# Mesa de opções

`OptionDeskWidget` apresenta uma série da cadeia de opções: à esquerda a call, à direita a put em espelho e, entre elas, o strike e o valor intrínseco. O volume, o interesse em aberto e as volatilidades são apresentados não só como número, mas também como barra, pelo que a cadeia se lê pela forma e não apenas pelos valores.

![Mesa de opções com os lados call e put em torno dos strikes](../../../../images/javascript_controls_option_desk.png)

## Criação e atualização

```ts
import {
  OptionDeskWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const desk = OptionDeskWidget.create(
  document.querySelector<HTMLElement>('#option-desk')!,
  {},
  { host },
);

desk.update(
  [{
    strike: 68_000,
    call: {
      symbol: 'BTC-68000-C',
      bid: 1_240,
      ask: 1_265,
      last: 1_250,
      theoretical: 1_248,
      volume: 320,
      openInterest: 1_480,
      ivBid: 0.42,
      ivAsk: 0.44,
      ivLast: 0.43,
      historicalVolatility: 0.39,
    },
    put: {
      symbol: 'BTC-68000-P',
      bid: 820,
      ask: 845,
      volume: 210,
      openInterest: 960,
      ivLast: 0.47,
    },
  }],
  {
    assetPrice: 68_420,
    timeToExpiry: 0.08,
    riskFree: 0.05,
    dividend: 0,
  },
);
```

A única dependência é `host`; o controlo não tem dependências opcionais. O segundo argumento de `create` é o estado do painel, que o controlo não utiliza.

`update(strikes, context)` substitui toda a cadeia. Os dois argumentos são passados em conjunto: a cadeia e o preço do ativo subjacente são uma única observação, e uma atualização em separado mostraria gregas calculadas a partir de um preço já deslocado. `context` é opcional e por predefinição está vazio.

## Contexto da cadeia

`OptionChainContext` descreve aquilo em relação a que a série é avaliada: `assetPrice` é o preço do ativo subjacente, `timeToExpiry` é o tempo até à expiração em anos, `riskFree` e `dividend` são taxas em fração (`0.05` são cinco por cento). Sem `assetPrice` a mesa continua a mostrar as cotações, mas o valor intrínseco é zero e as linhas não se dividem entre «dentro do dinheiro» e «fora do dinheiro». Sem `assetPrice` ou sem `timeToExpiry` as gregas não são calculadas e a célula fica vazia, em vez de ficar a zero.

## Gregas

As gregas chegam por uma de duas vias. Se o anfitrião as calcula, envia um objeto `greeks` já pronto no lado do strike e a mesa mostra o que recebeu. Se o anfitrião envia a volatilidade, as gregas são calculadas no momento por Black-Scholes a partir do primeiro valor disponível pela ordem `ivLast`, `ivBid`, `ivAsk`, `historicalVolatility`. Nenhuma das vias é alternativa de recurso da outra — são duas formas de o anfitrião fornecer os dados.

O número de casas decimais é escolhido a partir dos dados: quatro algarismos significativos para o menor valor da coluna, mas nunca menos de duas nem mais de oito casas. A contagem é única para toda a coluna e comum aos dois lados, pelo que o delta não se transforma em `0.0000` e a gama e a sua coluna em espelho são escritas da mesma forma.

## Colunas e apresentação

A ordem das colunas parte do strike para fora: as volatilidades e as cotações ficam mais perto do meio e as gregas nas extremidades; o lado da put repete o mesmo pela ordem inversa. A ordenação é por strike ascendente: a cadeia lê-se como uma escada.

Por predefinição estão ocultas as colunas `callRho`, `callTheta`, `callHv`, `callTheor` e as suas espelhadas `putRho`, `putTheta`, `putHv`, `putTheor` — o menu de contexto da tabela devolve-as.

As barras são escaladas de formas diferentes. O volume e o interesse em aberto são escalados separadamente em cada lado, porque as calls e as puts negoceiam em tamanhos diferentes. As volatilidades usam uma única escala para ambos os lados, caso contrário o desvio entre os lados desapareceria. A barra é desenhada com a largura do elemento, sem `canvas`.

À linha é atribuída a classe `option-itm-call` nos strikes abaixo do preço do ativo e `option-itm-put` nos restantes; na ausência de `assetPrice` fica apenas `option-row`. As volatilidades são apresentadas em percentagem com duas casas decimais e os preços com o formato de preço geral do pacote.

A mesa não guarda definições: não faz acessos a `host.preferences` nem a `host.cache`, e o conjunto de colunas e a ordenação vivem na instância atual.

## O que faz o anfitrião

Todo o texto visível vem de `host.t` — o título do painel, os títulos das colunas, a mensagem de tabela vazia e as entradas do menu de contexto. O botão de fecho invoca `host.close()`: o painel não se remove a si próprio. Ao ser criado, o controlo chama `host.register(this)` e, em `dispose()`, `host.unregister(this)`. Um botão na barra lateral exporta a cadeia para XLSX.

O controlo não subscreve dados nem envia ordens: a cadeia e o contexto são-lhe entregues pelo anfitrião através do método `update`.

## Métodos públicos

- `OptionDeskWidget.create(hostEl, state, deps)` — construir o painel e adicioná-lo ao contentor.
- `update(strikes, context)` — substituir a cadeia e o contexto de avaliação.
- `rows()` — devolver as linhas tal como a mesa as guarda: com as escalas das barras e o valor intrínseco já calculados.
- `dispose()` — libertar os recursos e cancelar o registo no anfitrião.
- `OptionDeskWidget.TYPE` — identificador do tipo de controlo, `ControlTypes.OptionDesk`.

## Funções exportadas

A parte de cálculo está disponível separadamente do painel:

- `scaleChain(strikes, context)` — numa única passagem pela cadeia, calcula os máximos das barras e o valor intrínseco de cada strike.
- `sideGreeks(row, which, context)` — as gregas de um lado do strike: as fornecidas pelo anfitrião ou as calculadas a partir da sua volatilidade; `null` quando nenhuma das duas é possível.
- `greekPlaces(values)` — o número de casas decimais para uma coluna de valores.
- `greekScales(rows, context)` — o número de casas para cada grega, medido de uma só vez sobre os dois lados da cadeia.

## Consulte também

- [Controlos de negociação JavaScript](../trading_controls.md)
- [Lista de instrumentos](watchlist.md)
- [Livro de ofertas](order_book.md)
- [Posições](positions.md)
