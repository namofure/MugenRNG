using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace MugenRNG
{
    public partial class MainForm :Form
    {
        MugenFloor Generate;
        public MainForm()
        {
            InitializeComponent();
        }


        public void button1_Click(object sender, EventArgs e)
        {
            ulong Seed = 0x4B079738;

            Generate = new MugenFloor();
            Generate.Generate(Seed);



            Mapping Mapper = new Mapping();

            pictureBox1.Image = Mapper.Drawing(Generate, 0);
            pictureBox2.Image = Mapper.Drawing(Generate, 1);
            pictureBox3.Image = Mapper.Drawing(Generate, 2);
            pictureBox4.Image = Mapper.Drawing(Generate, 3);
        }
    }
}
