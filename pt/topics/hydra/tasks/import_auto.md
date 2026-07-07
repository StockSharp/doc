# Import (auto)

A tarefa executa a importação automática de dados da bolsa a partir de ficheiros no diretório especificado, de acordo com a máscara de ficheiro especificada.

Para cada tipo de dados de mercado selecionado, o modelo é configurado no separador [Importação](../importing.md).

![hydra tasks import](../../../images/hydra_tasks_import.png)

Na parte inferior do painel, pode selecionar os instrumentos pelos quais os dados serão importados, bem como o tipo de dados a importar.

Para cada instrumento, pode especificar as seguintes propriedades de importação de dados:

![hydra tasks proper import](../../../images/hydra_tasks_proper_import.png)

**Import (auto)**

**Settings**

- **Data type** - tipo dos dados importados. 
- **Filename** - caminho completo para o ficheiro. 
- **Data directory** - diretório de dados. 
- **File mask** - máscara de ficheiro utilizada ao analisar o diretório. Por exemplo, candles\*.csv. 
- **Subdirectories** - incluir subdiretórios. 
- **Column separator** - separador de colunas. A tabulação é indicada por TAB. 
- **Indent from the beginning** - o número de linhas a ignorar desde o início do ficheiro (se contiverem metainformação). 
- **Time zone** - fuso horário. 
- **Interval** - a frequência de atualização dos dados. 
- **Extended information** - guardar campos importados alargados no armazenamento de informação alargada.
- **Duplicates** - se instrumentos duplicados devem ser atualizados caso já existam. 
- **Ignore without ID** - ignorar instrumentos sem identificador. 

**General**

- **Header** - Converter. 
- **Working hours** - configuração do horário de funcionamento da board. ![hydra tasks backup desk](../../../images/hydra_tasks_backup_desk.png)
- **Interval of operation** - o intervalo de funcionamento. 
- **Data directory** - diretório de dados, de onde serão recebidos os dados para conversão. 
- **Format** - o formato dos dados convertidos: BIN\/CSV. 
- **Max. errors** - o número máximo de erros; ao atingi-lo, a tarefa será parada. Por predefinição, 0 - o número de erros é ignorado. 
- **Dependency** - uma tarefa que deve ser executada antes de iniciar a atual. 

**Logging**

- **Identifier** - o identificador. 
- **Logging level** - o nível de registo. 
