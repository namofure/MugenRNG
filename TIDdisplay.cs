using System.Security.Cryptography;
using System.Windows.Forms;
using System.Drawing;

namespace MugenRNG
{
    internal class TIDdisplay
    {
        public void Display(DataGridView dgv, ushort[,] TIDs, int z, int[] ValTotal, int GateFloor, uint GateTrainerRand)
        {
            dgv.Rows.Clear();
            dgv.Columns.Clear();
            dgv.SuspendLayout();

            dgv.Columns.Add("TID", "ID");
            dgv.Columns.Add("Name", "トレーナー");

            for (int i = 0; i < ValTotal[z]; i++)
            {
                ushort TID = TIDs[z, i];
                string IDhex = $"0x{TID:X4}";
                string name = TrainerName(TID);

                int rowIndex = dgv.Rows.Add(IDhex, name);

                if (z == GateFloor && i == GateTrainerRand)
                {
                    dgv.Rows[rowIndex].Cells[0].Style.BackColor = Color.Yellow;
                    dgv.Rows[rowIndex].Cells[1].Style.BackColor = Color.Yellow;
                }
            }

            dgv.ResumeLayout();
        }

        private string TrainerName(ushort TID)
        {
            switch (TID)
            {
                case 0x1E0: return "だいすきクラブのイグナシオ";
                case 0x1E1: return "だいすきクラブのリベルト";
                case 0x1E2: return "だいすきクラブのマリノ";
                case 0x1E3: return "だいすきクラブのドナ";
                case 0x1E4: return "だいすきクラブのダルシー";
                case 0x1E5: return "だいすきクラブのイーディス";
                case 0x1E6: return "ベーカリーのエリノア";
                case 0x1E7: return "ベーカリーのイライザ";
                case 0x1E8: return "ほいくしのエステル";
                case 0x1E9: return "ほいくしのユーニス";
                case 0x1EA: return "カラテおうのイダル";
                case 0x1EB: return "カラテおうのイソク";
                case 0x1EC: return "カラテおうのジェハ";
                case 0x1ED: return "バトルガールのウルジェ";
                case 0x1EE: return "バトルガールのギュリ";
                case 0x1EF: return "バトルガールのヘイン";
                case 0x1F0: return "スキンヘッズのコルト";
                case 0x1F1: return "スキンヘッズのダグマル";
                case 0x1F2: return "おまわりさんのダニロ";
                case 0x1F3: return "おまわりさんのデーゲン";
                case 0x1F4: return "やまおとこのダリウス";
                case 0x1F5: return "やまおとこのデニス";
                case 0x1F6: return "やまおとこのデトレフ";
                case 0x1F7: return "さぎょういんのエッカルト";
                case 0x1F8: return "さぎょういんのエメリッヒ";
                case 0x1F9: return "さぎょういんのフラッツ";
                case 0x1FA: return "せいそういんのゲルティ";
                case 0x1FB: return "せいそういんのアマデオ";
                case 0x1FC: return "さぎょういんのアメリゴ";
                case 0x1FD: return "さぎょういんのアンドラ";
                case 0x1FE: return "サイキッカーのバルトロ";
                case 0x1FF: return "サイキッカーのブラスコ";
                case 0x200: return "サイキッカーのドナテロ";
                case 0x201: return "サイキッカーのフィオナ";
                case 0x202: return "サイキッカーのジェマ";
                case 0x203: return "サイキッカーのジンジャー";
                case 0x204: return "けんきゅういんのエツィオ";
                case 0x205: return "けんきゅういんのファビウス";
                case 0x206: return "けんきゅういんのグレタ";
                case 0x207: return "けんきゅういんのヘティー";
                case 0x208: return "じゅくがえりのフロリード";
                case 0x209: return "じゅくがえりのジョット";
                case 0x20A: return "じゅくがえりのジュスト";
                case 0x20B: return "じゅくがえりのイングリ";
                case 0x20C: return "じゅくがえりのジョスリン";
                case 0x20D: return "じゅくがえりのマチカ";
                case 0x20E: return "ウエーターのグッチオ";
                case 0x20F: return "ウエーターのハーバート";
                case 0x210: return "ウエートレスのケイティ";
                case 0x211: return "ウエートレスのキンバリー";
                case 0x212: return "たんぱんこぞうのフリスト";
                case 0x213: return "たんぱんこぞうのイゴール";
                case 0x214: return "たんぱんこぞうのカレル";
                case 0x215: return "ミニスカートのラナ";
                case 0x216: return "ミニスカートのレスリー";
                case 0x217: return "ミニスカートのリビー";
                case 0x218: return "バックパッカーのキーン";
                case 0x219: return "バックパッカーのキム";
                case 0x21A: return "バックパッカーのロレッタ";
                case 0x21B: return "バックパッカーのロッティ";
                case 0x21C: return "だいすきクラブのランド";
                case 0x21D: return "だいすきクラブのラウロ";
                case 0x21E: return "だいすきクラブのリオネロ";
                case 0x21F: return "だいすきクラブのマデリン";
                case 0x220: return "だいすきクラブのメイジー";
                case 0x221: return "だいすきクラブのマリアン";
                case 0x222: return "ベーカリーのマーサ";
                case 0x223: return "ベーカリーのマルティナ";
                case 0x224: return "ほいくしのメレディス";
                case 0x225: return "ほいくしのミラベル";
                case 0x226: return "カラテおうのセイゲン";
                case 0x227: return "カラテおうのゲンノスケ";
                case 0x228: return "カラテおうのザエモン";
                case 0x229: return "バトルガールのイク";
                case 0x22A: return "バトルガールのチカ";
                case 0x22B: return "バトルガールのミエ";
                case 0x22C: return "スキンヘッズのマーゾ";
                case 0x22D: return "スキンヘッズのマウリシオ";
                case 0x22E: return "おまわりさんのバティスト";
                case 0x22F: return "おまわりさんのバジル";
                case 0x230: return "やまおとこのベルトラン";
                case 0x231: return "やまおとこのダミアン";
                case 0x232: return "やまおとこのドニス";
                case 0x233: return "さぎょういんのデフロット";
                case 0x234: return "さぎょういんのエジット";
                case 0x235: return "さぎょういんのガエル";
                case 0x236: return "せいそういんのギデオン";
                case 0x237: return "せいそういんのギスラン";
                case 0x238: return "さぎょういんのエクトル";
                case 0x239: return "さぎょういんのエルマン";
                case 0x23A: return "サイキッカーのオノレ";
                case 0x23B: return "サイキッカーのオラス";
                case 0x23C: return "サイキッカーのユベール";
                case 0x23D: return "サイキッカーのモナ";
                case 0x23E: return "サイキッカーのマイラ";
                case 0x23F: return "サイキッカーのノーマ";
                case 0x240: return "けんきゅういんのジャコブ";
                case 0x241: return "けんきゅういんのジャンゴ";
                case 0x242: return "けんきゅういんのパメラ";
                case 0x243: return "けんきゅういんのパトリシア";
                case 0x244: return "じゅくがえりのジェフリー";
                case 0x245: return "じゅくがえりのトモユキ";
                case 0x246: return "じゅくがえりのミキヒサ";
                case 0x247: return "じゅくがえりのセイディ";
                case 0x248: return "じゅくがえりのナデシコ";
                case 0x249: return "じゅくがえりのオミナエ";
                case 0x24A: return "ウエーターのロイク";
                case 0x24B: return "ウエーターのリオネル";
                case 0x24C: return "ウエートレスのセラフィー";
                case 0x24D: return "ウエートレスのシャノン";
                case 0x24E: return "たんぱんこぞうのマルタン";
                case 0x24F: return "たんぱんこぞうのイサオ";
                case 0x250: return "たんぱんこぞうのジュンジ";
                case 0x251: return "ミニスカートのアーシュラ";
                case 0x252: return "ミニスカートのスミコ";
                case 0x253: return "ミニスカートのレンゲ";
                case 0x254: return "バックパッカーのアルフレド";
                case 0x255: return "バックパッカーのアモス";
                case 0x256: return "バックパッカーのタバサ";
                case 0x257: return "バックパッカーのヨランダ";
                case 0x258: return "ポケモンブリーダーのトシミチ";
                case 0x259: return "ポケモンブリーダーのナリアキラ";
                case 0x25A: return "ポケモンブリーダーのアリトモ";
                case 0x25B: return "ポケモンブリーダーのヨウドウ";
                case 0x25C: return "ポケモンブリーダーのタイスケ";
                case 0x25D: return "ポケモンブリーダーのタダマサ";
                case 0x25E: return "ポケモンブリーダーのタケアキ";
                case 0x25F: return "ポケモンブリーダーのケイタロウ";
                case 0x260: return "ポケモンブリーダーのムネミツ";
                case 0x261: return "ポケモンブリーダーのヤタロウ";
                case 0x262: return "ポケモンブリーダーのオトギリ";
                case 0x263: return "ポケモンブリーダーのクレマチス";
                case 0x264: return "ポケモンブリーダーのクロッカス";
                case 0x265: return "ポケモンブリーダーのコマクサ";
                case 0x266: return "ポケモンブリーダーのサザンカ";
                case 0x267: return "ポケモンブリーダーのジャスミン";
                case 0x268: return "ポケモンブリーダーのスイレン";
                case 0x269: return "ポケモンブリーダーのノバラ";
                case 0x26A: return "ポケモンブリーダーのベゴニア";
                case 0x26B: return "ポケモンブリーダーのホオズキ";
                case 0x26C: return "おぼっちゃまのアナトリー";
                case 0x26D: return "おぼっちゃまのアルカディ";
                case 0x26E: return "おぼっちゃまのフリップ";
                case 0x26F: return "おぼっちゃまのイルダー";
                case 0x270: return "おぼっちゃまのアイザック";
                case 0x271: return "おじょうさまのアドリアナ";
                case 0x272: return "おじょうさまのガリーナ";
                case 0x273: return "おじょうさまのイライダ";
                case 0x274: return "おじょうさまのユリアナ";
                case 0x275: return "おじょうさまのカテリーナ";
                case 0x276: return "ジェントルマンのカルル";
                case 0x277: return "ジェントルマンのマルセル";
                case 0x278: return "ジェントルマンのニコラス";
                case 0x279: return "ジェントルマンのルスタン";
                case 0x27A: return "ジェントルマンのレオニダス";
                case 0x27B: return "マダムのラリサ";
                case 0x27C: return "マダムのルーダ";
                case 0x27D: return "マダムのオリガ";
                case 0x27E: return "マダムのタマラ";
                case 0x27F: return "マダムのタチアナ";
                case 0x280: return "ドクターのワイアット";
                case 0x281: return "ドクターのゲンパク";
                case 0x282: return "ドクターのカダ";
                case 0x283: return "ドクターのドイル";
                case 0x284: return "ドクターのガタリ";
                case 0x285: return "ドクターのシュバイツ";
                case 0x286: return "ドクターのヘンジャク";
                case 0x287: return "ドクターのボンハン";
                case 0x288: return "ドクターのヒデオ";
                case 0x289: return "ドクターのオサム";
                case 0x28A: return "ナースのサヤ";
                case 0x28B: return "ナースのヤエ";
                case 0x28C: return "ナースのフローレン";
                case 0x28D: return "ナースのヘンダソン";
                case 0x28E: return "ナースのイズミ";
                case 0x28F: return "ナースのアイカ";
                case 0x290: return "ナースのユキエ";
                case 0x291: return "ナースのリリカ";
                case 0x292: return "ナースのコトコ";
                case 0x293: return "ナースのヒルクレス";

                default: return "（不明）"; //デバック
            }
        }
    }
}