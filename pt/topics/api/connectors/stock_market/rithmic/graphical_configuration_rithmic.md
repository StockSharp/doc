# Configuração gráfica Rithmic

Para todos os produtos [S#](../../../../api.md), a configuração gráfica da ligação é efetuada na [Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md):

![Configurações da API GUI Rithmic](../../../../../images/api_gui_settings_rithmic.png)

- **nome de utilizador** - Login.
- **palavra-passe** - Password.
- **certificado** - Caminho para o ficheiro de certificado, necessário para ligar ao sistema Rithmic.
- **Ficheiro de registo** - Caminho para o ficheiro de registo.
- **Tipo de servidor** - Tipo de servidor.
- **Ponto (administração)** - Ponto de ligação para funções administrativas (inicialização/desinicialização).
- **Ponto (dados)** - Ponto de ligação aos dados de mercado.
- **nome de utilizador (transações)** - Login adicional. Utilizado quando o envio de transações é efetuado para um servidor separado.
- **Ponto (transações)** - Ponto de ligação ao sistema de execução de transações.
- **palavra-passe (transações)** - Password adicional. Utilizada quando o envio de transações é efetuado para um servidor separado.
- **Ponto (posições)** - Ponto de ligação para acesso a informações sobre carteiras e posições.
- **Ponto (histórico)** - Ponto de ligação para acesso a dados históricos.
- **domínio (endereço)** - Endereço do domínio.
- **domínio (nome)** - Nome do domínio.
- **Licenças** - Endereço do servidor de licenças.
- **Corretor** - Endereço do corretor.
- **Log (endereço)** - Endereço do logger.
- **Nome de utilizador (hist)** - Login adicional. ID de utilizador utilizado para autenticação com a history plant.
- **palavra-passe (histórico)** - Password adicional. Password utilizada para autenticação com a history plant.
- **Intervalo de verificação da ligação** - Intervalo de verificação do servidor para acompanhar se a ligação está ativa. Por predefinição, é igual a 1 minuto.
- **Definições de religação** - Mecanismo para acompanhar ligações com as definições do sistema de negociação. ([Definições de religação](../../reconnection_settings.md))

## Conteúdo recomendado

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Criar o próprio conector](../../creating_own_connector.md)

[Guardar e carregar definições](../../save_and_load_settings.md)
