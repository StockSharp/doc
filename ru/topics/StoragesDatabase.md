# База данных

Работа с базой данных ведется с использованием SQLite. База данных создается при первом запуске [S\#.Data](Hydra.md) и сохраняется (по умолчанию) в

**Мои Документы\\StockSharp\\Hydra\\**

> [!TIP]
> Все настройки находятся в файле Hydra.exe.config, который лежит в папке из которой запускается [S\#.Data](Hydra.md).

Для изменения сохранения пути БД, необходимо в Hydra.exe.config отредактировать следующее место:

```none
	<connectionStrings>
		<!--<add name="SqlServerConStr" connectionString="Server=.\SQLExpress;Database=Trading;User ID=trading;Password=trading;" providerName=""/>-->
		<add name="SQLiteConStr" connectionString="Data Source=%Documents%\StockSharp\Hydra\StockSharp.db" providerName="System.Data.SQLite" />
	</connectionStrings>
		
```

Например, заменить **%Documents%\\StockSharp\\Hydra\\StockSharp.db** на **С:\\StockSharp.db**, для сохранения БД в корень диска С:\\.

## Следующие шаги

[Установка S\#.Data](HydraUsing.md)
