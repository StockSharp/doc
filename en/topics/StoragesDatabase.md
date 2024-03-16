# Database

The work with a database is performed using SQLite. The database is created at first start of [Hydra](Hydra.md) and saved (by default) in the

**My Documents\\StockSharp\\Hydra\\**

> [!TIP]
> All settings are in the **Hydra.exe.config** file that is in the folder from which [Hydra](Hydra.md) is started.

To change the path of the database saving, it is necessary to edit the following part of the **Hydra.exe.config** file:

```none
	<connectionStrings>
		<!--<add name="SqlServerConStr" connectionString="Server=.\SQLExpress;Database=Trading;User ID=trading;Password=trading;" providerName=""/>-->
		<add name="SQLiteConStr" connectionString="Data Source=%Documents%\StockSharp\Hydra\StockSharp.db" providerName="System.Data.SQLite" />
	</connectionStrings>
		
```

For example, to replace **%Documents%\\StockSharp\\Hydra\\StockSharp.db** with **C:\\StockSharp.db**, to save the database in the root of the drive C:\\.

## Next Steps

[Installing Hydra](HydraUsing.md)
