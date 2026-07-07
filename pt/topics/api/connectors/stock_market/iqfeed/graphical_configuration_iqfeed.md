# Configuração gráfica IQFeed

Para todos os produtos [S#](../../../../api.md), a configuração gráfica da ligação é efetuada na [Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md):

![API GUI Settings IQFeed](../../../../../images/api_gui_settings_iqfeed.png)

- **Level1 server** - Endereço para obter dados de Level1.
- **Level2 server** - Endereço para obter dados de Level2.
- **Lookup server** - Endereço para obter dados históricos.
- **Admin server** - Endereço para obter dados de serviço.
- **Derivatives** - Endereço para obter dados de derivados.
- **Data for Level1** - Todos os tipos de dados para Level1 que têm de ser transmitidos.
- **Data type** - Tipos de títulos para os quais os dados devem ser recebidos.
- **Load securities** - Se o conjunto completo de títulos deve ser carregado a partir do arquivo do site IQFeed.
- **File with securities** - Caminho para o ficheiro com a lista de títulos do IQFeed, descarregado do site. Se o caminho for especificado, não ocorre um segundo descarregamento a partir do site e apenas a cópia local é analisada.
- **Version** - Versão.
- **Heart beat** - Intervalo de verificação do servidor para controlar se a ligação está ativa. Por predefinição, é igual a 1 minuto.
- **Reconnection settings** - Mecanismo de controlo das ligações com as definições do sistema de negociação. ([Definições de religação](../../reconnection_settings.md))

## Conteúdo recomendado

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Criar o próprio conector](../../creating_own_connector.md)

[Guardar e carregar definições](../../save_and_load_settings.md)
