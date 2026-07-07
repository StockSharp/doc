# Configuração gráfica Rithmic

Para todos os produtos [S#](../../../../api.md), a configuração gráfica da ligação é efetuada na [janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md):

![API GUI Settings Rithmic](../../../../../images/api_gui_settings_rithmic.png)

- **Login** - Login.
- **Password** - Password.
- **Certificate** - Caminho para o ficheiro de certificado, necessário para ligar ao sistema Rithmic.
- **File log** - Caminho para o ficheiro de registo.
- **Server type** - Tipo de servidor.
- **Point (admin)** - Ponto de ligação para funções administrativas (inicialização/desinicialização).
- **Point (data)** - Ponto de ligação aos dados de mercado.
- **Login (trans)** - Login adicional. Utilizado quando o envio de transações é efetuado para um servidor separado.
- **Point (transactions)** - Ponto de ligação ao sistema de execução de transações.
- **Password (trans)** - Password adicional. Utilizada quando o envio de transações é efetuado para um servidor separado.
- **Point (positions)** - Ponto de ligação para acesso a informações sobre carteiras e posições.
- **Point (history)** - Ponto de ligação para acesso a dados históricos.
- **Domain (address)** - Endereço do domínio.
- **Domain (name)** - Nome do domínio.
- **Licenses** - Endereço do servidor de licenças.
- **Broker** - Endereço do corretor.
- **Log (address)** - Endereço do logger.
- **User name (hist)** - Login adicional. ID de utilizador utilizado para autenticação com a history plant.
- **Password (hist)** - Password adicional. Password utilizada para autenticação com a history plant.
- **Heart beat** - Intervalo de verificação do servidor para acompanhar se a ligação está ativa. Por predefinição, é igual a 1 minuto.
- **Reconnection settings** - Mecanismo para acompanhar ligações com as definições do sistema de negociação. ([Definições de religação](../../reconnection_settings.md))

## Conteúdo recomendado

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Criar o próprio conector](../../creating_own_connector.md)

[Guardar e carregar definições](../../save_and_load_settings.md)
