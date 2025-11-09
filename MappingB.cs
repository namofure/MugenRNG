using System.Drawing;

namespace MugenRNG
{
    public class MappingB
    {
        private const int FloorSize = 9;
        private const int MapSize = 41;
        private const int CellSize = 10;
        private const bool Flag = true;

        public Bitmap Drawing(MugenFloor floor, int z)
        {
            Bitmap bmp = new Bitmap(MapSize * CellSize, MapSize * CellSize);
            Graphics.FromImage(bmp).Clear(Color.Black);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        Room R = floor.Rooms[y, x, z];
                        byte Val = floor.Rooms[y, x, z].RoomID;
                        DrawFloor(g, x, y, z, R);
                        DrawRoom(g, x, y, z, Val, R);
                    }
                }
            }
            return bmp;
        }

        public void DrawFloor(Graphics g, int x, int y, int z, Room R)
        {
            int BaseX = x * (FloorSize + 1) + 1;//各フロアの(10,10)
            int BaseY = y * (FloorSize + 1) + 1;

            Color color = Color.White;
            if(R.GateTrainer == true) color = Color.Yellow;
            Color Pathcolor = Color.White;

            using (Brush b = new SolidBrush(color))
            {
                Rectangle RoomRect = new Rectangle(
                    BaseX * CellSize,
                    BaseY * CellSize,
                    FloorSize * CellSize,
                    FloorSize * CellSize
                );
                g.FillRectangle(b, RoomRect);

                int PathX = (BaseX + 4) * CellSize;
                int PathY = (BaseY + 4) * CellSize;


                using (Brush p = new SolidBrush(Pathcolor))
                {
                    if (R.North)
                    {
                        Rectangle pathRect = new Rectangle(
                            PathX,
                            PathY - (5 * CellSize),
                            10,
                            10
                        );
                        if (Flag == true) g.FillRectangle(p, pathRect);
                    }

                    if (R.South)
                    {
                        Rectangle pathRect = new Rectangle(
                            PathX,
                            PathY + (5 * CellSize),
                            10,
                            10
                        );
                        if (Flag == true) g.FillRectangle(p, pathRect);
                    }

                    if (R.East)
                    {
                        Rectangle pathRect = new Rectangle(
                            PathX + (5 * CellSize),
                            PathY,
                            10,
                            10
                        );
                        if (Flag == true) g.FillRectangle(p, pathRect);
                    }

                    if (R.West)
                    {
                        Rectangle pathRect = new Rectangle(
                            PathX - (5 * CellSize),
                            PathY,
                            10,
                            10
                        );
                        if (Flag == true) g.FillRectangle(p, pathRect);
                    }
                }
            }
        }
        public void DrawRoom(Graphics g, int x, int y, int z, byte Val, Room R)
        {
            if (Val == 0x13 || Val == 0xFF) return; //空

            int BaseX = x * (FloorSize + 1) + 1;    //各フロアの(0,0)
            int BaseY = y * (FloorSize + 1) + 1;

            if (Val == 0x01) //ボス部屋
            {
                using (Brush Brush = new SolidBrush(Color.Black))
                {
                    Rectangle All = new Rectangle(BaseX * CellSize, BaseY * CellSize, 90, 90);
                    g.FillRectangle(Brush, All);
                }
                using (Brush Brush = new SolidBrush(Color.Red))
                {
                    Rectangle Top = new Rectangle((BaseX + 1) * CellSize, (BaseY + 1) * CellSize, 70, 60);
                    g.FillRectangle(Brush, Top);

                    Rectangle Bottom = new Rectangle((BaseX + 3) * CellSize, (BaseY + 7) * CellSize, 30, 20);
                    g.FillRectangle(Brush, Bottom);
                }
                return;
            }

            using (Brush Brush = new SolidBrush(Color.Black))
            {
                Rectangle TopLeft = new Rectangle(BaseX * CellSize, BaseY * CellSize, 30, 30); //左上
                g.FillRectangle(Brush, TopLeft);

                Rectangle TopRight = new Rectangle((BaseX + 6) * CellSize, BaseY * CellSize, 30, 30); //右上
                g.FillRectangle(Brush, TopRight);

                Rectangle BottomLeft = new Rectangle(BaseX * CellSize, (BaseY + 6) * CellSize, 30, 30); //左下 
                g.FillRectangle(Brush, BottomLeft);

                Rectangle BottomRight = new Rectangle((BaseX + 6) * CellSize, (BaseY + 6) * CellSize, 30, 30); //右下
                g.FillRectangle(Brush, BottomRight);
            }

            Color color = Color.White;
            if (R.GateTrainer == true) color = Color.Yellow;
            using (Brush Brush = new SolidBrush(color))
            {
                Rectangle Center = new Rectangle((BaseX + 1) * CellSize, (BaseY + 1) * CellSize, 70, 70); //中心
                g.FillRectangle(Brush, Center);
            }

            if (Val == 0x0F) //フラッシュ部屋
            {
                int PathX = (BaseX + 4) * CellSize;
                int PathY = (BaseY + 4) * CellSize;

                using (Brush Brush = new SolidBrush(Color.Black))
                {
                    Rectangle Top = new Rectangle(PathX, PathY + 20, 10, 10);
                    g.FillRectangle(Brush, Top);

                    Rectangle Bottom = new Rectangle(PathX, PathY - 20, 10, 10);
                    g.FillRectangle(Brush, Bottom);

                    Rectangle Right = new Rectangle(PathX + 20, PathY, 10, 10);
                    g.FillRectangle(Brush, Right);

                    Rectangle Left = new Rectangle(PathX - 20, PathY, 10, 10);
                    g.FillRectangle(Brush, Left);
                }
                return;
            }

            if (Val == 0x10) //波乗り部屋
            {
                using (Brush Brush = new SolidBrush(Color.Black))
                {
                    Rectangle TopLeft = new Rectangle((BaseX + 1) * CellSize, (BaseY + 1) * CellSize, 30, 30); //左上
                    g.FillRectangle(Brush, TopLeft);

                    Rectangle TopRight = new Rectangle((BaseX + 5) * CellSize, (BaseY + 1) * CellSize, 30, 30); //右上
                    g.FillRectangle(Brush, TopRight);

                    Rectangle BottomLeft = new Rectangle((BaseX + 1) * CellSize, (BaseY + 5) * CellSize, 30, 30); //左下 
                    g.FillRectangle(Brush, BottomLeft);

                    Rectangle BottomRight = new Rectangle((BaseX + 5) * CellSize, (BaseY + 5) * CellSize, 30, 30); //右下
                    g.FillRectangle(Brush, BottomRight);
                }

                color = Color.White;
                if (R.GateTrainer == true) color = Color.Yellow;
                using (Brush Brush = new SolidBrush(color))
                {
                    Rectangle Center = new Rectangle((BaseX + 3) * CellSize, (BaseY + 3) * CellSize, 30, 30); //中心
                    g.FillRectangle(Brush, Center);
                }
                return;
            }
        }
    }
}



        
    

