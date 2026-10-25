# Pasta de definições

As seguintes pastas são importantes para o [Designer](../../designer.md):

1. A pasta onde o [Designer](../../designer.md) está instalado. A partir desta pasta, pode iniciar o [Designer](../../designer.md) executando **Designer.exe**, ou atualizar o [Designer](../../designer.md) executando **Designer.Update.exe**. Eliminar esta pasta remove o [Designer](../../designer.md), mas as definições do [Designer](../../designer.md) não são eliminadas.

2. A pasta de definições do **Designer** está localizada na pasta de documentos do utilizador: ...\\StockSharp\\Designer\\ (por exemplo, c:\\Users\\User\\Documents\\StockSharp\\Designer\\). Eliminar esta pasta repõe todas as definições do [Designer](../../designer.md) para os valores predefinidos. **Todas as estratégias criadas, instrumentos descarregados e outras informações armazenadas na pasta de definições serão DESTRUÍDOS.**

![Designer Diretório e edição manual dos dados 00](../../../images/designer_directory_and_edit_data_manually_00.png)

Esta pasta contém as seguintes pastas e ficheiros:

- As pastas `Backtest`, `Live`, `Remote`, `Optimization`, `Custom` e `Indicator` correspondem às pastas **Backtest**, **Ao vivo**, **Remotamente**, às otimizações, **Elementos próprios** e **Indicadores** do [Painel de esquemas](../user_interface/schemas.md). Cada estratégia, bloco ou indicador é guardado no seu próprio ficheiro JSON com um identificador em vez do nome. Eliminar um ficheiro elimina a estratégia ou o bloco correspondente do [Painel de esquemas](../user_interface/schemas.md). Não edite estes ficheiros manualmente; isso pode danificar a estratégia ou o bloco correspondente. Um ficheiro que não foi possível ler é ignorado no arranque: as restantes estratégias são carregadas e a causa é escrita no registo. Se adicionar manualmente um ficheiro de estratégia a uma destas pastas e reiniciar o [Designer](../../designer.md), a estratégia aparecerá na pasta correspondente do [Painel de esquemas](../user_interface/schemas.md).
- `Logs` contém todos os registos de falhas do [Designer](../../designer.md), o que simplifica a resolução de problemas do [Designer](../../designer.md).
- `Storage` contém dados de mercado descarregados pelo [Designer](../../designer.md) para o [Armazenamento de dados de mercado](../market_data_storage.md) correspondente. A pasta é criada quando o [Armazenamento de dados de mercado](../market_data_storage.md) é criado, e o caminho predefinido aponta para esta pasta. Eliminar esta pasta remove todos os dados de mercado descarregados do armazenamento correspondente. Se o armazenamento contiver ficheiros CSV, estes podem ser editados num editor de texto padrão ou no MS Excel. Os ficheiros BIN não podem ser editados manualmente.
- **exchange.csv** e **exchangeboard.csv** contêm a lista de **bolsas**, códigos de instrumentos e modos de negociação. Estes ficheiros podem ser editados num editor de texto padrão ou no MS Excel.
- **security.csv** contém todos os instrumentos recebidos e criados em todas as fontes. Eliminar este ficheiro remove todos os instrumentos do [Designer](../../designer.md). A adição de novos instrumentos é descrita em [Transferir instrumentos](../market_data_storage/download_instruments.md) e [Criar instrumento](../market_data_storage/create_instrument.md). Este ficheiro pode ser editado num editor de texto padrão ou no MS Excel.
- **portfolio.csv** e **position.csv** contêm todos os portefólios recebidos e criados e as respetivas posições atuais. Eliminar estes ficheiros remove os dados correspondentes do [Designer](../../designer.md). Se o [Designer](../../designer.md) receber informações de portefólio em cada ligação, as informações de posições ainda assim podem perder-se permanentemente. Estes ficheiros podem ser editados num editor de texto padrão ou no MS Excel.
- **settings.json** contém as definições atuais. O [Designer](../../designer.md) cria este ficheiro quando as definições são alteradas ou quando o programa é fechado. Eliminar este ficheiro repõe as definições atuais para os valores predefinidos. Não edite este ficheiro manualmente; fazê-lo pode danificar o [Designer](../../designer.md).

Antes de editar manualmente ficheiros individuais ou repor as definições do [Designer](../../designer.md), faça cópias de segurança dos ficheiros que alterar ou de toda a pasta.

## Ver também

[Atualizar para a nova versão](../update_to_the_new_version.md)
