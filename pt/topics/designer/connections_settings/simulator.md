# Simulador

O [Designer](../../designer.md) permite executar as estratégias criadas no modo **Simulation**. Para personalizar a **Simulation**, execute as seguintes ações:

1. Ao clicar na seta junto ao botão **Ligar** ![Designer The quick access toolbar 00](../../../images/designer_quick_access_toolbar_00.png), aparece o botão **Definições do emulador**:

![Designer The connection settings 00](../../../images/designer_connection_settings_00.png)

2. Ao clicar no botão **Definições do emulador**, abre-se a janela **Definições do emulador**:

![Designer Properties emulation 00](../../../images/designer_properties_emulation_00.png)

1. **Simulator**

- **Use emulator** - Usar o emulador.
- **Instruments** - Instrumentos.

2. **Definições**

- **Combine on touch** - Durante a emulação, combinar negócios quando o preço do negócio tocar no preço da ordem (ou seja, for igual ao preço da ordem).
- **Market depth (lifetime)** - Tempo máximo durante o qual o livro de ordens permanece no emulador. Se não houver atualização durante esse período, o livro de ordens é apagado. Esta propriedade pode ser usada para remover livros de ordens antigos quando existem lacunas nos dados.
- **Errors percentage** - Valor percentual do erro ao registar novas ordens. O valor pode ir de 0 (sem erros) a 100.
- **Latency** - Valor mínimo do atraso para ordens registadas.
- **Reregistering** - A reinscrição de ordens sob a forma de um único negócio é suportada?
- **Buffering period** - Enviar respostas em lotes num único pacote. São emulados o atraso de rede e o funcionamento em buffer do núcleo da bolsa.
- **Order ID** - Número a partir do qual o emulador irá gerar os identificadores das ordens.
- **Trade ID** - Número a partir do qual o emulador irá gerar os identificadores dos negócios.
- **Transaction** - Número a partir do qual o emulador irá gerar os identificadores das transações de ordens.
- **Spread size** - Tamanho do spread em incrementos de preço. Usado ao determinar o spread para a geração do livro de ordens a partir de tick trades.
- **Depth of book** - Profundidade máxima do livro de ordens que será gerado a partir dos ticks.
- **Number of volume steps** - Número de passos de volume pelos quais a ordem é maior do que o tick trade. Usado ao testar tick trades.
- **Portfolios interval** - Intervalo de recálculo dos portefólios. Se o intervalo for igual a zero, não é efetuado recálculo.
- **Change time** - Alterar a hora das ordens e negócios para a hora da bolsa.
- **Time zone** - Informação sobre o fuso horário da bolsa.
- **Price shift** - Desvio de preço face ao último negócio, que determina os limites dos preços máximo e mínimo para a sessão seguinte.
- **Add extra volume** - Adicionar volume extra à ordem no livro ao registar ordens de grande volume.

## Conteúdo recomendado

[Gráfico](../user_interface/components/chart.md)
