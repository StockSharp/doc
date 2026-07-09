# Pasta de definições

As seguintes pastas são importantes para o [Designer](../../designer.md):

1. A pasta onde o [Designer](../../designer.md) está instalado. A partir desta pasta, pode iniciar o [Designer](../../designer.md) executando **Designer.exe**, ou atualizar o [Designer](../../designer.md) executando **Designer.Update.exe**. Eliminar esta pasta remove o [Designer](../../designer.md), mas as definições do [Designer](../../designer.md) não são eliminadas.

2. A pasta de definições do **Designer** está localizada na pasta de documentos do utilizador: ...\\StockSharp\\Designer\\ (por exemplo, c:\\Users\\User\\Documents\\StockSharp\\Designer\\). Eliminar esta pasta repõe todas as definições do [Designer](../../designer.md) para os valores predefinidos. **Todas as estratégias criadas, instrumentos descarregados e outras informações armazenadas na pasta de definições serão DESTRUÍDOS.**

![Designer Directory and edit the data manually 00](../../../images/designer_directory_and_edit_data_manually_00.png)

Esta pasta contém as seguintes pastas e ficheiros:

- **Compositions** armazena, como ficheiros XML, todos os blocos da pasta **Elementos compostos** no painel [Schemas](../user_interface/schemas.md). Eliminar ficheiros desta pasta remove o **Elemento composto** correspondente da pasta **Elementos compostos** no painel [Schemas](../user_interface/schemas.md). Não edite estes ficheiros manualmente; fazê-lo pode danificar o bloco **Elementos compostos** correspondente.
- **LiveStrategies** armazena, como ficheiros XML, todos os blocos da pasta **Negociação** no painel [Schemas](../user_interface/schemas.md). Eliminar ficheiros desta pasta remove a estratégia da pasta **Negociação** no painel [Schemas](../user_interface/schemas.md). Não edite estes ficheiros manualmente; fazê-lo pode danificar a estratégia correspondente.
- **Registos** contém todos os registos de falhas do [Designer](../../designer.md), o que simplifica a resolução de problemas do [Designer](../../designer.md).
- **SourceCode** armazena, como ficheiros XML, todos os blocos da pasta **Código-fonte** no painel [Schemas](../user_interface/schemas.md). Eliminar ficheiros desta pasta remove o bloco **Código-fonte** da pasta **Código-fonte** no painel [Schemas](../user_interface/schemas.md). Não edite estes ficheiros manualmente; fazê-lo pode danificar o bloco **Código-fonte** correspondente.
- **Estratégias** armazena, como ficheiros XML, todos os blocos da pasta **Estratégias** no painel [Schemas](../user_interface/schemas.md). Eliminar ficheiros desta pasta remove a estratégia da pasta **Estratégias** no painel Schemas. Não edite estes ficheiros manualmente; fazê-lo pode danificar a estratégia correspondente. Se adicionar manualmente um ficheiro de estratégia a esta pasta e reiniciar o [Designer](../../designer.md), a estratégia aparece na pasta **Estratégias** no painel [Schemas](../user_interface/schemas.md).
- **Armazenamento** contém dados de mercado descarregados pelo [Designer](../../designer.md) para o [Armazenamento de dados de mercado](../market_data_storage.md) correspondente. A pasta é criada quando o [Armazenamento de dados de mercado](../market_data_storage.md) é criado, e o caminho predefinido aponta para esta pasta. Eliminar esta pasta remove todos os dados de mercado descarregados do armazenamento correspondente. Se o armazenamento contiver ficheiros CSV, estes podem ser editados num editor de texto padrão ou no MS Excel. Os ficheiros BIN não podem ser editados manualmente.
- **exchange.csv** e **exchangeboard.csv** contêm a lista de **bolsas**, códigos de instrumentos e modos de negociação. Estes ficheiros podem ser editados num editor de texto padrão ou no MS Excel.
- **security.csv** contém todos os instrumentos recebidos e criados em todas as fontes. Eliminar este ficheiro remove todos os instrumentos do [Designer](../../designer.md). A adição de novos instrumentos é descrita em [Transferir instrumentos](../market_data_storage/download_instruments.md) e [Criar instrumento](../market_data_storage/create_instrument.md). Este ficheiro pode ser editado num editor de texto padrão ou no MS Excel.
- **portfolio.csv** e **position.csv** contêm todos os portefólios recebidos e criados e as respetivas posições atuais. Eliminar estes ficheiros remove os dados correspondentes do [Designer](../../designer.md). Se o [Designer](../../designer.md) receber informações de portefólio em cada ligação, as informações de posições ainda assim podem perder-se permanentemente. Estes ficheiros podem ser editados num editor de texto padrão ou no MS Excel.
- **settings.json** contém as definições atuais. O [Designer](../../designer.md) cria este ficheiro quando as definições são alteradas ou quando o programa é fechado. Eliminar este ficheiro repõe as definições atuais para os valores predefinidos. Não edite este ficheiro manualmente; fazê-lo pode danificar o [Designer](../../designer.md).

Antes de editar manualmente ficheiros individuais ou repor as definições do [Designer](../../designer.md), faça cópias de segurança dos ficheiros que alterar ou de toda a pasta.

## Ver também

[Atualizar para a nova versão](../update_to_the_new_version.md)
