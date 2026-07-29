# Configuração da emulação

No modo de servidor, o programa permite ativar o modo de emulação.

![Captura de ecrã de Configuração da emulação 1](../../../images/hydra_emulator_start.png)

No modo de emulação, o programa [Hydra](../../hydra.md) permite executar as seguintes funções:

- O programa permite configurar as chaves para ligar à fonte e trabalhar simultaneamente com uma ligação em diferentes programas ([Designer](../../designer.md), [Terminal](../../terminal.md)).
- Se a fonte de dados de mercado permitir descarregar dados históricos, estes podem ser utilizados simultaneamente para testes.
- Se a fonte permitir receber dados em tempo real, então o modo de emulação permite emular o modo de negociação. Neste modo, os dados sobre ações do utilizador (registo de ordens, transações) são transferidos diretamente para o Hydra, enquanto as ações são registadas separadamente para cada programa. Por exemplo, ao registar uma ordem no Terminal, as alterações nela serão visíveis apenas para ele e, no Designer, não serão registadas. Isto evita conflitos entre dois programas executados sobre a mesma ligação.
- IMPORTANTE\! As transações realizadas em modo de emulação, a negociação e as operações sobre elas são emuladas em tempo real; quando o modo está desligado, as ações serão executadas na negociação real.

Este modo é utilizado ao [testar estratégias](../../shell/user_interface/emulation.md).

## Definições de emulação.

![Captura de ecrã de Configuração da emulação 2](../../../images/hydra_emulator_prop.png)

- **Correspondência no toque** - ao emular o matching de transações, faz corresponder ordens quando o preço da transação é igual ao preço da ordem.
- **Livro de ordens (validade)** - o período máximo do livro de ordens no emulador. Se o livro de ordens não tiver sido atualizado dentro do período especificado, o seu valor é apagado. É utilizado para remover dados antigos do livro de ordens se existirem lacunas nos dados.
- **Percentagem de erros** - a percentagem de erros no registo de novas ordens (de 0 a 100).
- **Latência** - a latência mínima das ordens registadas.
- **Novo registo** - se o novo registo de ordens será suportado como uma única transação.
- **Período de buffer** - um parâmetro responsável pelo período de envio de pacotes completos para emular a latência de rede e fazer buffering do trabalho do núcleo da bolsa.
- **ID da ordem** - o número com o qual o emulador irá gerar identificadores para ordens.
- **Identificador da transação** - o número com o qual o emulador irá gerar identificadores para transações.
- **Transação** - o número com o qual o emulador irá gerar identificadores para transações de ordens.
- **Tamanho do spread** - o tamanho do spread em passos de preço. É utilizado para determinar o spread ao gerar o livro de ordens a partir de transações tick.
- **Profundidade do livro de ordens** - profundidade máxima do livro de ordens gerado por ticks
- **Número de passos de volume** - o número de passos de volume pelos quais a ordem é maior do que a transação tick. É utilizado em testes com transações tick.
- **Intervalo da carteira** - intervalo para recalcular dados sobre carteiras. Se o intervalo for 0, não é efetuado qualquer recálculo.
- **Ajustar hora** - ajustar a hora de ordens e transações para a hora da bolsa.
- **Fuso horário** - informações sobre o fuso horário onde a bolsa está localizada
- **Deslocamento de preço** - um deslocamento de preço a partir da última transação, que determina os limites dos preços máximo e mínimo para a sessão seguinte
- **Adicionar volume adicional** - adicionar volume adicional ao livro de ordens ao registar ordens com grande volume.
- **Estado da sessão de negociação** - verificação do estado de negociação.
- **Dinheiro** - verificar o saldo monetário
- **Posições curtas** - a capacidade de abrir posições curtas.
- **Armazenamento** - armazenamento.
