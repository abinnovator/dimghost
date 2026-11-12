# Kominka Modular Home Lite

Version 1.2.0 / 24種類 / 古民家の室内・外観を組み立てるゲーム用アセット

無料版は24種類。柱・梁・壁・建具・畳・板床・瓦屋根・縁側・入口・基本家具で、室内と外観を作れます。

## 導入

商品フォルダ内の `addons/` をGodotプロジェクト直下にコピーし、インポート後、`addons/kominka_modular_home/demo/Exterior.tscn` を開いてF6で実行してください。
`scenes/*.tscn` はすべてNode3Dルート・モデル・StaticBody3D・CollisionShape3Dを持つ配置用シーンです。
**対応バージョンの実測結果は同梱のCOMPATIBILITY.mdを参照。**

マウスドラッグ：回転 / ホイール：拡大縮小 / R：リセット / H：屋根・天井の表示切替 / Tab：使用例の切替。
詳細な組立座標・使用例・制限はASSEMBLY_GUIDE.mdに記載しています。

## 収録形式

GLB（PBR画像内包）/ Godot配置シーン・静的コリジョン / 3デモ / 説明書・MIT LICENSE。

BlenderはZ-up・1 unit=1m、Godot/glTFはY-up。PBR Base ColorはsRGB、Roughness/Metallic/Normalは線形データ、NormalはOpenGL +Y。主素材は2K、小物の鉄・陶器・灰・掛け軸は1Kです。AOは未ベイク。アセットごとの寸法・ポリゴン数・ライセンス・検査結果はmanifest.jsonに記録しています。

Full/Liteは同じ `res://addons/kominka_modular_home/` を使用します。一方だけ導入してください。別シリーズのmodern_japanese_home_propsとは共存できます。

## Inventory

| ID | Asset | Triangles | License |
|---|---|---:|---|
| beam | Timber beam · 1.8m | 188 | MIT |
| ceiling | Ceiling panel · 1.8m | 752 | MIT |
| corner_post | Corner post with braces | 564 | MIT |
| door_wood | Wood entrance door | 3,016 | MIT |
| doorway | Open doorway · 1.8m | 940 | MIT |
| engawa | Engawa deck · 1.8m | 3,384 | MIT |
| fence | Bamboo fence · 1.8m | 9,428 | MIT |
| floor_wood | Wood floor · 1.8m | 2,068 | MIT |
| foundation | Raised foundation · 1.8m | 1,316 | MIT |
| fusuma | Fusuma panel · 0.9m | 1,700 | MIT |
| gable | Gable infill · 5.4m | 1,324 | MIT |
| lattice_window | Lattice window wall | 3,760 | MIT |
| low_table | Low dining table | 1,316 | MIT |
| plaster_wall | Plaster wall · 1.8m | 940 | MIT |
| post | Timber post · 2.4m | 188 | MIT |
| roof_eave | Porch tiled canopy | 1,644 | MIT |
| roof_ridge | Ridge tiles · 1.8m | 3,228 | MIT |
| roof_slope | Tiled roof slope · 1.8m | 4,364 | MIT |
| shoji | Shoji panel · 0.9m | 2,444 | MIT |
| sill | Timber sill · 1.8m | 188 | MIT |
| step | Two-step entrance | 376 | MIT |
| stone_path | Stepping stone | 188 | MIT |
| tatami | Tatami · 0.9 × 1.8m | 564 | MIT |
| zabuton | Indigo floor cushion | 1,192 | MIT |

## License

収録モデル・素材・シーン・デモ・コード・説明書はすべてMIT。LICENSEの全文を保持してください。
