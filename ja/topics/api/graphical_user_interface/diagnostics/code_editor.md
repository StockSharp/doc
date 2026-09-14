# コードエディタ

![スクリーンショット: 構文強調表示のあるコードエディタ](../../../../images/gui_codepanel.png)

[CodePanel](xref:StockSharp.Xaml.CodeEditor.CodePanel) - 構文強調表示、行番号、入力補完、コンパイル用ツールバーを備えたソースコードエディタです。戦略、インジケータ、スクリプトなど、アプリケーション内でコードを編集するあらゆる場面で使われます。

**主なプロパティ**

- [CodePanel.Code](xref:StockSharp.Xaml.CodeEditor.CodePanel.Code) - 編集中のコードとそのコンパイル状態。
- [CodePanel.ReadOnly](xref:StockSharp.Xaml.CodeEditor.CodePanel.ReadOnly) - テキスト変更の禁止。
- [CodePanel.ShowToolBar](xref:StockSharp.Xaml.CodeEditor.CodePanel.ShowToolBar) - ツールバーの表示。
- [CodePanel.AutoCompile](xref:StockSharp.Xaml.CodeEditor.CodePanel.AutoCompile) - 編集後の自動コンパイル。

コンパイルエラーはエディタ自身が表示しません。隣に [ErrorsGrid](xref:StockSharp.Xaml.Code.ErrorsGrid) テーブルを置いて表示し、ダブルクリックで該当行へ移動させると便利です。

以下は使用例のコード断片です:

```xaml
<Window x:Class="Sample.CodeWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:code="clr-namespace:StockSharp.Xaml.CodeEditor;assembly=StockSharp.Xaml.CodeEditor"
	Height="600" Width="900">
	<code:CodePanel x:Name="CodePanel" ShowToolBar="True" />
</Window>
```

```cs
// ソーステキストをエディタに読み込みます
CodePanel.Code = new CodeInfo
{
	Text = File.ReadAllText("MyStrategy.cs"),
};

// コンパイル後にエラーを別のテーブルへ出力します
CodePanel.CompiledCode += () => ErrorsGrid.Errors.AddRange(CodePanel.Code.Errors);

// 編集せずに閲覧します
CodePanel.ReadOnly = true;
```

## 関連項目

[診断](../diagnostics.md)
