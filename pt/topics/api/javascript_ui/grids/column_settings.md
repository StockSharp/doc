# ColumnSettings

`ColumnSettings` acrescenta a seleção, a reorganização e a ocultação de colunas a uma tabela HTML já criada pelo servidor ou por outro componente. Ao contrário de [DataGrid](data_grid.md), o adaptador não constrói o cabeçalho nem as linhas e não gere os dados da tabela.

## Requisitos da marcação

A tabela deve conter um verdadeiro `<thead>`. As colunas geridas recebem atributos `data-col` únicos:

```html
<table id="negocios">
  <thead>
    <tr>
      <th data-col="time">Hora</th>
      <th data-col="symbol">Instrumento</th>
      <th data-col="price">Preço</th>
      <th data-col="volume">Volume</th>
      <th>Ações</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>10:15:02</td>
      <td>SBER</td>
      <td>312,45</td>
      <td>10</td>
      <td><button type="button">Abrir</button></td>
    </tr>
  </tbody>
</table>
```

Uma coluna sem `data-col` é considerada fixa: o utilizador não pode ocultá-la nem deslocá-la da posição original. Ao ser criado, o adaptador marca as células correspondentes do corpo com as mesmas chaves. As linhas com um número diferente de células, por exemplo uma linha «Sem dados» com `colspan`, não são alteradas.

## Ligação

A aplicação anfitriã fornece três elementos:

- uma tabela com marcação gerada pelo servidor;
- uma caixa de diálogo na qual o componente adiciona interruptores e botões de deslocação;
- um armazenamento com os métodos `read()` e `write()`.

```ts
import { ColumnSettings } from '@stocksharp/grids/column-settings';

const dialogElement = document.querySelector<HTMLElement>('#column-dialog')!;
const list = dialogElement.querySelector<HTMLElement>('#column-list')!;

const settings = new ColumnSettings({
  table: document.querySelector<HTMLTableElement>('#negocios')!,
  dialog: {
    list,
    moveUpTitle: 'Mover para cima',
    moveDownTitle: 'Mover para baixo',
    classes: {
      item: 'column-picker-item',
      toggle: 'column-picker-toggle',
      label: 'column-picker-label',
      move: 'column-picker-move',
      moveUpIcon: 'icon-arrow-up',
      moveDownIcon: 'icon-arrow-down',
    },
    open: () => { dialogElement.hidden = false; },
    close: () => { dialogElement.hidden = true; },
  },
  store: {
    read: () => {
      const value = localStorage.getItem('colunas-negocios');
      return value ? JSON.parse(value) : null;
    },
    write: visible => {
      if (visible === null)
        localStorage.removeItem('colunas-negocios');
      else
        localStorage.setItem('colunas-negocios', JSON.stringify(visible));
    },
  },
});

document.querySelector('#open-columns')!
  .addEventListener('click', () => settings.openPicker());

document.querySelector('#apply-columns')!
  .addEventListener('click', () => settings.applyPicked());

document.querySelector('#reset-columns')!
  .addEventListener('click', () => settings.resetToDefault());
```

O componente preenche apenas o elemento `list`. O título, os botões de confirmação e reposição, a animação e a abertura e o fecho da janela modal pertencem à aplicação, pelo que os processadores `applyPicked()` e `resetToDefault()` também devem ser atribuídos na aplicação.

## Armazenamento da disposição

`ColumnLayoutStore.read()` devolve uma matriz de chaves visíveis na ordem pretendida ou `null` quando é utilizada a disposição original. O construtor lê imediatamente o valor e aplica-o antes da primeira interação do utilizador.

`write(visibleKeys)` recebe apenas as colunas geridas que estão visíveis. O valor `null` significa que foi selecionada a ordem original e que nenhuma coluna está oculta. Assim, o armazenamento no URL ou em `localStorage` pode eliminar uma entrada desnecessária em vez de guardar todo o valor predefinido.

O adaptador trabalha com chaves **em minúsculas**: é assim que lê `data-col` ao analisar a tabela e são essas as chaves que entrega para fora — em `defaultKeys()` e em `write()`.

> [!CAUTION]
> As chaves que chegam em `apply()` e as que vêm de `read()` são confrontadas com essa lista **tal como estão, sem conversão de maiúsculas e minúsculas**. A chave `Time` não coincide com a chave detetada `time` e é descartada como desconhecida; e se todas as chaves estiverem escritas assim, não sobra uma única coluna visível — a tabela abre sem nenhuma coluna gerida e sem qualquer erro na consola. Guarde no armazenamento aquilo que chegou em `write()` e não reconstrua as chaves por sua conta.

As chaves desconhecidas ou repetidas são ignoradas durante a aplicação. Uma matriz vazia é uma disposição legítima — «não mostrar nenhuma coluna gerida» — e não «nenhuma disposição selecionada»; isso último indica-se apenas com `null`.

## Métodos

- `defaultKeys()` devolve a ordem original das colunas geridas;
- `apply(visibleKeys)` reorganiza e oculta imediatamente as colunas, mas não guarda a disposição;
- `isDefault(visibleKeys)` verifica se a disposição coincide com a original;
- `openPicker()` lê o DOM atual, constrói a lista e abre a caixa de diálogo;
- `applyPicked()` aplica a seleção atual, guarda-a e fecha a caixa de diálogo;
- `resetToDefault()` repõe todas as colunas geridas, chama `write(null)` e fecha a caixa de diálogo.

## Estilo

`ColumnSettings` não inclui CSS e não depende de uma biblioteca modal nem de um conjunto de ícones específico. Através de `ColumnPickerClasses`, a aplicação define as classes da linha, caixa de seleção, legenda, botões e dois ícones. `moveUpTitle` e `moveDownTitle` devem ser localizados antes de serem fornecidos ao componente.

## Consulte também

- [Tabelas JavaScript](../grids.md)
- [DataGrid](data_grid.md)
- [Repositório JS-Grids](https://github.com/StockSharp/JS-Grids)
