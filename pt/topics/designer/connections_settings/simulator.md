# Simulador

O [Designer](../../designer.md) permite executar as estratégias criadas no modo **Simulação**. Para personalizar a **Simulação**, execute as seguintes ações:

1. Ao clicar na seta junto ao botão **Ligar** ![Designer Barra de ferramentas de acesso rápido 00](../../../images/designer_quick_access_toolbar_00.png), aparece o botão **Definições do emulador**:

![Designer Definições de ligação 00](../../../images/designer_connection_settings_00.png)

2. Ao clicar no botão **Definições do emulador**, abre-se a janela **Definições do emulador**:

![Designer Propriedades de emulação 00](../../../images/designer_properties_emulation_00.png)

1. **Simulador**

- **Usar emulador** - Usar o emulador.
- **Instrumentos** - Instrumentos.

2. **Definições**

- **Combinar ao tocar** - Durante a emulação, combinar negócios quando o preço do negócio tocar no preço da ordem (ou seja, for igual ao preço da ordem).
- **Profundidade de mercado (tempo de vida)** - Tempo máximo durante o qual o livro de ordens permanece no emulador. Se não houver atualização durante esse período, o livro de ordens é apagado. Esta propriedade pode ser usada para remover livros de ordens antigos quando existem lacunas nos dados.
- **Percentagem de erros** - Valor percentual do erro ao registar novas ordens. O valor pode ir de 0 (sem erros) a 100.
- **Latência** - Valor mínimo do atraso para ordens registadas.
- **Novo registo** - A reinscrição de ordens sob a forma de um único negócio é suportada?
- **Período de buffering** - Enviar respostas em lotes num único pacote. São emulados o atraso de rede e o funcionamento em buffer do núcleo da bolsa.
- **ID da ordem** - Número a partir do qual o emulador irá gerar os identificadores das ordens.
- **ID do negócio** - Número a partir do qual o emulador irá gerar os identificadores dos negócios.
- **Transação** - Número a partir do qual o emulador irá gerar os identificadores das transações de ordens.
- **Tamanho do spread** - Tamanho do spread em incrementos de preço. Usado ao determinar o spread para a geração do livro de ordens a partir de negócios por tick.
- **Profundidade do livro** - Profundidade máxima do livro de ordens que será gerado a partir dos ticks.
- **Número de passos de volume** - Número de passos de volume pelos quais a ordem é maior do que o negócio por tick. Usado ao testar negócios por tick.
- **Intervalo dos portefólios** - Intervalo de recálculo dos portefólios. Se o intervalo for igual a zero, não é efetuado recálculo.
- **Alterar hora** - Alterar a hora das ordens e negócios para a hora da bolsa.
- **Fuso horário** - Informação sobre o fuso horário da bolsa.
- **Desvio de preço** - Desvio de preço face ao último negócio, que determina os limites dos preços máximo e mínimo para a sessão seguinte.
- **Adicionar volume extra** - Adicionar volume extra à ordem no livro ao registar ordens de grande volume.

## Conteúdo recomendado

[Gráfico](../user_interface/components/chart.md)
