# 转储模式

FAST 连接器支持在转储模式下工作。在这种情况下，数据不是来自真实的网络连接，而是来自应用程序 [UDP Dumper](../../../../udp_dumper.md) 的累积文件。

要启用转储模式，需要在连接之前，通过转储方法 [IFastDialect.Dump](xref:StockSharp.Fix.Dialects.IFastDialect.Dump(System.Collections.Generic.IDictionary{Ecng.Net.MulticastSourceAddress,System.Collections.Generic.IEnumerable{System.IO.Stream}}))**(**[System.Collections.Generic.IDictionary\<Ecng.Net.MulticastSourceAddress,System.Collections.Generic.IEnumerable\<System.IO.Stream\>\>](xref:System.Collections.Generic.IDictionary`2) dumpFiles **)** 传入文件：

```cs
// ... connector initialization
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

之后，连接器的工作方式和平常一样，就好像它是在从网络接收数据一样。
