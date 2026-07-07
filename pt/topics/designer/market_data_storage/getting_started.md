# Introdução

Para criar um armazenamento de dados históricos, clique no botão ![Designer Creating a repository of historical data 00](../../../images/designer_creating_repository_of_historical_data_00.png) no separador **Market data**. Clique em ![Designer Creating a repository of historical data 01](../../../images/designer_creating_repository_of_historical_data_01.png) para alterar os parâmetros do armazenamento atual. Clique em ![Designer Creating a repository of historical data 02](../../../images/designer_creating_repository_of_historical_data_02.png) para eliminar o armazenamento atual da lista de armazenamentos.

![Designer Creating a repository of historical data 03](../../../images/designer_creating_repository_of_historical_data_03.png)

O armazenamento de dados históricos pode ser local ou remoto.

Armazenamento local - quando todos os dados são guardados no computador local. Para configurar o armazenamento local, basta indicar o caminho para a pasta com os dados armazenados.

O armazenamento remoto pode estar localizado noutro computador. Para o configurar, especifique o endereço do armazenamento remoto e, se necessário, o login e a palavra-passe.

Pode reproduzir o armazenamento remoto numa máquina local usando o software [Hydra](../../hydra.md) (nome de código Hydra), concebido para o carregamento automático de dados de mercado (instrumentos, candles, tick trades e livros de ordens, etc.) de várias fontes e para os armazenar no armazenamento local. Para isso, coloque o [Hydra](../../hydra.md) em modo de servidor.

![Designer Creating a repository of historical data 04](../../../images/designer_creating_repository_of_historical_data_04.png)

Depois disso, no [Designer](../../designer.md), crie um novo armazenamento clicando no botão ![Designer Creating a repository of historical data 00](../../../images/designer_creating_repository_of_historical_data_00.png). Nas definições de armazenamento, no campo de endereço, especifique "net.tcp:\/\/localhost:8000". Clique em OK. Ao usar o [Hydra](../../hydra.md) como armazenamento remoto, não se esqueça de que o [Hydra](../../hydra.md) deve estar iniciado e configurado em conformidade.

![Designer Creating a repository of historical data 05](../../../images/designer_creating_repository_of_historical_data_05.png)

Depois de adicionar um novo armazenamento, este pode ser selecionado na lista pendente **Storage**.

![Designer Creating a repository of historical data 06](../../../images/designer_creating_repository_of_historical_data_06.png)

Também tem de selecionar o formato dos ficheiros de armazenamento - BIN ou CSV. Os dados podem ser guardados em dois formatos: num formato binário especial BIN, que proporciona a taxa máxima de compressão, ou no formato de texto CSV, que é conveniente ao analisar dados noutros programas. O formato BIN é preferível quando há necessidade de poupar espaço em disco. O formato CSV é preferível quando há necessidade de ajustar dados manualmente. O CSV é facilmente editado pelo bloco de notas padrão, MS Excel, etc.

## Conteúdo recomendado

[Transferir instrumentos](download_instruments.md)
