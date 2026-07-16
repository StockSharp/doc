# Configuração do conector CTP

Obtenha as credenciais, os endereços dos servidores e os identificadores da conta necessários para a API de negociação e dados de mercado CTP e indique as seguintes definições do conector.

- **Nome de utilizador** - propriedade `Login`.
- **Palavra-passe** - propriedade `Password`.
- **Identificador do corretor** - propriedade `BrokerId`.
- **Identificador do investidor** - propriedade `InvestorId`.
- **Endereço de dados de mercado** - propriedade `MarketDataAddress`.
- **Endereço do servidor de negociação** - propriedade `TraderAddress`.
- **Identificador da aplicação** - propriedade `AppId`.
- **Código de autenticação** - propriedade `AuthCode`.
- **Informações do produto** - propriedade `ProductInfo`. Valor predefinido: `StockSharp`.
- **Modo de recuperação** - propriedade `ResumeType`. Valor predefinido: `Quick`.
- **Modo de produção** - propriedade `ProductionMode`. Valor predefinido: `true`.
- **Diretório de dados** - propriedade `DataPath`.
- **Intervalo de consultas** - propriedade `QueryInterval`. Valor predefinido: `1 s`.
- **Tempo limite da ligação** - propriedade `ConnectionTimeout`. Valor predefinido: `30 s`.
