using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MugenRNG
{
    public partial class MainForm :Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();


        public MainForm()
        {
            InitializeComponent();
            AllocConsole();
        }


        public void button1_Click(object sender, EventArgs e)
        {
            ulong Seed = 0x4B079738;

            MugenFloor Floor = new MugenFloor();
            Seed = Floor.GenerateFloor(Seed);

            MugenRoomB Room = new MugenRoomB();
            Room.GenerateRoomB(Floor, Seed);

            Mapping Mapper = new Mapping();

            pictureBox1.Image = Mapper.Drawing(Floor, 0);
            pictureBox2.Image = Mapper.Drawing(Floor, 1);
            pictureBox3.Image = Mapper.Drawing(Floor, 2);
            pictureBox4.Image = Mapper.Drawing(Floor, 3);


        }
    }
}
