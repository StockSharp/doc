# S# グラフィカルコンポーネントのテーマ

すべての S# グラフィカル要素には、複数の異なるテーマがあります。以下は、最もよく使用される 2 つのテーマです。

![API GUI テーマ 01](../../../images/api_gui_thems_01.png)

![API GUI テーマ 02](../../../images/api_gui_thems_02.png)

アプリケーションテーマをインストールするには、1 行記述するだけです。たとえば、VisualStudio 2017 のダークテーマを設定するには、次の行を指定する必要があります。

```cs
...
ThemeExtensions.ApplyDefaultTheme();
...
```

すべての S# グラフィカル要素は **DevExpress** グラフィカル要素に基づいているため、適切な **DevExpress** ライブラリ（**DevExpress.Xpf.Core**、**DevExpress.Xpf.Themes.VS2017Dark** など）を追加する必要があります。
