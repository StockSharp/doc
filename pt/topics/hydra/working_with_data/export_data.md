# Exportar dados

O [Hydra](../../hydra.md) permite exportar os dados de mercado recebidos em vários formatos, incluindo [formatos de dados MetaStock](export_data/export_into_metastock.md).

Para exportar dados, são usados ficheiros nos formatos [Excel](https://en.wikipedia.org/wiki/Excel), xml, bin, txt, Json ou tabelas SQL.

Para a exportação, deve selecionar o formato de ficheiro necessário na lista pendente:

![Hydra exportação](../../../images/hydra_export.png)

Depois é necessário selecionar uma pasta e alterar o nome do ficheiro, se necessário.

Ao exportar para ficheiros de texto (txt), aparece uma janela na qual pode especificar o modelo de exportação no formato: 

**{OpenTime:default:yyyyMMdd};{OpenTime:default:HH:mm:ss};{OpenPrice};{HighPrice};{LowPrice};{ClosePrice};{TotalVolume}**

Aqui, entre chavetas, são indicadas as propriedades a exportar e a respetiva ordem, separadas por ponto e vírgula.

Ao clicar no botão **Pré-visualizar**, pode ver que dados serão guardados no ficheiro.

![Hydra exportação TSLab MetaStock 1](../../../images/hydra_export_tslab_metastock_1.png)

O utilizador pode adicionar propriedades adicionais, como o código do instrumento através da propriedade **{SecurityId.SecurityCode}**, ou especificar um valor de período.

Pode adicionar um cabeçalho indicando o nome das propriedades. Neste caso, o registo terá o seguinte aspeto.

![Hydra exportação TSLab MetaStock 2](../../../images/hydra_export_tslab_metastock_2.png)

Se precisar de exportar num formato que use dois-pontos, deve especificar a palavra-chave default como no exemplo acima **{OpenTime:default:HH:mm:ss}**.

**Veja o [tutorial em vídeo](../videos/saving_format.md)**
