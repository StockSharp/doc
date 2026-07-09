# Definições de backtesting

O painel **Propriedades** está minimizado por predefinição no lado direito do separador da estratégia. Este painel é uma tabela de propriedades de emulação ou de negociação em tempo real. Quando selecciona uma propriedade específica, aparece uma descrição detalhada dessa propriedade na parte inferior da tabela. Todas as propriedades estão agrupadas em grupos:

![Designer Properties emulation 00](../../../../images/designer_properties_emulation_00.png)

**Definições**

- **Dados de mercado** - o armazenamento de dados.
- **Formato de armazenamento** - o formato de armazenamento.
- **Tipo de dados** - o tipo de dados.
- **Período** - para utilizar velas com o período especificado.
- **Volume máximo de cotações na profundidade gerada** - o volume máximo de cotações no livro de ordens gerado.
- **Intervalo** - o intervalo de tempo.
- **P/L não realizado** - o intervalo de recálculo do lucro não realizado.
- **Negócios** - que negociações utilizar.
- **Profundidade de mercado** - que livros de ordens utilizar.
- **Registo de ordens** - para utilizar o registo de ordens.
- **Número de estratégias** - o número de estratégias testadas em simultâneo.
- **Nível de registo** - o nível de registo.
- **Combinar ao tocar** - durante a emulação, combinar ordens quando o preço da negociação toca no preço da ordem (ou seja, é igual ao preço da ordem).
- **Profundidade de mercado (tempo de vida)** - o tempo máximo durante o qual o livro de ordens permanece no emulador. Se durante este tempo não houver actualização, o livro de ordens é eliminado. Esta propriedade pode ser utilizada para remover livros de ordens antigos quando existem falhas nos dados.
- **Percentagem de erros** - o valor percentual do erro no registo de novas ordens. O valor pode ir de 0 (não haverá erros) a 100.
- **Latência** - o valor mínimo da latência da ordem registada.
- **Novo registo** - se o novo registo de ordens é suportado como uma única negociação.
- **Período de buffering** - para enviar respostas num único pacote em intervalos. São emuladas a latência de rede e a operação em buffer do núcleo da bolsa.
- **ID da ordem** - o número a partir do qual o emulador irá gerar identificadores para ordens.
- **ID do negócio** - o número a partir do qual o emulador irá gerar identificadores para negociações.
- **Transação** - o número a partir do qual o emulador irá gerar identificadores para transacções de ordens.
- **Tamanho do spread** - o tamanho do spread em passos de preço. É utilizado ao especificar o spread para a geração de um livro de ordens a partir de negociações tick.
- **Profundidade do livro** - a profundidade máxima do livro de ordens que será gerado a partir dos ticks.
- **Número de passos de volume** - o número de passos de volume em que a ordem é maior do que a negociação tick. É utilizado para testes em negociações tick.
- **Intervalo dos portefólios** - o intervalo para recálculo da carteira. Se o intervalo for zero, não é efectuado qualquer recálculo.
- **Alterar hora** - para alterar a hora de ordens e negociações para a hora da bolsa.
- **Fuso horário** - informação sobre o fuso horário onde a bolsa se encontra.
- **Desvio de preço** - o deslocamento de preço face à última negociação, especificando os limites dos preços máximo e mínimo para a sessão seguinte.
- **Adicionar volume extra** - adicionar volume extra ao livro de ordens quando estão a ser registadas ordens com grande volume.
- **[Comissões](../commissions.md)** - a comissão (corretagem, bolsa, etc.).

**Registo**

- **Nível de registo** - o nível de registo deste elemento.

**Configuração**

- **[Gestão de risco](../risk_management.md)** - as definições de gestão de risco.

**Parâmetros do diagrama**

- **Instrumento** - o instrumento.
- **Carteira** - a carteira.

Se não preencher os **Parâmetros do diagrama**, ao emular, o instrumento será utilizado a partir do campo **Instrumento** do separador **Emulação**, e como carteira será utilizada por predefinição a carteira de teste.

## Conteúdo recomendado

[Gráfico](chart.md)
