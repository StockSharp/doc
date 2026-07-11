# Definições do Hydra

A seguir descreve-se como criar e configurar uma tarefa de cópia de segurança.

1. Para criar uma tarefa, clique no botão **Adicionar tarefas...**; na janela que se abre, selecione o item **Cópia de segurança** e clique no botão **OK**.![hydra tasks backup add](../../../../images/hydra_tasks_backup_add.png)
2. Em seguida, é necessário configurar a tarefa.![hydra tasks backup](../../../../images/hydra_tasks_backup.png)

   **Cópia de segurança**
   - **Serviço** - o endereço do serviço.
   - **Endereço** - o endereço da região. O endereço da região especificado nas definições do bucket. Consulte [Regiões e endpoints](https://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region).
   - **Armazenamento** - o nome do bucket.
   - **Início de sessão** - início de sessão. **Access Key ID**.
   - **Palavra-passe** - palavra-passe. **Secret Access Key**.
   - **Data inicial** - a partir de que data iniciar a cópia de segurança.
   - **Desfasamento temporal** - um desvio em dias a partir da data atual.

   **Geral**
   - **Cabeçalho** - título da tarefa.
   - **Horário de trabalho** - configuração do horário de funcionamento da board. ![Hydra tarefas de cópia de segurança](../../../../images/hydra_tasks_backup_desk.png)
   - **Intervalo de operação** - o intervalo de operação.
   - **Diretório de dados** - diretório de dados, a partir do qual serão recebidos os dados para conversão.
   - **Formato** - o formato dos dados convertidos: BIN\/CSV.
   - **Máx. erros** - o número máximo de erros após o qual a tarefa será parada. Por predefinição, 0 - o número de erros é ignorado.
   - **Dependência** - uma tarefa que deve ser executada antes de iniciar a atual.

   **Registo**
   - **Identificador** - o identificador.
   - **Nível de registo** - o nível de registo.
3. Depois de configurar a tarefa, adicione os instrumentos que devem ser guardados no armazenamento de cópia de segurança e clique no botão **Iniciar**.

## Conteúdo recomendado

[Criar e configurar uma conta](setup.md)
