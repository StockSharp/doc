# Ligar um Cliente Hydra

No modo de servidor, é possível ligar outro programa Hydra, que atuará como cliente e descarregará dados para si próprio. Ao contrário da [Ligação através do Protocolo FIX](fix_fast_connectivity.md), os dados são transmitidos sob a forma de ficheiros no formato StockSharp. Isto torna a fonte adequada para transferir um grande volume de dados históricos.

É utilizada uma fonte especial para a ligação:

![hydra tasks server](../../../images/hydratasksserver_1.png)

**Settings**

![hydra tasks server](../../../images/hydratasksserver_2.png)

- **Address** - o endereço do servidor Hydra.
- **Login** - login (necessário se o servidor exigir autorização).
- **Password** - palavra-passe (necessária se o servidor exigir autorização).
- **Time Offset** - um desfasamento temporal em dias relativamente à data atual, necessário para evitar o descarregamento de dados incompletos da sessão de negociação atual.
- **Weekends** - se deve descarregar dados dos fins de semana.

**Main**

- **Title** - o título da tarefa.
- **Working Hours** - definição do funcionamento da plataforma.
- **Interval of Operation** - intervalo de funcionamento.
- **Data Directory** - o diretório com dados onde serão guardados os ficheiros finais no formato [S#](../../api.md).
- **Format** - o formato dos dados: BIN/CSV.
- **Max. Errors** - o número máximo de erros; ao atingi-lo, a tarefa será parada. Por predefinição, 0 - o número de erros é ignorado.
- **Dependency** - uma tarefa que deve ser concluída antes de iniciar a atual.

**Logging**

- **Identifier** - identificador.
- **Logging Level** - o nível de registo.
