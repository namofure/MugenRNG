using Microsoft.VisualBasic.Logging;
using System;

namespace MugenRNG
{
    public class MugenRoomB
    {
        public  int[]  GenerateRoomB(MugenFloor floor, ulong Seed , ushort[,] TIDs, int GateFloor, int GateRand, int BossFloor)
        {
            byte [,] RoomIDs =
            {
                { 0x02,0x02,0x09,0x09,0x09,0x0A,0x0A,0x0A,0x0D,0x0D,0x0F,0x0F,0x10,0x13,0x13,0x13 },
                { 0x00,0x00,0x00,0x02,0x02,0x0B,0x0B,0x0B,0x0E,0x0E,0x0F,0x0F,0x10,0x13,0x13,0x13 },
                { 0x02,0x02,0x06,0x06,0x06,0x09,0x09,0x09,0x0E,0x0E,0x0F,0x0F,0x10,0x13,0x13,0x13 },
                { 0x02,0x02,0x07,0x07,0x07,0x0A,0x0A,0x0A,0x0C,0x0C,0x0F,0x0F,0x10,0x13,0x13,0x13 }
            };

            int[] ValTotal = new int[4];

            byte[,] tempIDs =
            {
                {0x02, 0x09, 0x0A, 0x0D },
                {0x00, 0x02, 0x0B, 0x0E },
                {0x02, 0x06, 0x09, 0x0E },
                {0x02, 0x07, 0x0A, 0x0C }
            };

            for (int z = 0; z < 4; z++)
            {
                byte[] data = new byte[16];
                for (int i = 0; i < 16; i++)
                    data[i] = RoomIDs[z, i];

                for (int i = 0; i < 15; i++)
                {
                    if (z == 0 && i == 0);
                    else Seed = NextSeed(Seed);

                    ulong swapIndex = (((Seed >> 32) * 15) >> 32);

                    byte tmp = data[i];
                    data[i] = data[swapIndex];
                    data[swapIndex] = tmp;
                }

                byte tempID = 0x0;

                if (z == BossFloor) //ボス部屋の下
                {
                    Seed = NextSeed(Seed);
                    ulong Rand = ((Seed >> 32 ) * 4 ) >> 32;
                    tempID = tempIDs[z, Rand];
                }             

                int index = 0;
                int TIDindex = 0;
                int FloorVal = 0;

                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (floor.Rooms[y, x, z].Boss)
                        {
                            floor.Rooms[y, x, z].RoomID = 0x01;
                            floor.Rooms[y, x, z].Val = 0;

                            floor.Rooms[y + 1, x, z].RoomID = tempID;
                            floor.Rooms[y + 1, x, z].Skip = true;
                            continue;
                        }
                        if (floor.Rooms[y, x, z].Skip == true) continue;

                        if (x == 0 && y == 0)
                        {
                            floor.Rooms[y, x, z].RoomID = 0xFF;
                            floor.Rooms[y, x, z].Val = 0;
                            continue;
                        }
                        else
                        {
                            floor.Rooms[y, x, z].RoomID = data[index];

                            byte n = data[index];
                            if (n == 0x13) floor.Rooms[y, x, z].Val = 0;
                            else if (n == 0x01 || n == 0x7 || n == 0x9 || n == 0xB) floor.Rooms[y, x, z].Val = 2;
                            else if (n == 0x02 || n == 0xC || n == 0xD || n == 0xE) floor.Rooms[y, x, z].Val = 3;
                            else floor.Rooms[y, x, z].Val = 1;

                            index++;
                        }

                        FloorVal += floor.Rooms[y, x, z].Val;

                        for (int i = 0; i < floor.Rooms[y, x, z].Val; i++)
                        {
                            ushort TID = TIDs[z,TIDindex];
                            floor.Rooms[y, x, z].Trainer.Add(TID);

                            if(z == GateFloor)
                            {
                                if (TIDindex == GateRand) floor.Rooms[y, x, z].GateTrainer = true;
                            }
                            
                            TIDindex++;
                        }
                    }
                }
                ValTotal[z] = FloorVal;

            }
            return ValTotal;
        }

        private ulong NextSeed(ulong Seed)
        {
            return Seed * 0x5D588B656C078965UL + 0x269EC3UL;
        }
    }
}

