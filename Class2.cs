using System.Drawing;

namespace MugenRNG
{
    public class Mapping
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
                        DrawRoom(g, x, y, z, R);
                    }
                }

            }
            return bmp;
        }

        public void DrawRoom(Graphics g, int x, int y, int z, Room R)
        {
            //(00)(01)(02)(03)
            //(10)(11)(12)(13)

            //(1)
            //(1)(9)(1)(9)(1)(9)(1)(9)(1)
            int BaseX = x * (FloorSize + 1) + 1;//各フロアの(0,0)
            int BaseY = y * (FloorSize + 1) + 1;

            Color color = Color.White;
            if (x == 0 && y == 0) color = Color.Blue;
            if (R.Boss) color = Color.Red;

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

                if (R.North)
                {
                    Rectangle pathRect = new Rectangle(
                        PathX,
                        PathY - (5 * CellSize),
                        10,
                        10
                    );
                    if(Flag == true) g.FillRectangle(b, pathRect);
                }

                if (R.South)
                {
                    Rectangle pathRect = new Rectangle(
                        PathX,
                        PathY + (5 * CellSize),
                        10,
                        10
                    );
                    if (Flag == true) g.FillRectangle(b, pathRect);
                }

                if (R.East)
                {
                    Rectangle pathRect = new Rectangle(
                        PathX + (5 * CellSize),
                        PathY,
                        10,
                        10
                    );
                    if (Flag == true) g.FillRectangle(b, pathRect);
                }

                if (R.West)
                {
                    Rectangle pathRect = new Rectangle(
                        PathX - (5 * CellSize),
                        PathY,
                        10,
                        10
                    );
                    if (Flag == true) g.FillRectangle(b, pathRect);
                }
            }
        }
    }
}



        
    

