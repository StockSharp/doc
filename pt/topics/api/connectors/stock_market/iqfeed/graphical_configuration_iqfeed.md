# Configuração gráfica IQFeed

Para todos os produtos [S#](../../../../api.md), a configuração gráfica da ligação é efetuada na [Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md):

![API GUI Settings IQFeed](../../../../../images/api_gui_settings_iqfeed.png)

- **Servidor Level1** - Endereço para obter dados de Level1.
- **Servidor Level2** - Endereço para obter dados de Level2.
- **Servidor Lookup** - Endereço para obter dados históricos.
- **Servidor Admin** - Endereço para obter dados de serviço.
- **Derivados** - Endereço para obter dados de derivados.
- **Dados de Level1** - Todos os tipos de dados para Level1 que têm de ser transmitidos.
- **Tipo de dados** - Tipos de títulos para os quais os dados devem ser recebidos.
- **Carregar títulos** - Se o conjunto completo de títulos deve ser carregado a partir do arquivo do site IQFeed.
- **Ficheiro com títulos** - Caminho para o ficheiro com a lista de títulos do IQFeed, descarregado do site. Se o caminho for especificado, não ocorre um segundo descarregamento a partir do site e apenas a cópia local é analisada.
- **Versão** - Versão.
- **Intervalo de verificação da ligação** - Intervalo de verificação do servidor para controlar se a ligação está ativa. Por predefinição, é igual a 1 minuto.
- **Definições de religação** - Mecanismo de controlo das ligações com as definições do sistema de negociação. ([Definições de religação](../../reconnection_settings.md))

## Conteúdo recomendado

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Criar o próprio conector](../../creating_own_connector.md)

[Guardar e carregar definições](../../save_and_load_settings.md)
