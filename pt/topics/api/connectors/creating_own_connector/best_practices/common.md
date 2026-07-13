# Melhores Práticas

Ao desenvolver um conector para diversas bolsas dentro da plataforma StockSharp, recomenda-se seguir uma abordagem estabelecida que envolve dividir a funcionalidade em vários componentes-chave:

1. [Autenticação](authentication.md) - Gerencia o gerenciamento de chaves de API e a geração de assinaturas de solicitação.
2. [Cliente REST](rest_client.md) - Facilita a interação com a API REST da bolsa.
3. [Cliente WebSocket](websocket_client.md) - Gerencia dados em tempo real através de conexões WebSocket.
4. [Conversão de Tipos](type_conversion.md) - Fornece métodos para converter entre tipos de dados do StockSharp e formatos específicos da bolsa.

Essa divisão permite uma estrutura de código mais modular e sustentável, facilita os testes e possibilita a reutilização de componentes em outros projetos.

Ao implementar cada um desses componentes, é importante considerar as especificidades da bolsa em particular, mas a estrutura geral permanece semelhante para a maioria das bolsas.
