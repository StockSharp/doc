# Definições de backtesting

O painel **Properties** está minimizado por predefinição no lado direito do separador da estratégia. Este painel é uma tabela de propriedades de emulação ou de negociação em tempo real. Quando selecciona uma propriedade específica, aparece uma descrição detalhada dessa propriedade na parte inferior da tabela. Todas as propriedades estão agrupadas em grupos:

![Designer Properties emulation 00](../../../../images/designer_properties_emulation_00.png)

**Definições**

- **Market data** - o armazenamento de dados.
- **Storage format** - o formato de armazenamento.
- **Data type** - o tipo de dados.
- **Time frame** - para utilizar velas com o período especificado.
- **Maximum quote volume in generated depth** - o volume máximo de cotações no livro de ordens gerado.
- **Interval** - o intervalo de tempo.
- **Unrealized P\/L** - o intervalo de recálculo do lucro não realizado.
- **Trades** - que negociações utilizar.
- **Marked depth** - que livros de ordens utilizar.
- **Order log** - para utilizar o registo de ordens.
- **Number of strategies** - o número de estratégias testadas em simultâneo.
- **Logging level** - o nível de registo.
- **Combine on touch** - durante a emulação, combinar ordens quando o preço da negociação toca no preço da ordem (ou seja, é igual ao preço da ordem).
- **Marked depth (lifetime)** - o tempo máximo durante o qual o livro de ordens permanece no emulador. Se durante este tempo não houver actualização, o livro de ordens é eliminado. Esta propriedade pode ser utilizada para remover livros de ordens antigos quando existem falhas nos dados.
- **Errors percentage** - o valor percentual do erro no registo de novas ordens. O valor pode ir de 0 (não haverá erros) a 100.
- **Latency** - o valor mínimo da latência da ordem registada.
- **Reregistering** - se o novo registo de ordens é suportado como uma única negociação.
- **Buffering period** - para enviar respostas num único pacote em intervalos. São emuladas a latência de rede e a operação em buffer do núcleo da bolsa.
- **Order ID** - o número a partir do qual o emulador irá gerar identificadores para ordens.
- **Trade ID** - o número a partir do qual o emulador irá gerar identificadores para negociações.
- **Transaction** - o número a partir do qual o emulador irá gerar identificadores para transacções de ordens.
- **Spread size** - o tamanho do spread em passos de preço. É utilizado ao especificar o spread para a geração de um livro de ordens a partir de negociações tick.
- **Depth of book** - a profundidade máxima do livro de ordens que será gerado a partir dos ticks.
- **Number of volume steps** - o número de passos de volume em que a ordem é maior do que a negociação tick. É utilizado para testes em negociações tick.
- **Portfolios interval** - o intervalo para recálculo da carteira. Se o intervalo for zero, não é efectuado qualquer recálculo.
- **Change time** - para alterar a hora de ordens e negociações para a hora da bolsa.
- **Time zone** - informação sobre o fuso horário onde a bolsa se encontra.
- **Price shift** - o deslocamento de preço face à última negociação, especificando os limites dos preços máximo e mínimo para a sessão seguinte.
- **Add extra volume** - adicionar volume extra ao livro de ordens quando estão a ser registadas ordens com grande volume.
- **[Commissions](../commissions.md)** - a comissão (corretagem, bolsa, etc.).

**Registo**

- **Logging level** - o nível de registo deste elemento.

**Configuração**

- **[Risk management](../risk_management.md)** - as definições de gestão de risco.

**Parâmetros do diagrama**

- **Security** - o instrumento.
- **Portfolio** - a carteira.

Se não preencher os **Diagram parameters**, ao emular, o instrumento será utilizado a partir do campo **Instrument** do separador **Emulation**, e como carteira será utilizada por predefinição a carteira de teste.

## Conteúdo recomendado

[Gráfico](chart.md)
