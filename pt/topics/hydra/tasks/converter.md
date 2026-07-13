# Conversor

A tarefa converte dados da bolsa. Por exemplo, de registos de ordens para ticks ou de ticks para velas, etc.

![Hydra tarefas de conversão](../../../images/hydra_tasks_converter.png)

**Conversor**

- **Conversor** - conversor.
- **Origem** - que tipo de dados será convertido.
- **Formato de dados** - formato dos dados convertidos.
- **Data inicial** - a partir de que data iniciar a conversão dos dados.
- **Desfasamento temporal** - o desfasamento temporal em dias relativamente à data em que a tarefa foi iniciada. Isto impede a conversão de um dia incompleto. Se estiver configurada a conversão de dados em tempo real, o intervalo de atualização pode deixar o dia atual apenas parcialmente convertido. Utilize o desfasamento temporal para evitar isso.
- **Destino** - o diretório de dados onde os dados convertidos serão guardados.

**Livros de ordens**

- **Intervalo** - intervalo de geração dos livros de ordens.
- **Profundidade** - profundidade máxima da geração dos livros de ordens.
- **Registo de ordens** - como construir livros de ordens a partir do registo de ordens.

  Cada bolsa tem o seu próprio formato de **registo de ordens**; o programa [Hydra](../../hydra.md) suporta três formatos:
  - **Por predefinição** - é utilizado na maioria dos casos.
  - **ITCH** - é utilizado para o protocolo ITCH (bolsas: LSE e Nasdaq).

**Geral**

- **Cabeçalho** - Conversor.
- **Horário de trabalho** - configuração do horário de funcionamento do mercado. ![Hydra tarefas de cópia de segurança](../../../images/hydra_tasks_backup_desk.png)
- **Intervalo de operação** - o intervalo de funcionamento.
- **Diretório de dados** - diretório de dados, de onde serão recebidos os dados para conversão.
- **Formato** - o formato dos dados convertidos: BIN\/CSV.
- **Máx. erros** - o número máximo de erros; ao atingi-lo, a tarefa será parada. Por predefinição, 0 - o número de erros é ignorado.
- **Dependência** - uma tarefa que deve ser executada antes de iniciar a atual.

**Registo**

- **Identificador** - o identificador.
- **Nível de registo** - o nível de registo.

Consideremos um exemplo de conversão de dados.

1. Aceda à tarefa **Conversor**. ![Hydra tarefas de conversão 00](../../../images/hydra_tasks_converter_00.png)
2. Selecione o instrumento e, na janela que aparece, defina o tipo de dados que devemos receber durante a conversão, bem como o tipo de dados a partir do qual devemos converter. Por exemplo, precisa de converter Ticks em velas com um período de 15 minutos.

   > [!TIP]
> IMPORTANTE\! O período de dados solicitado deve corresponder ao período disponível para conversão; caso contrário, os dados não serão convertidos. Nas definições, especifique o formato correto dos dados de origem para que corresponda ao formato dos dados que estão a ser convertidos.
3. Especifique os diretórios necessários. Desfasamento temporal. Intervalo de funcionamento.
4. Iniciamos a conversão.![Hydra tarefas de conversão 01](../../../images/hydra_tasks_converter_01.png)

Pode ver-se que os dados foram convertidos. [Vamos analisar](../working_with_data/view_and_export.md) os dados resultantes.

![Hydra tarefas de conversão 02](../../../images/hydra_tasks_converter_02.png)

Esta função é semelhante a [obter os dados de mercado necessários](../working_with_data/any_market_data_types.md) a partir de outro tipo de dados.

**Veja o [tutorial em vídeo](../videos/converter_task.md)**
