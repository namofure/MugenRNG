using Microsoft.VisualBasic.Logging;
using System;

namespace MugenRNG
{
    public class MugenRoomW
    {
        public ulong GenerateRoomW(MugenFloor floor, ulong Seed)
        {
            byte [,] RoomIDs =
            {
                { 0x02,0x02,0x09,0x09,0x09,0x0A,0x0A,0x0A,0x0D,0x0D,0x0F,0x0F,0x10,0x13,0x13,0x13 },
                { 0x00,0x00,0x00,0x02,0x02,0x0B,0x0B,0x0B,0x0E,0x0E,0x0F,0x0F,0x10,0x13,0x13,0x13 },
                { 0x02,0x02,0x06,0x06,0x06,0x09,0x09,0x09,0x0E,0x0E,0x0F,0x0F,0x10,0x13,0x13,0x13 },
                { 0x02,0x02,0x07,0x07,0x07,0x0A,0x0A,0x0A,0x0C,0x0C,0x0F,0x0F,0x10,0x13,0x13,0x13 }
            };
            Console.WriteLine($"Seed={Seed:X16}");

            for (int z = 0; z < 4; z++)
            {
                byte[] data = new byte[16];
                for (int i = 0; i < 16; i++)
                    data[i] = RoomIDs[z, i];

                for (int i = 0; i < 15; i++)
                {
                    Seed = NextSeed(Seed);
                    ulong swapIndex = (((Seed >> 32) * 16) >> 32);
                    byte tmp = data[i];
                    data[i] = data[swapIndex];
                    data[swapIndex] = tmp;

                }

                Console.WriteLine($"--- Shuffled RoomIDs (Z={z}) ---");
                for (int i = 0; i < 16; i++)
                {
                    Console.Write($"{data[i]:X2} ");
                    if ((i + 1) % 4 == 0) Console.WriteLine();
                }
                int index = 0;

                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (floor.Rooms[y, x, z].Boss)
                        {
                            floor.Rooms[y, x, z].RoomID = 0xFF;
                            floor.Rooms[y, x, z].Val = 0;
                            continue;
                        }

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
                            if (n == 0x01 || n == 0x7 || n == 0x9 || n == 0xB) floor.Rooms[y, x, z].Val = 2;
                            if (n == 0x02 || n == 0xC || n == 0xD || n == 0xE) floor.Rooms[y, x, z].Val = 2;
                            else floor.Rooms[y, x, z].Val = 2;

                            index++;
                        }
                    }
                }

                Console.WriteLine("=== Room IDs ===");

                Console.WriteLine($"Z={z}");
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        var id = floor.Rooms[y, x, z].RoomID;
                            Console.Write($"{id:X2} ");
                    }
                    Console.WriteLine();
                }
                
            }
            return Seed;
        }

        private ulong NextSeed(ulong Seed)
        {
            return Seed * 0x5D588B656C078965UL + 0x269EC3UL;
        }
    }
}

