# 使用銘柄

## 説明

基底クラス `Strategy` の `GetWorkingSecurities()` メソッドは、ストラテジーが動作で使用する銘柄とデータ型のリストを取得するために使用されます。このメソッドは、[Designer](../../designer.md) を使用する際に重要な役割を果たします。

## 目的

このメソッドの主な目的は、ストラテジーの動作に必要な銘柄とデータ型に関する情報を Designer に提供することです。これにより、Designer は次のことを行えます。

1. テスト開始前に、必要な履歴データがストレージに存在するか確認する
2. 利用可能な場合に必要なデータを自動的に読み込む
3. ストラテジー起動時にサブスクリプションを正しく設定する

## 実装

基底クラス `Strategy` では、このメソッドは空のコレクションを返します。Designer と正しく連携するには、ストラテジーでこのメソッドをオーバーライドすることをお勧めします。

```cs
public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
{
	// ストラテジーで使用するペア（銘柄、データ型）のリストを返す
	return new[] 
	{ 
		(Security, CandleType),
		// ストラテジーが複数を使用する場合は、他の銘柄とデータ型のペア
	};
}
```

## メソッドをオーバーライドする重要性

ストラテジーで `GetWorkingSecurities()` メソッドをオーバーライドしない場合:

- Designer は必要なデータを自動的に確認できません
- 必要な履歴データがストレージにない場合でも、Designer は警告を出しません
- ストラテジーをテスト用に起動できる場合がありますが、結果は表示されません
- ユーザーは結果が表示されない理由に関する情報を受け取れません

## 使用例

```cs
public class MySmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleType;
	
	public DataType CandleType
	{
		get => _candleType.Value;
		set => _candleType.Value = value;
	}
	
	public MySmaStrategy()
	{
		_candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
	}
	
	// Designer と正しく連携するためにメソッドをオーバーライド
	public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
	{
		return new[] { (Security, CandleType) };
	}
	
	// ストラテジーコードの残り...
}
```

## 結論

`GetWorkingSecurities()` メソッドは、ストラテジーの基本機能を実装するうえで必須ではありませんが、StockSharp Designer と正しく連携するためには、オーバーライドすることを強くお勧めします。これにより、必要な履歴データがないために、ストラテジーをテスト用に起動しても結果が表示されない状況を避けられます。
