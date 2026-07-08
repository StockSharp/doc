# ストラテジーデザイナー

ストラテジーとその構成要素を設計する主な作業は、ブロックと接続線を組み合わせることで **Scheme** パネル上で行います。Scheme パネルは、**Palette**、**Designer**、**Properties** の各パネルで構成されています。

![Designer Designer schemes strategies and component elements 00](../../../../images/designer_designer_schemes_strategies_and_component_elements_00.png)

## Palette パネル

**Palette** パネルには、ストラテジーの作成に使用するブロックが含まれています。パレット内のすべての要素はカテゴリに分けられており、[ブロックの説明](elements.md)セクションで説明されています。ブロックを **Designer** パネルに追加するには、必要なブロックを右クリックし、ボタンを放さずに **Designer** パネルへドラッグします。その後、その要素が自動的に選択され、ブロックのプロパティを編集するウィンドウにそのパラメーターが表示されます。

## Designer パネル

**Designer** パネルでは、ブロックと接続（線）を組み合わせることで、ストラテジー作成の全プロセスを行います。ここにはストラテジーのスキームが視覚的に表示されます。ストラテジーの作成に関する詳細は、[ブロックからアルゴリズムを作成する](first_strategy.md)セクションで説明されています。

## Properties パネル

**Properties** パネルには、**Designer** パネルで選択したブロックのパラメーターが表示されます。**Designer** パネルでブロックを選択すると、その枠が黒色になります。

![Designer The Properties Panel 00](../../../../images/designer_properties_panel_00.png)

**Properties** パネルは、*Basic settings* と *Advanced settings* の 2 つのモードで表示できます。

デフォルトでは、スキームの構築時に、プロパティは最初に *Basic settings* モードで表示されます。*Advanced settings* モードに切り替えるには、対応するタイトルをクリックする必要があります。

*Basic settings* モードでは、ブロックの最も必要なプロパティだけが表示されます。たとえば、[candles](elements/data_sources/candles.md) ブロックでは、タイムフレーム、形成済みローソク足のみを受信するためのフラグ、より小さいタイムフレームからローソク足を構築できるかどうかのフラグ、およびシグナル時にローソク足を購読するためのフラグが表示されます。

*Advanced settings* モードでは、変更および設定が可能なブロックのすべてのプロパティが表示されます。

![Designer The Properties Panel 00](../../../../images/designer_properties_panel_01.png)

すべてのブロックには、*Advanced settings* モードで表示される定義済みプロパティのセットがあります。

- **Name** - デザイナーに表示される要素の名前。
- **Logging level** - この要素のログ記録レベル。
- **Parameters** - 上位レベルの要素に要素のパラメーターを表示します。
- **Sockets** - 上位レベルの要素に要素のソケットを表示します。

各ブロックのプロパティに関する詳細は、[ブロックの説明](elements.md)セクションで説明されています。

## 関連項目

[ブロックの説明](elements.md)

