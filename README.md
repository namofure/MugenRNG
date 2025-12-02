# MugenRNG
MugenRNGはイッシュの難関攻略・乱数調整用のツールです.

64bitの性格値乱数初期SEEDを入れると, 
 - エリア配列
 - 通路の有無
 - トレーナー配列
 - ゲートトレーナーの配置

を返します.

黒の摩天楼/白の樹洞に対応しています.

ドクターの上書き処理は未実装です.

## 5genSeedUnti
BW2初期SEED検索の簡易版です.

UIの入力フォームの情報を元に, イッシュの難関最速クリアが可能な初期SEEDを検索します.

使い方は5genSearchの初期SEED検索とほぼ同じです.

ハッシュ化の処理は (https://github.com/yatsuna827/5genInitialSeedSearch) を ~~パクリ~~ 参考にしています.

## 動作環境
Microsoft .NET Runtime 8.0

## バージョン情報
- v1.1.0 各種処理の生成, 初期SEED検索の実装
- v1.0.0 初版

## 製作者
ジラーテ(@namofure)
