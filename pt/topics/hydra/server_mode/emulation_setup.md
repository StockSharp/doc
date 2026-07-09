# Configuração da emulação

No modo de servidor, o programa permite ativar o modo de emulação.

![hydra emulator start](../../../images/hydra_emulator_start.png)

No modo de emulação, o programa [Hydra](../../hydra.md) permite executar as seguintes funções:

- O programa permite configurar as chaves para ligar à fonte e trabalhar simultaneamente com uma ligação em diferentes programas ([Designer](../../designer.md), [Terminal](../../terminal.md)).
- Se a fonte de dados de mercado permitir descarregar dados históricos, estes podem ser utilizados simultaneamente para testes.
- Se a fonte permitir receber dados em tempo real, então o modo de emulação permite emular o modo de negociação. Neste modo, os dados sobre ações do utilizador (registo de ordens, transações) são transferidos diretamente para o Hydra, enquanto as ações são registadas separadamente para cada programa. Por exemplo, ao registar uma ordem no Terminal, as alterações nela serão visíveis apenas para ele e, no Designer, não serão registadas. Isto evita conflitos entre dois programas executados sobre a mesma ligação.
- IMPORTANTE\! As transações realizadas em modo de emulação, a negociação e as operações sobre elas são emuladas em tempo real; quando o modo está desligado, as ações serão executadas na negociação real.

Este modo é utilizado ao [testar estratégias](../../shell/user_interface/emulation.md).

## Definições de emulação.

![hydra emulator prop](../../../images/hydra_emulator_prop.png)

- **Match on touch** - ao emular o matching de transações, faz corresponder ordens quando o preço da transação é igual ao preço da ordem.
- **Order book (time in force)** - o período máximo do livro de ordens no emulador. Se o livro de ordens não tiver sido atualizado dentro do período especificado, o seu valor é apagado. É utilizado para remover dados antigos do livro de ordens se existirem lacunas nos dados.
- **Percentage of errors** - a percentagem de erros no registo de novas ordens (de 0 a 100).
- **Latency** - a latência mínima das ordens registadas.
- **Re-registration** - se o novo registo de ordens será suportado como uma única transação.
- **Buffering period** - um parâmetro responsável pelo período de envio de pacotes completos para emular a latência de rede e fazer buffering do trabalho do núcleo da bolsa.
- **Order ID** - o número com o qual o emulador irá gerar identificadores para ordens.
- **Trade identifier** - o número com o qual o emulador irá gerar identificadores para transações.
- **Transaction** - o número com o qual o emulador irá gerar identificadores para transações de ordens.
- **Spread size** - o tamanho do spread em passos de preço. É utilizado para determinar o spread ao gerar o livro de ordens a partir de transações tick.
- **Order book depth** - profundidade máxima do livro de ordens gerado por ticks
- **Number of volume steps** - o número de passos de volume pelos quais a ordem é maior do que a transação tick. É utilizado em testes com transações tick.
- **Portfolio interval** - intervalo para recalcular dados sobre carteiras. Se o intervalo for 0, não é efetuado qualquer recálculo.
- **Adjust time** - ajustar a hora de ordens e transações para a hora da bolsa.
- **Fuso horário** - informações sobre o fuso horário onde a bolsa está localizada
- **Price shift** - um deslocamento de preço a partir da última transação, que determina os limites dos preços máximo e mínimo para a sessão seguinte
- **Add additional volume** - adicionar volume adicional ao livro de ordens ao registar ordens com grande volume.
- **Trading session state** - verificação do estado de negociação.
- **Money** - verificar o saldo monetário
- **Short** - a capacidade de abrir posições curtas.
- **Storage** - armazenamento.
