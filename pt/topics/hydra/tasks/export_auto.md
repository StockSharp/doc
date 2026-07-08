# Exportação (automática)

A tarefa exporta dados da bolsa para vários formatos: Excel, xml, sql, bin, Json ou txt.

![hydra tasks export](../../../images/hydra_tasks_export.png)

**Database**

- **Connection** - ligação à base de dados. Utilizado no caso de exportação via SQL. 
- **Packet** - o tamanho do pacote de dados transmitido. Por predefinição, o tamanho é de 50 elementos. Utilizado no caso de exportação via SQL. 
- **Uniqueness** - verificar a unicidade dos dados na base de dados. Afeta o desempenho. Ativado por predefinição. Utilizado no caso de exportação via SQL. 

> [!TIP]
> Ao utilizar exportação via SQL, é necessário definir os parâmetros da string de ligação

**New connection string**

![hydra tasks connstring](../../../images/hydra_tasks_connstring.png)

- **Provider** - definições do fornecedor. 
- **Server** - endereço do servidor ou caminho para a base de dados. 
- **Database** - nome da base de dados. Não utilizado para SQLite. 
- **Login** - login para aceder à base de dados. Não utilizado para acesso anónimo. 
- **Password** - palavra-passe para aceder à base de dados. Não utilizada para acesso anónimo. 
- **Windows** - utilizar a conta atual do Windows para ligar à base de dados. 
- **Connection** - string de ligação pronta a utilizar. 

> [!TIP]
> Pode verificar a ligação à base de dados usando o botão **Check**.

**General**

- **Header** - Converter. 
- **Working hours** - configuração do horário de funcionamento da board. ![hydra tasks backup desk](../../../images/hydra_tasks_backup_desk.png)
- **Interval of operation** - o intervalo de funcionamento. 
- **Data directory** - diretório de dados, de onde serão recebidos os dados para conversão. 
- **Format** - o formato dos dados convertidos: BIN\/CSV. 
- **Max. errors** - o número máximo de erros; ao atingi-lo, a tarefa será parada. Por predefinição, 0 - o número de erros é ignorado. 
- **Dependency** - uma tarefa que deve ser executada antes de iniciar a atual. 

**CSV**

- **Templates** - modelos para cada tipo de dados exportados. 
- **Header** - o cabeçalho na primeira linha. Se for passada uma string vazia, o cabeçalho não será adicionado ao ficheiro.
- **Name format** - o formato para escrever o nome do ficheiro exportado. 

**Export (auto)**

- **Type** - tipo de exportação (formato). 
- **Start date** - a partir de que data iniciar a exportação de dados. 
- **Time offset** - desfasamento temporal em dias. 
- **Export directory** - diretório para onde os dados serão exportados. 
- **Format** - formato dos dados. 
- **Split** - tipo de divisão. 

**Logging**

- **Identifier** - o identificador. 
- **Logging level** - o nível de registo. 

Consideremos um exemplo de exportação automática:

1. Selecione o instrumento.
2. Configure os dados de mercado que precisam de ser exportados.![hydra tasks export 00](../../../images/hydra_tasks_export_00.png)
3. Defina o período de exportação. Se estiver configurado o descarregamento de dados de mercado em tempo real, pode omitir a data de fim do período. Neste caso, os dados serão exportados em tempo real, de acordo com o intervalo de trabalho (atualização dos dados). ![hydra tasks export 01](../../../images/hydra_tasks_export_01.png)
4. Configuração de diretórios. Intervalo de funcionamento. Tipo de dados. Formato dos dados.
5. Iniciamos a exportação.![hydra tasks export 02](../../../images/hydra_tasks_export_02.png)

Vamos ver os dados exportados

![hydra tasks export 03](../../../images/hydra_tasks_export_03.png)

**Veja o [tutorial em vídeo](../videos/export_task.md)**
