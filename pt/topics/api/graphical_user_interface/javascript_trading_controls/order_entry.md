# Introdução de ordens

`OrderEntryWidget` é um painel bilateral de compra e venda. Suporta ordens de mercado, limite, stop e stop-limit, apresenta apenas os campos relativos ao tipo selecionado e valida os valores antes de os fornecer ao anfitrião.

## Criação e configuração do instrumento

```ts
import {
  OrderEntrySides,
  OrderEntryTypes,
  OrderEntryWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const pad = OrderEntryWidget.create(
  document.querySelector<HTMLElement>('#order-entry')!,
  {},
  {
    host,
    submitOrder: (side, values) =>
      console.log('enviar', side, values),
  },
);

pad.setInstrument({
  symbol: 'BTC@IMEX',
  lotSize: 0.001,
  tickSize: 0.1,
  minVolume: 0.001,
  maxVolume: 5,
});

pad.setOrderType(OrderEntryTypes.Limit);
pad.setBbo(68_420.4, 68_420.5);
pad.setAvailable(OrderEntrySides.Buy, 48_251);
pad.setMaxQuantity(OrderEntrySides.Buy, 0.7);
pad.setQuantity(0.01);
```

`OrderEntryTypes` contém `Market`, `Limit`, `Stop` e `StopLimit`, enquanto `OrderEntrySides` contém `Buy` e `Sell`.

## Validação e envio

O controlo valida os valores positivos, o tamanho do lote, o incremento do preço e o volume mínimo e máximo. Os valores opcionais de Take Profit e Stop Loss são ativados através do método `toggleTpSl`.

`submit(side)` começa por chamar `validate(side)`. Quando os dados são válidos, o processador `submitOrder` recebe a direção e o objeto:

```ts
interface OrderEntryValues {
  type: 'market' | 'limit' | 'stop' | 'stoplimit';
  quantity: number;
  limitPrice: number | null;
  stopPrice: number | null;
  takeProfit: number | null;
  stopLoss: number | null;
}
```

O próprio controlo não seleciona a carteira, não verifica a ligação nem envia a ordem para o servidor. Estas ações permanecem no processador do anfitrião.

## Preços, volume e estado

- `setBbo(bid, ask)` atualiza o melhor preço de compra e de venda.
- `applyBbo(side)` coloca na coluna selecionada o lado oposto do BBO para execução imediata.
- `setAvailable(side, value)` define o saldo disponível apresentado.
- `setMaxQuantity(side, value)` define o máximo a partir do qual os botões de percentagem calculam a quantidade.
- `applyPercent(side, pct)` aplica 25, 50, 75 ou 100 por cento da quantidade máxima definida através de `setMaxQuantity(side, value)`.
- `preselect(side)` seleciona a direção depois do gesto «negociar com um clique».
- `setEnabled(false)` impede o envio sem eliminar os valores introduzidos.
- `getValues(side)` e `validate(side)` permitem validar externamente o formulário.

O método estático `toApiType` converte `Limit` em `0`, `Market` em `1` e `Stop` e `StopLimit` em `2`.

## Consulte também

- [Controlos de negociação JavaScript](../javascript_trading_controls.md)
- [Livro de ofertas](order_book.md)
- [Posições](positions.md)
