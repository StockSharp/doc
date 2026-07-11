# 独自キューブの作成

[スキームからキューブを作成](../../using_visual_designer/composite_elements.md)する場合と同様に、Python コードに基づいて独自のキューブを作成できます。このようなキューブは、スキームから作成したキューブよりも高機能になります。

コードからキューブを作成するには、**カスタムブロック** フォルダー内に作成する必要があります。

![Designer ソースコード要素 00](../../../../../images/designer_source_code_elem_00.png)

次の例では、キューブは [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) クラスを継承し、次のようになります。

```python
import clr

# 必要な StockSharp アセンブリへの参照を追加
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Diagram.Core")

# .NET と StockSharp から必要な型をインポート
from System import Action
from StockSharp.Messages import Unit
from StockSharp.Messages import ICandleMessage
from StockSharp.Diagram import DiagramExternalElement

from designer_extensions import diagram_external

# 入力ソケットと出力ソケットの使用方法を示すカスタムダイアグラム要素クラス
class empty_diagram_element(DiagramExternalElement):
	"""
	入力ソケットと出力ソケットの使用方法を示すサンプルダイアグラム要素。

	https://doc.stocksharp.com/topics/designer/strategies/using_code/python/creating_your_own_cube.html
	"""

	def __init__(self):
		super(empty_diagram_element, self).__init__()

		# ダイアグラム要素にパラメーターを追加する方法を示すサンプルプロパティ
		# このパラメーターは "MinValue" という名前で、既定値は 10
		self._minValue = self.AddParam("MinValue", 10)\
							.SetBasic(True)\
							.SetDisplay("パラメーター", "最小値", "最小値パラメーターの説明", 10)

		# 出力イベントハンドラーを空のリストとして初期化
		# 購読者はこれらのハンドラーに呼び出し可能メソッドを割り当てることができる
		self._output1_handlers = []
		self._output2_handlers = []

	# 出力ソケットは DiagramExternal 属性でマークされたイベント

	@diagram_external
	def add_Output1(self, handler: Action[Unit]):
		"""
		Output1 イベントを購読します。

		:param handler: Output1 がトリガーされたときに呼び出される呼び出し可能メソッド。
		"""
		self._output1_handlers.append(handler)

	def remove_Output1(self, handler):
		"""
		Output1 イベントの購読を解除します。

		:param handler: Output1 の購読者から削除される呼び出し可能メソッド。
		"""
		if handler in self._output1_handlers:
			self._output1_handlers.remove(handler)

	@diagram_external
	def add_Output2(self, handler: Action[Unit]):
		"""
		Output2 イベントを購読します。

		:param handler: Output2 がトリガーされたときに呼び出される呼び出し可能メソッド。
		"""
		self._output2_handlers.append(handler)

	def remove_Output2(self, handler):
		"""
		Output2 イベントの購読を解除します。

		:param handler: Output2 の購読者から削除される呼び出し可能メソッド。
		"""
		if handler in self._output2_handlers:
			self._output2_handlers.remove(handler)

	# 新しい引数を受信するたびに Process メソッドを呼び出したい場合は、
	# 次のプロパティのコメントを解除します
	# （すべての入力引数を受信するまで待つ必要はありません）。
	#
	# @property
	# def WaitAllInput(self):
	#     return False

	@diagram_external
	def Process(self, candle: ICandleMessage, diff: Unit) -> None:
		"""
		入力ソケットは DiagramExternal 属性でマークされたメソッドパラメーターです。
		ローソク足と diff 値を処理し、ロジックに基づいて出力イベントを呼び出します。

		:param candle: ローソク足を表す CandleMessage 入力。
		:param diff: 処理対象の差分値を表す Unit。
		"""
		# ローソク足の終値と diff 値の合計として結果を計算
		res = candle.ClosePrice + diff

		# diff が MinValue パラメーター以上なら Output1 を呼び出し、
		# それ以外なら Output2 を呼び出す
		if diff >= self._minValue.Value:
			for handler in self._output1_handlers:
				handler(res)
		else:
			for handler in self._output2_handlers:
				handler(res)

	def Start(self):
		"""
		ダイアグラム要素の開始時に呼び出されます。必要な開始前ロジックをここに追加します。
		"""
		super(empty_diagram_element, self).Start()
		# 要素の開始前に実行するカスタムロジックを追加

	def Stop(self):
		"""
		ダイアグラム要素の停止時に呼び出されます。必要な停止後ロジックをここに追加します。
		"""
		super(empty_diagram_element, self).Stop()
		# 要素の停止後に実行するカスタムロジックを追加

	def Reset(self):
		"""
		ダイアグラム要素のリセット時に呼び出されます。必要なリセットロジックをここに追加します。
		"""
		super(empty_diagram_element, self).Reset()
		# 要素の内部状態をリセットするカスタムロジックを追加
```

このコードでは、キューブには 2 つの入力ソケットと 2 つの出力ソケットがあります。入力ソケットは、メソッドに @diagram_external デコレーターを適用して定義します。

```python
@diagram_external
def Process(self, candle: ICandleMessage, diff: Unit) -> None:
```

出力ソケットは、イベント（イベント購読操作 add_NNN）に @diagram_external デコレーターを適用して定義します。キューブの例では、このようなイベントが 2 つあります。

```python
# 出力ソケットは DiagramExternal 属性でマークされたイベント

@diagram_external
def add_Output1(self, handler: Action[Unit]):
	"""
	Output1 イベントを購読します。

	:param handler: Output1 がトリガーされたときに呼び出される呼び出し可能メソッド。
	"""
	self._output1_handlers.append(handler)

def remove_Output1(self, handler):
	"""
	Output1 イベントの購読を解除します。

	:param handler: Output1 の購読者から削除される呼び出し可能メソッド。
	"""
	if handler in self._output1_handlers:
		self._output1_handlers.remove(handler)

@diagram_external
def add_Output2(self, handler: Action[Unit]):
	"""
	Output2 イベントを購読します。

	:param handler: Output2 がトリガーされたときに呼び出される呼び出し可能メソッド。
	"""
	self._output2_handlers.append(handler)

def remove_Output2(self, handler):
	"""
	Output2 イベントの購読を解除します。

	:param handler: Output2 の購読者から削除される呼び出し可能メソッド。
	"""
	if handler in self._output2_handlers:
		self._output2_handlers.remove(handler)
```

したがって、出力ソケットも 2 つになります。

さらに、キューブのプロパティを作成する方法も示しています。

```python
self._minValue = self.AddParam("MinValue", 10)\
					.SetBasic(True)\
					.SetDisplay("パラメーター", "最小値", "最小値パラメーターの説明", 10)
```

[DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) クラスを使用すると、設定の保存と復元の仕組みが自動的に使用されます。

**最小値** プロパティは基本プロパティとしてマークされており、[基本プロパティ](../../using_visual_designer/diagram_panel.md) モードで表示されます。

コメントアウトされた [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) プロパティは、入力ソケットを持つメソッドがいつ呼び出されるかを決定します。

```python
# @property
# def WaitAllInput(self):
#     return False
```

このプロパティのコメントを解除すると、少なくとも 1 つの値が到着するたびに **プロセス** メソッドが呼び出されます（この例では、ローソク足または数値のどちらか）。

作成したキューブをスキームに追加するには、パレットの **カスタムブロック** セクションで作成済みのキューブを選択する必要があります。

![Designer ソースコード要素 01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING]
> Python コードのキューブは、Python コードで作成されたストラテジーでは使用できません。[キューブから](../../using_visual_designer.md)作成されたストラテジーでのみ使用できます。

## 関連項目

[インジケーターの作成](create_own_indicator.md)
