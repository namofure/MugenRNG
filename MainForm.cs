using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace MugenRNG
{
    public partial class MainForm : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        public MainForm()
        {
            InitializeComponent();
            //AllocConsole();
        }

        //MugenRNG
        public void button1_Click(object sender, EventArgs e)
        {
            ulong Seed1 = 0x1;
            ulong Seed2 = 0x2;
            ulong InSeed = 0x0;
            uint Seed = 0x0;
            ulong temp = 0x0;

            try
            {
                InSeed = Convert.ToUInt64(textBox1.Text, 16);
                Seed = (uint)(InSeed >> 32);
                MTSeed MTSeed = new MTSeed(Seed);
                (temp, Seed1, Seed2) = MTSeed.UnovaRNG();

                Console.WriteLine($"Seed1:{Seed1:X16}, Seed2:{Seed2:X16}");


            }
            catch (Exception)
            {
                MessageBox.Show($"初期SEEDが不正です！");
            }

            if (comboBox1.SelectedIndex == 0) //黒の摩天楼
            {
                TrainerB trainerB = new TrainerB();
                var (TIDs, GateFloor, GateRand) = trainerB.GenerateTrainerB(Seed1); //TID生成

                MugenFloor Floor = new MugenFloor();
                (Seed2, int BossFloor) = Floor.GenerateFloor(Seed2); //通路生成


                MugenRoomB Room = new MugenRoomB();
                var (ValTotal, GateTrainerVal) = Room.GenerateRoomB(Floor, Seed2, TIDs, GateFloor, GateRand, BossFloor); //部屋生成

                MappingB Mapper = new MappingB(); //描画
                pictureBox1.Image = Mapper.Drawing(Floor, 0);
                pictureBox2.Image = Mapper.Drawing(Floor, 1);
                pictureBox3.Image = Mapper.Drawing(Floor, 2);
                pictureBox4.Image = Mapper.Drawing(Floor, 3);

                TIDdisplay display = new TIDdisplay();
                display.Display(dataGridView1, TIDs, 0, ValTotal, GateFloor, GateTrainerVal);
                display.Display(dataGridView2, TIDs, 1, ValTotal, GateFloor, GateTrainerVal);
                display.Display(dataGridView3, TIDs, 2, ValTotal, GateFloor, GateTrainerVal);
                display.Display(dataGridView4, TIDs, 3, ValTotal, GateFloor, GateTrainerVal);
            }
            else if (comboBox1.SelectedIndex == 1) //白の樹洞
            {
                TrainerW trainerW = new TrainerW();
                var (TIDs, GateFloor, GateRand) = trainerW.GenerateTrainerW(Seed1); //TID生成

                MugenFloor Floor = new MugenFloor();
                (Seed2, int BossFloor) = Floor.GenerateFloor(Seed2); //通路生成

                MugenRoomW Room = new MugenRoomW();
                var (ValTotal, GateTrainerVal) = Room.GenerateRoomW(Floor, Seed2, TIDs, GateFloor, GateRand, BossFloor); //部屋生成

                MappingW Mapper = new MappingW(); //描画
                pictureBox1.Image = Mapper.Drawing(Floor, 0);
                pictureBox2.Image = Mapper.Drawing(Floor, 1);
                pictureBox3.Image = Mapper.Drawing(Floor, 2);
                pictureBox4.Image = Mapper.Drawing(Floor, 3);

                TIDdisplay display = new TIDdisplay();
                display.Display(dataGridView1, TIDs, 0, ValTotal, GateFloor, GateTrainerVal);
                display.Display(dataGridView2, TIDs, 1, ValTotal, GateFloor, GateTrainerVal);
                display.Display(dataGridView3, TIDs, 2, ValTotal, GateFloor, GateTrainerVal);
                display.Display(dataGridView4, TIDs, 3, ValTotal, GateFloor, GateTrainerVal);
            }
        }

        //初期SEED検索
        private async void button2_Click(object sender, EventArgs e)
        {
            if (!GetDateTime(
                numericUpDown1, numericUpDown2, numericUpDown3,
                numericUpDown4, numericUpDown5, numericUpDown6,
                out DateTime InDateTime))
            {
                MessageBox.Show("検索範囲が不正です！");
                return;
            }

            if (!GetDateTime(
                numericUpDown7, numericUpDown8, numericUpDown9,
                numericUpDown10, numericUpDown11, numericUpDown12,
                out DateTime EnDateTime))
            {
                MessageBox.Show("検索範囲が不正です！");
                return;
            }

            if (InDateTime > EnDateTime)
            {
                MessageBox.Show("検索範囲が不正です！");
                return;
            }

            //-------------------------
            //Values[0] = Nazo1
            //Values[1] = Nazo2
            //Values[2] = Nazo3
            //Values[3] = Vount
            //Values[4] = InTimer0
            //Values[5] = EnTimer0 
            //Values[6] = Mac 上位
            //Values[7] = Mac 下位
            //Values[8] = GxFrame
            //Values[9] = Frame
            //Values[10] = Key 入力なし
            //Values[11] = Prm1
            //Values[12] = Prm2
            //Values[13] = Prm3
            //-------------------------

            var param = new PIDPrm();
            int Version = 0;
            int CountCheck = 0;
            if (checkBox7.Checked) CountCheck = 1;

            if (comboBox5.SelectedIndex == 0) Version = 0; //黒の摩天楼
            if (comboBox5.SelectedIndex == 1) Version = 1; //白の樹洞

            //nazo値
            if (comboBox3.SelectedIndex == 0) //JPB2
            {
                param.Values[0] = 0x0209A8DC;
                param.Values[1] = 0x02039AC9;
                param.Values[2] = 0x021FF9B0;
                param.Values[3] = 0x82;

                if (checkBox4.Checked)
                {
                    uint Timer0 = Convert.ToUInt32(textBox10.Text, 16);
                    param.Values[4] = Timer0;
                    param.Values[5] = Timer0;
                }
                else
                {
                    param.Values[4] = 0x1102;
                    param.Values[5] = 0x1108;
                }
            }
            else if (comboBox3.SelectedIndex == 1) //JPW2
            {
                param.Values[0] = 0x0209A8FC;
                param.Values[1] = 0x02039AF5;
                param.Values[2] = 0x021FF9D0;
                param.Values[3] = 0x82;

                if (checkBox4.Checked)
                {
                    uint Timer0 = Convert.ToUInt32(textBox10.Text, 16);
                    param.Values[4] = Timer0;
                    param.Values[5] = Timer0;
                }
                else
                {
                    param.Values[4] = 0x10F5;
                    param.Values[5] = 0x10FB;
                }
            }

            byte M1 = Convert.ToByte(textBox6.Text, 16);
            byte M2 = Convert.ToByte(textBox7.Text, 16);

            param.Values[6] = (uint)((M1 << 8) + M2);

            byte M3 = Convert.ToByte(textBox2.Text, 16);
            byte M4 = Convert.ToByte(textBox3.Text, 16);
            byte M5 = Convert.ToByte(textBox4.Text, 16);
            byte M6 = Convert.ToByte(textBox5.Text, 16);

            param.Values[7] = (uint)((M3 << 24) | (M4 << 16) | (M5 << 8) | M6);

            param.Values[8] = 0x06000000;
            param.Values[9] = 0x8;
            if (comboBox2.SelectedIndex == 2)
            {
                param.Values[9] = 0x6;
            }

            param.Values[10] = 0x00002FFF;
            param.Values[11] = 0x80000000;
            param.Values[12] = 0x00000000;
            param.Values[13] = 0x000001A0;

            if (checkBox6.Checked)
            {
                param.Increment = TimeSpan.FromHours(1);
            }
            else if (checkBox5.Checked)
            {
                param.Increment = TimeSpan.FromMinutes(1);
            }
            else
            {
                param.Increment = TimeSpan.FromSeconds(1); //デフォルト
            }

            param.InDt = InDateTime;
            param.EnDt = EnDateTime;

            SeedSearcher SeedSearch = new SeedSearcher(param, Version, CountCheck);

            cts = new CancellationTokenSource();　//CancellationTokenSourceってなに？
            await Task.Run(() => SeedSearch.GenerateSeeds(cts.Token, dataGridView5));
        }

        private bool GetDateTime(
            NumericUpDown year, NumericUpDown month, NumericUpDown day,
            NumericUpDown hour, NumericUpDown minute, NumericUpDown second,
            out DateTime result)
        {
            result = default;

            try
            {
                int y = (int)year.Value;
                int m = (int)month.Value;
                int d = (int)day.Value;
                int h = (int)hour.Value;
                int min = (int)minute.Value;
                int s = (int)second.Value;

                result = new DateTime(y, m, d, h, min, s);
                return true;
            }
            catch
            {
                // 不正
                return false;
            }
        }
        public class PIDPrm
        {
            public List<uint> Values { get; set; } = Enumerable.Repeat(0u, 14).ToList();
            public DateTime InDt { get; set; }
            public DateTime EnDt { get; set; }
            public TimeSpan Increment { get; set; }
        }

        private CancellationTokenSource cts; //処理をキャンセルするやつ？
        private void button3_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                textBox8.Text = "1102";
                textBox9.Text = "1108";
            }
            if (comboBox3.SelectedIndex == 1)
            {
                textBox8.Text = "10F5";
                textBox9.Text = "10FB";
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox4.Checked) textBox10.ReadOnly = true;
            else textBox10.ReadOnly = false;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked) checkBox6.Checked = false;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked) checkBox5.Checked = false;
        }
    }
}
