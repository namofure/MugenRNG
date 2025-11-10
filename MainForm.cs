using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

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


        public void button1_Click(object sender, EventArgs e)
        {
            ulong Seed1 = 0x1;
            ulong Seed2 = 0x2;
            ulong InSeed = 0x0;
            uint Seed = 0x0;

            try
            {
                InSeed = Convert.ToUInt64(textBox1.Text, 16);
                Seed = (uint)(InSeed >> 32);

                MTSeed MTSeed = new MTSeed(Seed);
                (Seed1, Seed2) = MTSeed.UnovaRNG();
            }
            catch (Exception)
            {
                MessageBox.Show($"èâä˙SEEDÇ™ïsê≥Ç≈Ç∑ÅI");
            }

            if (comboBox1.SelectedIndex == 0) //çïÇÃñÄìVòO
            {
                TrainerB trainerB = new TrainerB();
                var (TIDs, GateFloor, GateRand) = trainerB.GenerateTrainerB(Seed1); //TIDê∂ê¨

                MugenFloor Floor = new MugenFloor();
                (Seed2, int BossFloor) = Floor.GenerateFloor(Seed2); //í òHê∂ê¨
                Console.WriteLine($"Seed2 = 0x{Seed2:X8}");

                MugenRoomB Room = new MugenRoomB();
                var ValTotal = Room.GenerateRoomB(Floor, Seed2, TIDs, GateFloor, GateRand, BossFloor); //ïîâÆê∂ê¨

                MappingB Mapper = new MappingB(); //ï`âÊ
                pictureBox1.Image = Mapper.Drawing(Floor, 0);
                pictureBox2.Image = Mapper.Drawing(Floor, 1);
                pictureBox3.Image = Mapper.Drawing(Floor, 2);
                pictureBox4.Image = Mapper.Drawing(Floor, 3);

                TIDdisplay display = new TIDdisplay();
                display.Display(dataGridView1, TIDs, 0, ValTotal, GateFloor, GateRand);
                display.Display(dataGridView2, TIDs, 1, ValTotal, GateFloor, GateRand);
                display.Display(dataGridView3, TIDs, 2, ValTotal, GateFloor, GateRand);
                display.Display(dataGridView4, TIDs, 3, ValTotal, GateFloor, GateRand);
            }
            else if (comboBox1.SelectedIndex == 1) //îíÇÃé˜ì¥
            {
                TrainerW trainerW = new TrainerW();
                var (TIDs, GateFloor, GateRand) = trainerW.GenerateTrainerW(Seed1); //TIDê∂ê¨

                MugenFloor Floor = new MugenFloor();
                (Seed2, int BossFloor) = Floor.GenerateFloor(Seed2); //í òHê∂ê¨

                MugenRoomW Room = new MugenRoomW();
                var ValTotal = Room.GenerateRoomW(Floor, Seed2, TIDs, GateFloor, GateRand, BossFloor); //ïîâÆê∂ê¨

                MappingW Mapper = new MappingW(); //ï`âÊ
                pictureBox1.Image = Mapper.Drawing(Floor, 0);
                pictureBox2.Image = Mapper.Drawing(Floor, 1);
                pictureBox3.Image = Mapper.Drawing(Floor, 2);
                pictureBox4.Image = Mapper.Drawing(Floor, 3);

                TIDdisplay display = new TIDdisplay();
                display.Display(dataGridView1, TIDs, 0, ValTotal, GateFloor, GateRand);
                display.Display(dataGridView2, TIDs, 1, ValTotal, GateFloor, GateRand);
                display.Display(dataGridView3, TIDs, 2, ValTotal, GateFloor, GateRand);
                display.Display(dataGridView4, TIDs, 3, ValTotal, GateFloor, GateRand);
            }
        }
    }
}
