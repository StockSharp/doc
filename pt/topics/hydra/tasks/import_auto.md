# Importação (automática)

A tarefa executa a importação automática de dados da bolsa a partir de ficheiros no diretório especificado, de acordo com a máscara de ficheiro especificada.

Para cada tipo de dados de mercado selecionado, o modelo é configurado no separador [Importação](../importing.md).

![Captura de ecrã de Importação (automática) 1](../../../images/hydra_tasks_import.png)

Na parte inferior do painel, pode selecionar os instrumentos pelos quais os dados serão importados, bem como o tipo de dados a importar.

Para cada instrumento, pode especificar as seguintes propriedades de importação de dados:

![Captura de ecrã de Importação (automática) 2](../../../images/hydra_tasks_proper_import.png)

**Importação (automática)**

**Definições**

- **Tipo de dados** - tipo dos dados importados.
- **Nome do ficheiro** - caminho completo para o ficheiro.
- **Diretório de dados** - diretório de dados.
- **Máscara de ficheiro** - máscara de ficheiro utilizada ao analisar o diretório. Por exemplo, candles\*.csv.
- **Subdiretórios** - incluir subdiretórios.
- **Separador de colunas** - separador de colunas. A tabulação é indicada por TAB.
- **Recuo desde o início** - o número de linhas a ignorar desde o início do ficheiro (se contiverem metainformação).
- **Fuso horário** - fuso horário.
- **Intervalo** - a frequência de atualização dos dados.
- **Informação alargada** - guardar campos importados alargados no armazenamento de informação alargada.
- **Duplicados** - se instrumentos duplicados devem ser atualizados caso já existam.
- **Ignorar sem ID** - ignorar instrumentos sem identificador.

**Geral**

- **Cabeçalho** - título da tarefa.
- **Horário de trabalho** - configuração do horário de funcionamento do mercado. ![Hydra tarefas de cópia de segurança](../../../images/hydra_tasks_backup_desk.png)
- **Intervalo de operação** - o intervalo de funcionamento.
- **Diretório de dados** - diretório de dados, de onde serão recebidos os dados para conversão.
- **Formato** - o formato dos dados convertidos: BIN\/CSV.
- **Máx. erros** - o número máximo de erros; ao atingi-lo, a tarefa será parada. Por predefinição, 0 - o número de erros é ignorado.
- **Dependência** - uma tarefa que deve ser executada antes de iniciar a atual.

**Registo**

- **Identificador** - o identificador.
- **Nível de registo** - o nível de registo.
