# Отладка в dump режиме

FAST коннектор доступен в режиме dump. В этом случае данные идут не из реального сетевого подключения, а из накопленных файлов программой [S\#.UDP Dumper](UdpDumper.md).

Для того, чтобы включить dump режим, необходимо перед подключение в коннектор передать через метод [IFastDialect.Dump](../api/StockSharp.Fix.Dialects.IFastDialect.Dump.html) dump файлы:

```cs
\/\/ ... инициализация коннектора
var fastAdapter \= (FastMessageAdapter)connector.Adapters.InnerAdapters.First();
IEnumerable\<string\> dumpFiles \= Directory.GetFiles(dumpDir, "\*.bin");
var dict \= dumpFiles.Select(f \=\>
{
	var name \= Path.GetFileNameWithoutExtension(f);
	var parts \= name.Split('\_').Skip(1).ToArray();
	var groupAddr \= parts\[0\];
	var port \= parts\[1\];
	var sourceAddr \= parts\[2\];
	if (sourceAddr.IsEmpty())
		sourceAddr \= null;
	return Tuple.Create(new MulticastSourceAddress
	{
		GroupAddress \= groupAddr.To\<IPAddress\>(),
		Port \= port.To\<int\>(),
		SourceAddress \= sourceAddr.To\<IPAddress\>(),
	}, f);
}).GroupBy(t \=\> t.Item1).ToDictionary(g \=\> g.Key, g \=\> (IEnumerable\<Stream\>)g.Select(p \=\> File.OpenRead(p.Item2)).ToArray());
			
fastAdapter.DialectSettings.Dump(dict);
\/\/ ...
connector.Connect();
```

После этого работа с коннектором идет в обычном режиме так, как если бы он получал данные из сети.
