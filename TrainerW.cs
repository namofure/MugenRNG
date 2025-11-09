using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MugenRNG
{
    internal class TrainerW
    {
        public (ushort[,] TIDs, int GateTrainerFloor, int GateTrainerRand) GenerateTrainerW(ulong Seed1)
        {
            ushort[,] TrainerIDs =
                {
                    { 0x01E0 , 0x01E1 , 0x01E2 , 0x021C , 0x021D , 0x021E }, //1
                    { 0x01E3 , 0x01E4 , 0x01E5 , 0x021F , 0x0220 , 0x0221 }, //3
                    { 0x01E6 , 0x01E7 , 0x0222 , 0x0223 , 0x0 , 0x0 }, //3
                    { 0x01E8 , 0x01E9 , 0x0224 , 0x0225 , 0x0 , 0x0 }, //2
                    { 0x01ED , 0x01EE , 0x01EF , 0x0229 , 0x022A , 0x022B }, //2
                    { 0x01F0 , 0x01F1 , 0x022C , 0x022D , 0x0 , 0x0 }, //3
                    { 0x01F2 , 0x01F3 , 0x022E , 0x022F , 0x0 , 0x0 }, //1
                    { 0x01F4 , 0x01F5 , 0x01F6 , 0x0230 , 0x0231 , 0x0232 }, //2
                    { 0x01FA , 0x01FB , 0x0236 , 0x0237 , 0x0 , 0x0 }, //2
                    { 0x01FC , 0x01FD , 0x0238 , 0x0239 , 0x0 , 0x0 }, //2
                    { 0x01F7 , 0x01F8 , 0x01F9 , 0x0223 , 0x0234 , 0x0235 }, //2
                    { 0x01EA , 0x01EB , 0x01EC , 0x0226 , 0x0227 , 0x0228 }, //2
                    { 0x025E , 0x025F , 0x0260 , 0x0261 , 0x0 , 0x0 }, //1
                    { 0x0268 , 0x0269 , 0x026A , 0x026B , 0x0 , 0x0 }, //1
                };

            int[] GetCount = { 1, 3, 3, 2, 2, 3, 1, 2, 2, 2, 2, 2, 1, 1 };
            ushort[,] TIDs = new ushort[4, 27];

            for (int z = 0; z < 4; z++)　//TIDs生成
            {
                List<ushort> List = new List<ushort>();

                for (int n = 0; n < 14; n++)
                {
                    ushort[] data = new ushort[6];
                    var Count = 6;
                    if (n == 2 || n == 3 || n == 5 || n == 6 || n == 8 || n == 9 || n == 12 || n == 13) Count = 4;

                    for (int i = 0; i < Count; i++)
                        data[i] = TrainerIDs[n, i];

                    for (int i = 0; i < Count; i++)
                    {
                        Seed1 = NextSeed(Seed1);
                        ulong swapIndex = (((Seed1 >> 32) * (ulong)Count) >> 32);
                        ushort tmp = data[i];
                        data[i] = data[swapIndex];
                        data[swapIndex] = tmp;
                    }

                    for (int i = 0; i < GetCount[n]; i++)
                    {
                        List.Add(data[i]);
                    }
                }

                for (int i = 0; i < 27; i++)
                {
                    TIDs[z, i] = List[i];
                }

                for (int i = 0; i < 27; i++)
                {
                    Seed1 = NextSeed(Seed1);
                    ulong swapIndex = ((Seed1 >> 32) * 27) >> 32;
                    ushort tmp = TIDs[z, i];
                    TIDs[z, i] = TIDs[z, swapIndex];
                    TIDs[z, swapIndex] = tmp;
                }
            }

            //ゲートトレーナー生成
            Seed1 = NextSeed(Seed1);
            int GateTrainerFloor = (int)(((Seed1 >> 32) * 4) >> 32);

            Seed1 = NextSeed(Seed1);
            int GateTrainerRand = (int)(((Seed1 >> 32) * 21) >> 32) + 1;

            Console.WriteLine($"GateTrainer:{GateTrainerFloor},{GateTrainerRand}");

            //ドクター生成を追加
            return (TIDs, GateTrainerFloor, GateTrainerRand);
        }

        private ulong NextSeed(ulong Seed1)
        {
            return Seed1 * 0x5D588B656C078965UL + 0x269EC3UL;
        }
    }
}
