using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MugenRNG
{
    internal class TrainerB
    {
        public (ushort[,] TIDs, int GateTrainerFloor, int GateTrainerRand) GenerateTrainerB(ulong Seed1)
        {
            ushort[,] TrainerIDs =
                {
                    { 0x01FE , 0x01FF , 0x0200 , 0x023A , 0x023B , 0x023C }, //2
                    { 0x0201 , 0x0202 , 0x0203 , 0x023D , 0x023E , 0x023F }, //1
                    { 0x0204 , 0x0205 , 0x0240 , 0x0241 , 0x0 , 0x0}, //3
                    { 0x0206 , 0x0207 , 0x0242 , 0x0243 , 0x0 , 0x0}, //2
                    { 0x0208 , 0x0209 , 0x020A , 0x0244 , 0x0245 , 0x0246 }, //2
                    { 0x020B , 0x020C , 0x020D , 0x0247 , 0x0248 , 0x0249 }, //1
                    { 0x020E, 0x020F, 0x024A, 0x024B, 0x0, 0x0}, //2
                    { 0x0210, 0x0211, 0x024C, 0x024D, 0x0, 0x0}, //2
                    { 0x0212 , 0x0213 , 0x0214 , 0x024E , 0x024F , 0x0250 }, //3
                    { 0x0215 , 0x0216 , 0x0217 , 0x0251 , 0x0252 , 0x0253 }, //2
                    { 0x0218 , 0x0219 , 0x0254 , 0x0255 , 0x0 , 0x0 }, //3
                    { 0x021A , 0x021B , 0x0256 , 0x0257 , 0x0 , 0x0 }, //2
                    { 0x027C, 0x027D, 0x027E, 0x027F, 0x0, 0x0}, //1
                    { 0x0277, 0x0278, 0x0279, 0x027A, 0x0, 0x0}, //1
                };

            int[] GetCount = { 2, 1, 3, 2, 2, 1, 2, 2, 3, 2, 3, 2, 1, 1 };
            ushort[,] TIDs = new ushort[4, 27];

            for (int z = 0; z < 4; z++)　//TIDs生成
            {
                List<ushort> List = new List<ushort>();

                for (int n = 0; n < 14; n++)
                {
                    ushort[] data = new ushort[6];
                    var Count = 6;
                    if (n == 2 || n == 3 || n == 6 || n == 7 || n == 10 || n == 11 || n == 12 || n == 13) Count = 4;

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
