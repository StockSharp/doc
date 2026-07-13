# Exportar para MetaStock

Para exportar dados para ficheiros no formato MetaStock, selecione o formato Txt na lista pendente:

![Hydra exportação](../../../../images/hydra_export.png)

Ao exportar para ficheiros em formato de texto (Txt), aparece uma janela: 

![Hydra exportação Meta Stock 2](../../../../images/hydra_export_tslab_metastock_2.png)

Nesta janela, especifique o modelo de exportação, no qual as chavetas indicam as propriedades a exportar e a respetiva ordem:

```none
{SecurityId.SecurityCode},5,{OpenTime:yyyyMMdd},{OpenTime:HHmmss},{OpenPrice},{HighPrice},{LowPrice},{ClosePrice},{TotalVolume}
	  				
```

No exemplo, o período da vela de cinco minutos é especificado na segunda posição.

Além disso, a primeira linha (Header) deve ser definida no ficheiro: 

```none
<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>
	  				
```
