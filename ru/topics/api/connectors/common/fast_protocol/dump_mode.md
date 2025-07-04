# Отладка в dump режиме

FAST коннектор доступен в режиме dump. В этом случае данные идут не из реального сетевого подключения, а из накопленных файлов программой [UDP Dumper](../../../../udp_dumper.md).

Для того, чтобы включить dump режим, необходимо перед подключением в коннектор передать через метод [IFastDialect.Dump](xref:StockSharp.Fix.Dialects.IFastDialect.Dump(System.Collections.Generic.IDictionary{Ecng.Net.MulticastSourceAddress,System.Collections.Generic.IEnumerable{System.IO.Stream}}))**(**[System.Collections.Generic.IDictionary\<Ecng.Net.MulticastSourceAddress,System.Collections.Generic.IEnumerable\<System.IO.Stream\>\>](xref:System.Collections.Generic.IDictionary`2) dumpFiles **)** dump файлы:

```cs
// ... инициализация коннектора
var fastAdapter = (FastMessageAdapter)connector.Adapters.InnerAdapters.First();
IEnumerable<string> dumpFiles = Directory.GetFiles(dumpDir, "*.bin");
var dict = dumpFiles.Select(f =>
{
	var name = Path.GetFileNameWithoutExtension(f);
	var parts = name.Split('_').Skip(1).ToArray();
	var groupAddr = parts[0];
	var port = parts[1];
	var sourceAddr = parts[2];
	if (sourceAddr.IsEmpty())
		sourceAddr = null;
	return Tuple.Create(new MulticastSourceAddress
	{
		GroupAddress = groupAddr.To<IPAddress>(),
		Port = port.To<int>(),
		SourceAddress = sourceAddr.To<IPAddress>(),
	}, f);
}).GroupBy(t => t.Item1).ToDictionary(g => g.Key, g => (IEnumerable<Stream>)g.Select(p => File.OpenRead(p.Item2)).ToArray());
			
fastAdapter.DialectSettings.Dump(dict);
// ...
connector.Connect();
```

После этого работа с коннектором идет в обычном режиме так, как если бы он получал данные из сети.
