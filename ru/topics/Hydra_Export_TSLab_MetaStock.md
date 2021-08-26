# Экспорт в форматы TSLab и MetaStock

Для экспорта данных в файлы форматов TSLab и MetaStock необходимо из раскрывающегося списка выбрать формат Txt:

![hydra export](~/images/hydra_export.png)

При экспорте в файлы текстового формата (Txt) появляется окно: 

![hydra export TSLab Meta Stock 2](~/images/hydra_export_TSLab_MetaStock_2.png)

В котором необходимо указать шаблон экспорта, где в фигурных скобках указаны свойства, которые необходимо экспортировать, и их порядок: 

```none
{SecurityId.SecurityCode},5,{OpenTime:yyyyMMdd},{OpenTime:default:HH:mm:ss},{OpenPrice},{HighPrice},{LowPrice},{ClosePrice},{TotalVolume}
	  				
```

В примере на втором месте указан тайм фрейм свечей в 5 минут. Если необходимо экспортировать в формате с использованием двоеточий, то необходимо указать ключевое слово **default** как в примере выше **{OpenTime:default:HH:mm:ss}**. 

Также следует задать первую строку (Заголовок) в файле: 

```none
\<TICKER\>,\<PER\>,\<DATE\>,\<TIME\>,\<OPEN\>,\<HIGH\>,\<LOW\>,\<CLOSE\>,\<VOL\>
	  				
```

## См. также
