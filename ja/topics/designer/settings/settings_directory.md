# 設定ディレクトリ

[Designer](../../designer.md) では、次のディレクトリが重要です。

1. [Designer](../../designer.md) がインストールされているディレクトリ。このフォルダーから、**Designer.exe** を実行して [Designer](../../designer.md) を起動したり、**Designer.Update.exe** を実行して [Designer](../../designer.md) を更新したりできます。このディレクトリを削除すると [Designer](../../designer.md) は削除されますが、[Designer](../../designer.md) の設定は削除されません。

2. **Designer** の設定ディレクトリは、ユーザーのドキュメントフォルダー配下にあります: …\\StockSharp\\Designer\\ (例: c:\\Users\\User\\Documents\\StockSharp\\Designer\\)。このディレクトリを削除すると、すべての [Designer](../../designer.md) 設定が既定値にリセットされます。**作成済みのすべてのストラテジー、ダウンロード済み銘柄、および設定ディレクトリに保存されているその他の情報は破棄されます。**

![Designer Directory and edit the data manually 00](../../../images/designer_directory_and_edit_data_manually_00.png)

このディレクトリには、次のフォルダーとファイルが含まれます。

- **Compositions** は、[Schemas](../user_interface/schemas.md) パネルの **複合要素** フォルダーにあるすべてのブロックを XML ファイルとして保存します。このディレクトリからファイルを削除すると、対応する **複合要素** が [Schemas](../user_interface/schemas.md) パネルの **複合要素** フォルダーから削除されます。これらのファイルを手動で編集しないでください。編集すると、対応する **複合要素** ブロックが破損する可能性があります。
- **LiveStrategies** は、[Schemas](../user_interface/schemas.md) パネルの **取引** フォルダーにあるすべてのブロックを XML ファイルとして保存します。このディレクトリからファイルを削除すると、対応するストラテジーが [Schemas](../user_interface/schemas.md) パネルの **取引** フォルダーから削除されます。これらのファイルを手動で編集しないでください。編集すると、対応するストラテジーが破損する可能性があります。
- **ログ** には、すべての [Designer](../../designer.md) クラッシュログが含まれ、[Designer](../../designer.md) のトラブルシューティングを簡単にします。
- **SourceCode** は、[Schemas](../user_interface/schemas.md) パネルの **ソースコード** フォルダーにあるすべてのブロックを XML ファイルとして保存します。このディレクトリからファイルを削除すると、**ソースコード** ブロックが [Schemas](../user_interface/schemas.md) パネルの **ソースコード** フォルダーから削除されます。これらのファイルを手動で編集しないでください。編集すると、対応する **ソースコード** ブロックが破損する可能性があります。
- **戦略** は、[Schemas](../user_interface/schemas.md) パネルの **戦略** フォルダーにあるすべてのブロックを XML ファイルとして保存します。このディレクトリからファイルを削除すると、対応するストラテジーが Schemas パネルの **戦略** フォルダーから削除されます。これらのファイルを手動で編集しないでください。編集すると、対応するストラテジーが破損する可能性があります。このフォルダーにストラテジーファイルを手動で追加して [Designer](../../designer.md) を再起動すると、そのストラテジーは [Schemas](../user_interface/schemas.md) パネルの **戦略** フォルダーに表示されます。
- **ストレージ** には、[Designer](../../designer.md) が対応する [マーケットデータストレージ](../market_data_storage.md) にダウンロードしたマーケットデータが含まれます。このフォルダーは [マーケットデータストレージ](../market_data_storage.md) の作成時に作成され、既定のパスはこのフォルダーを指します。このフォルダーを削除すると、対応するストレージからすべてのダウンロード済みマーケットデータが削除されます。ストレージに CSV ファイルが含まれている場合、それらは標準のテキストエディターまたは MS Excel で編集できます。BIN ファイルを手動で編集することはできません。
- **exchange.csv** と **exchangeboard.csv** には、**取引所**、銘柄コード、および取引モードのリストが含まれます。これらのファイルは、標準のテキストエディターまたは MS Excel で編集できます。
- **security.csv** には、すべてのソースを通じて受信および作成されたすべての銘柄が含まれます。このファイルを削除すると、[Designer](../../designer.md) からすべての銘柄が削除されます。新しい銘柄の追加については、[銘柄のダウンロード](../market_data_storage/download_instruments.md) と [銘柄の作成](../market_data_storage/create_instrument.md) で説明されています。このファイルは、標準のテキストエディターまたは MS Excel で編集できます。
- **portfolio.csv** と **position.csv** には、受信および作成されたすべてのポートフォリオと、その現在のポジションが含まれます。これらのファイルを削除すると、対応するデータが [Designer](../../designer.md) から削除されます。[Designer](../../designer.md) が各接続でポートフォリオ情報を受信している場合でも、ポジション情報は恒久的に失われる可能性があります。これらのファイルは、標準のテキストエディターまたは MS Excel で編集できます。
- **settings.json** には現在の設定が含まれます。[Designer](../../designer.md) は、設定が変更されたとき、またはプログラムが終了するときにこのファイルを作成します。このファイルを削除すると、現在の設定が既定値にリセットされます。このファイルを手動で編集しないでください。編集すると、[Designer](../../designer.md) が破損する可能性があります。

個別のファイルを手動で編集する前、または [Designer](../../designer.md) 設定をリセットする前に、変更するファイルまたはディレクトリ全体のバックアップコピーを作成してください。

## 関連項目

[新しいバージョンへの更新](../update_to_the_new_version.md)
