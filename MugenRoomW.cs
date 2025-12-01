using Microsoft.VisualBasic.Logging;
using System;
using System.Reflection.Metadata.Ecma335;

namespace MugenRNG
{
    public class MugenRoomW
    {
        public (int[] ValTotal , uint GateTrainerVal) GenerateRoomW(MugenFloor floor, ulong Seed2, ushort[,] TIDs, int GateFloor, uint GateRand, int BossFloor)
        {
            byte[,] RoomIDs =
            {
                { 0x02,0x02,0x09,0x09,0x09,0x0A,0x0A,0x0A,0x0D,0x0D,0x11,0x11,0x12,0x13,0x13,0x13 },
                { 0x00,0x00,0x00,0x02,0x02,0x0B,0x0B,0x0B,0x0E,0x0E,0x11,0x11,0x12,0x13,0x13,0x13 },
                { 0x02,0x02,0x06,0x06,0x06,0x09,0x09,0x09,0x0E,0x0E,0x11,0x11,0x12,0x13,0x13,0x13 },
                { 0x02,0x02,0x07,0x07,0x07,0x0A,0x0A,0x0A,0x0C,0x0C,0x11,0x11,0x12,0x13,0x13,0x13 }
            };

            int[] ValTotal = new int[4];
            List<int> TIDRoom = new List<int>();
            List<int> TIDVal = new List<int>();
            uint GateTrainerRand = 0;
            uint GateTrainerVal = 0;


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
                Console.WriteLine($"Rand1:{Seed2:X16}");
                for (int i = 0; i < 15; i++)
                {
                    if (z == 0 && i == 0) ;
                    else Seed2 = NextSeed(Seed2);

                    ulong swapIndex = (((Seed2 >> 32) * 15) >> 32);

                    byte tmp = data[i];
                    data[i] = data[swapIndex];
                    data[swapIndex] = tmp;
                }

                byte tempID = 0x0;

                if (z == BossFloor) //ボス部屋の下
                {
                    Seed2 = NextSeed(Seed2);
                    ulong Rand = ((Seed2 >> 32) * 4) >> 32;
                    tempID = tempIDs[z, Rand];
                }

                Console.WriteLine($"Rand2:{Seed2:X16}");
                Console.WriteLine("Shuffled data: " + string.Join(", ", data.Select(b => $"{b:X2}")));

                uint index = 0;
                int TIDindex = 0;
                int FloorVal = 0;
                int tempIndex = 0;
                int tempVal = -1;

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
                            tempIndex++;
                            continue;
                        }
                        if (floor.Rooms[y, x, z].Skip)
                        {
                            byte n = tempID;
                            if (n == 0x13) floor.Rooms[y, x, z].Val = 0;
                            else if (n == 0x01 || n == 0x7 || n == 0x9 || n == 0xB) floor.Rooms[y, x, z].Val = 2;
                            else if (n == 0x02 || n == 0xC || n == 0xD || n == 0xE) floor.Rooms[y, x, z].Val = 3;
                            else floor.Rooms[y, x, z].Val = 1;
                        }
                        if (x == 0 && y == 0)
                        {
                            floor.Rooms[y, x, z].RoomID = 0xFF;
                            floor.Rooms[y, x, z].Val = 0;
                            tempIndex++;
                            continue;
                        }
                        else
                        {
                            if (!floor.Rooms[y, x, z].Skip)
                            {
                                floor.Rooms[y, x, z].RoomID = data[index];

                                byte n = data[index];
                                if (n == 0x13) floor.Rooms[y, x, z].Val = 0;
                                else if (n == 0x01 || n == 0x7 || n == 0x9 || n == 0xB) floor.Rooms[y, x, z].Val = 2;
                                else if (n == 0x02 || n == 0xC || n == 0xD || n == 0xE) floor.Rooms[y, x, z].Val = 3;
                                else floor.Rooms[y, x, z].Val = 1;

                                if (0x1 < data[index] && data[index] < 0xF && z == GateFloor)
                                {
                                    for (int i = 0; i < floor.Rooms[y, x, z].Val; i++)
                                    {
                                        TIDRoom.Add(tempIndex);
                                        Console.WriteLine($"Add.{tempIndex:X1}, Val.{floor.Rooms[y, x, z].Val}");
                                    }

                                    for (int i = floor.Rooms[y, x, z].Val; i > 0; i--)
                                    {
                                        TIDVal.Add(tempVal + i);
                                    }
                                }
                                tempVal += floor.Rooms[y, x, z].Val;
                            }
                            else tempVal += floor.Rooms[y, x, z].Val;

                            tempIndex++;
                            if (!floor.Rooms[y, x, z].Skip) index++;
                        }

                        FloorVal += floor.Rooms[y, x, z].Val;

                        for (int i = 0; i < floor.Rooms[y, x, z].Val; i++)
                        {
                            ushort TID = TIDs[z, TIDindex];
                            floor.Rooms[y, x, z].Trainer.Add(TID);
                            TIDindex++;
                        }
                    }
                }

                if (z == GateFloor)
                {
                    int Rooms = 0;
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            ulong temp = (ulong)GateRand * (ulong)(TIDRoom.Count);

                            GateTrainerRand = (uint)TIDRoom[(int)(temp >> 32)];

                            if (Rooms == GateTrainerRand)
                            {
                                floor.Rooms[y, x, z].GateTrainer = true;
                                GateTrainerVal = (uint)TIDVal[(int)(temp >> 32)];
                            }
                            Rooms++;
                        }
                    }
                }
                ValTotal[z] = FloorVal;
            }
            return (ValTotal, GateTrainerVal);
        }

        private ulong NextSeed(ulong Seed2)
        {
            return Seed2 * 0x5D588B656C078965UL + 0x269EC3UL;
        }
    }
}