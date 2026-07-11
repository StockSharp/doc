# ストレージドライブ

StockSharp のストレージドライブは、市場データの物理的な配置を担当します。配置先はローカルディスクまたはリモートサーバーです。基底インターフェイス [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) は、すべての実装に共通する契約を定義します。

## IMarketDataDrive -- 基底インターフェイス

[IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) インターフェイスは、次の主要な機能を提供します。

- **Path** -- データストレージへのパス。
- **GetAvailableSecuritiesAsync()** -- ストレージ内で利用可能なすべての銘柄リストを取得します。
- **GetAvailableDataTypesAsync()** -- 特定の銘柄で利用可能なデータ型のリストを取得します。
- **GetStorageDrive()** -- 特定の銘柄およびデータ型のストレージドライブを取得します。
- **VerifyAsync()** -- ストレージの整合性を検証します。
- **LookupSecuritiesAsync()** -- 指定した条件で銘柄を検索します。

## LocalMarketDataDrive -- ローカルファイルストレージ

[LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) クラスは、ファイルシステム内のローカルディスクにデータを保存する主要なドライブ実装です。

### 主な機能

- **ファイルシステム** -- データは、銘柄と日付ごとの階層ディレクトリ構造で整理されます。
- **インデックスシステム** -- 内部の `Index` クラスは、ファイルシステムをスキャンせずに高速なデータアクセスを提供します。インデックスファイルは `{instrument_path}{file_name}Dates2.bin` 形式で保存されます。
- **スレッドセーフ** -- マルチスレッドアプリケーションで正しく動作するように、データアクセスはロック機構によって保護されます。
- **インデックス構築** -- `BuildIndexAsync()` メソッドにより、大量データ操作後のパフォーマンス向上のためにインデックスを再構築できます。

### 使用例

```cs
// 指定したパスでローカルドライブを作成します
var localDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));

// ストレージレジストリで使用します
var storageRegistry = new StorageRegistry
{
    DefaultDrive = localDrive,
};

// 利用可能な銘柄のリストを取得します
await foreach (var secId in localDrive.GetAvailableSecuritiesAsync())
{
    Console.WriteLine(secId);
}
```

## RemoteMarketDataDrive -- リモートストレージ

[RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive) クラスでは、ネットワーク経由で市場データにアクセスするために、リモート Hydra サーバーへ接続できます。

### 接続設定

- **アドレス** -- リモートサーバーアドレス。既定値は `127.0.0.1:5002` です。
- **Credentials** -- 認証資格情報（Email と Password）。
- **TargetCompId** -- 対象コンポーネント識別子。既定値は `"StockSharpHydraMD"` です。
- **SecurityBatchSize** -- 銘柄を読み込む際のバッチサイズ。既定値は 1000 です。
- **Timeout** -- 接続タイムアウト。既定値は 2 分です。

### 使用例

```cs
// リモートドライブを作成します
var remoteDrive = new RemoteMarketDataDrive
{
    Address = "192.168.1.100:5002".To<EndPoint>(),
    Credentials = { Email = "user", Password = "pass".Secure() }
};

// 銘柄で利用可能なデータ型を取得します
var secId = "AAPL@NASDAQ".ToSecurityId();
await foreach (var dataType in remoteDrive.GetAvailableDataTypesAsync(secId, StorageFormats.Binary))
{
    Console.WriteLine(dataType);
}
```

リモートストレージの操作に関する詳細は、[リモートストレージの操作](remote.md) セクションを参照してください。

## DriveCache -- ドライブ管理

[DriveCache](xref:StockSharp.Algo.Storages.DriveCache) クラスは、ストレージドライブのコレクションを管理し、再利用のためのキャッシュを提供します。

### 主なメソッドとプロパティ

- **GetDrive(path)** -- パスによって既存のドライブを取得するか、新しいドライブを作成します。
- **DeleteDrive(drive)** -- キャッシュからドライブを削除します。
- **TryDefaultDrive** -- 最初に利用可能な [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive)。
- **NewDriveCreated** -- 新しいドライブ作成のイベント。
- **DriveDeleted** -- ドライブ削除のイベント。
- **Changed** -- ドライブコレクション変更のイベント。

このクラスは `IPersistable` インターフェイスを実装しており、ドライブ設定の保存と読み込みを可能にします。

### 使用例

```cs
// 既定のローカルドライブを持つキャッシュを作成します
var defaultDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));
var driveCache = new DriveCache(defaultDrive);

// パスによってドライブを取得または作成します
var anotherDrive = driveCache.GetDrive(@"D:\MarketData");

// イベントを購読します
driveCache.NewDriveCreated += drive =>
    Console.WriteLine($"ドライブを作成しました: {drive.Path}");
```

## 関連項目

- [API の操作](api.md)
- [リモートストレージの操作](remote.md)
- [ストレージ形式](formats.md)

