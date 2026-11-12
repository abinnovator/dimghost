# Godot対応バージョン — 実測結果

Kominka Modular Home 1.2.0 / Lite 24・Full 58 / macOS / Compatibility renderer

| Godot（実行した版） | Lite | Full |
|---|---|---|
| 4.2.2 | FAIL | FAIL |
| 4.3 | PASS_GUI | PASS_GUI |
| 4.4.1 | RUNTIME_PASS_IMPORT_WARNING | RUNTIME_PASS_IMPORT_WARNING |
| 4.5.2 | PASS | PASS |
| 4.6 | PASS | PASS |
| 4.6.3 | PASS | PASS |
| 4.7.2 | PASS | PASS |

PASS：空のGodotプロジェクトへのインポート、全アセットのメッシュ・PBR画像・衝突形状、3デモ、入口の通過・柱の衝突検査を完了。

PASS_GUI：ヘッドレス検査の表示処理でエラーが出たため、画面を開いたCompatibility rendererで全実行検査を再確認しました。4.3はこの方法で全アセット・3デモ・物理検査を通過。通常の画面あり実行で使用できますが、ヘッドレスでのデモ実行には制限があります。

RUNTIME_PASS_IMPORT_WARNING：4.4.1はインポート時にヘッドレス描画のエラーが出ました。空キャッシュからのGUIインポートでもエディタ終了時に描画スレッドのエラーが残るため、クリーンなインポート成功とはしません。生成されたデータの全アセット・3デモ・物理検査はGUI実行でも通過。日常の制作にはPASSの版を推奨します。

FAIL：インポートまたは実行検査が失敗。対応確認済みには含めません。4.2.2の詳細は制作側のインポートログに記録しています。

実際に試した上記の版のみを動作確認済みとします。Godot 3.x、4.0/4.1、表にない版、他OS、Mobile/Forward+、エクスポートしたゲームは未検証。使用例PNGはGodot 4.6のCompatibility rendererで実描画しています。

推奨：今回の実描画にも使ったGodot 4.6。ほかの版は上表の結果と検証方法を参照してください。

バイナリの完全なバージョン、入手元、ダウンロードSHA-256、ログ、検証データ指紋はbuild/GODOT_COMPATIBILITY.jsonに記録。公式配布元：[Godot archive](https://godotengine.org/download/archive/)。

追加6点を含む今回の再検証：既存GLBはSHA-256で同一性を確認した版別キャッシュを再利用し、新規GLBを追加インポートして全種類・3デモ・入口・井戸・池・橋・手水鉢・かまど・のれん・竹籠の物理検査を再実行。Godot 4.6では、本体ZIPを空の別プロジェクトへ導入し、追加ZIPを重ねる実際の導入手順も検証。
