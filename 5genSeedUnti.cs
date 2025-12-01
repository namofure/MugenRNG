using MugenRNG;
using System;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Windows.Forms;
using static MugenRNG.MainForm;

public class SeedSearcher
{
    public struct InSeedData
    {
        public ulong Seed;
        public byte Year;
        public byte Month;
        public byte Day;
        public byte Hour;
        public byte Minute;
        public byte Second;
        public uint VCount;
        public uint InTimer0;
    }

    private PIDPrm param;
    private int Version;
    private int CountCheck;

    public SeedSearcher(PIDPrm param, int Version, int CountCheck)
    {
        this.param = param;
        this.Version = Version;
        this.CountCheck = CountCheck;
    }

    public void GenerateSeeds(CancellationToken token, DataGridView DataGridView5)
    {
        DataGridView5.Invoke(new Action(() =>
        {
            DataGridView5.Rows.Clear();
            DataGridView5.Columns.Clear();
            DataGridView5.SuspendLayout();

            DataGridView5.Columns.Add("Seed", "初期SEED");
            DataGridView5.Columns.Add("Year", "年");
            DataGridView5.Columns.Add("Month", "月");
            DataGridView5.Columns.Add("Day", "日");
            DataGridView5.Columns.Add("Hour", "時");
            DataGridView5.Columns.Add("Minute", "分");
            DataGridView5.Columns.Add("Second", "秒");
            DataGridView5.Columns.Add("InTimer0", "Timer0");
            DataGridView5.Columns.Add("VCount", "VCount");
        }));

        for (DateTime Dt = param.InDt; Dt <= param.EnDt; Dt += param.Increment)
        {

            for (uint Timer0 = param.Values[4]; Timer0 <= param.Values[5]; Timer0++)
            {
                if (token.IsCancellationRequested) return; //中止

                InSeedData SeedData = GenSeed(param, Dt, Timer0);
                uint Seed = (uint)(SeedData.Seed >> 32);
                ulong PIDSeed = SeedData.Seed;

                ulong temp, Seed1, Seed2;
                MTSeed MTSeed = new MTSeed(Seed);               
                (temp, Seed1, Seed2) = MTSeed.UnovaRNG();

                ulong tempSeed2 = Seed2;
                Seed2 = NextSeed(Seed2);

                ulong High32 = Seed2 >> 32;
                ulong temp1 = High32;
                int BossFloor = (int)((High32 * 4) >> 32);
                if (BossFloor == 0)
                {
                    Seed2 = NextSeed(Seed2);

                    High32 = Seed2 >> 32;
                    ulong temp2 = High32;
                    int BossRand = (int)(((High32 * 11) >> 32));
                    if (BossRand == 0)
                    {
                        for (int i = 0; i < 119; i++)
                        {
                            Seed2 = NextSeed(Seed2);
                        }

                        List<int> TIDVal = new List<int>();

                        if (Version == 0)//黒の摩天楼
                        {
                            byte[] RoomIDs = { 0x02, 0x02, 0x09, 0x09, 0x09, 0x0A, 0x0A, 0x0A, 0x0D, 0x0D, 0x0F, 0x0F, 0x10, 0x13, 0x13, 0x13 };
                            byte[] tempIDs = { 0x02, 0x09, 0x0A, 0x0D };

                            byte[] data = new byte[16];
                            for (int i = 0; i < 16; i++)
                                data[i] = RoomIDs[i];
                            for (int i = 0; i < 15; i++)
                            {
                                if (i == 0) ;
                                else Seed2 = NextSeed(Seed2);
                                ulong swapIndex = (((Seed2 >> 32) * 15) >> 32);

                                byte tmp = data[i];
                                data[i] = data[swapIndex];
                                data[swapIndex] = tmp;
                            }

                            Seed2 = NextSeed(Seed2);
                            ulong Rand = ((Seed2 >> 32) * 4) >> 32;
                            byte tempID = tempIDs[Rand];

                            byte n = 0;
                            int Val = 0;
                            for (int i = 0; i < 3; i++)
                            {
                                n = data[i];
                                Val = 0;
                                if (n == 0x13) Val = 0;
                                else if (n == 0x01 || n == 0x7 || n == 0x9 || n == 0xB) Val = 2;
                                else if (n == 0x02 || n == 0xC || n == 0xD || n == 0xE) Val = 3;
                                else Val = 1;

                                if (0x1 < data[i] && data[i] < 0xF)
                                {
                                    for (int m = 0; m < Val; m++)
                                    {
                                        TIDVal.Add(i + 2);
                                    }
                                }
                            }

                            for (int i = 3; i < 14; i++)
                            {
                                n = data[i];
                                Val = 0;
                                if (n == 0x13) Val = 0;
                                else if (n == 0x01 || n == 0x7 || n == 0x9 || n == 0xB) Val = 2;
                                else if (n == 0x02 || n == 0xC || n == 0xD || n == 0xE) Val = 3;
                                else Val = 1;

                                if (0x1 < data[i] && data[i] < 0xF)
                                {
                                    for (int m = 0; m < Val; m++)
                                    {
                                        TIDVal.Add(i + 3);
                                    }
                                }
                            }
                        }

                        else if (Version == 1)//白の樹洞
                        {
                            byte[] RoomIDs = { 0x02, 0x02, 0x09, 0x09, 0x09, 0x0A, 0x0A, 0x0A, 0x0D, 0x0D, 0x11, 0x11, 0x12, 0x13, 0x13, 0x13 };
                            byte[] tempIDs = { 0x02, 0x09, 0x0A, 0x0D };

                            byte[] data = new byte[16];
                            for (int i = 0; i < 16; i++)
                                data[i] = RoomIDs[i];

                            for (int i = 0; i < 15; i++)
                            {
                                if (i == 0) ;
                                else Seed2 = NextSeed(Seed2);
                                ulong swapIndex = (((Seed2 >> 32) * 15) >> 32);

                                byte tmp = data[i];
                                data[i] = data[swapIndex];
                                data[swapIndex] = tmp;
                            }

                            Seed2 = NextSeed(Seed2);
                            ulong Rand = ((Seed2 >> 32) * 4) >> 32;
                            byte tempID = tempIDs[Rand];

                            byte n = 0;
                            int Val = 0;
                            for (int i = 0; i < 3; i++)
                            {
                                n = data[i];
                                Val = 0;
                                if (n == 0x13) Val = 0;
                                else if (n == 0x01 || n == 0x7 || n == 0x9 || n == 0xB) Val = 2;
                                else if (n == 0x02 || n == 0xC || n == 0xD || n == 0xE) Val = 3;
                                else Val = 1;

                                if (0x1 < data[i] && data[i] < 0xF)
                                {
                                    for (int m = 0; m < Val; m++)
                                    {
                                        TIDVal.Add(i + 2);
                                    }
                                }
                            }

                            for (int i = 3; i < 14; i++)
                            {
                                n = data[i];
                                Val = 0;
                                if (n == 0x13) Val = 0;
                                else if (n == 0x01 || n == 0x7 || n == 0x9 || n == 0xB) Val = 2;
                                else if (n == 0x02 || n == 0xC || n == 0xD || n == 0xE) Val = 3;
                                else Val = 1;

                                if (0x1 < data[i] && data[i] < 0xF)
                                {
                                    for (int m = 0; m < Val; m++)
                                    {
                                        TIDVal.Add(i + 3);
                                    }
                                }
                            }
                        }

                        for (int i = 0; i < 380; i++)
                        {
                            Seed1 = NextSeed(Seed1);

                        }

                        //ゲートトレーナー生成
                        Seed1 = NextSeed(Seed1);
                        ulong temp3 = Seed1 >> 32;
                        int GateTrainerFloor = (int)((temp3 * 4) >> 32);
                        if (GateTrainerFloor == 0)
                        {
                            Seed1 = NextSeed(Seed1);
                            uint temp4 = (uint)(Seed1 >> 32);
                            int GateTrainerRand = TIDVal[(int)((temp4 * TIDVal.Count) >> 32)];

                            if (GateTrainerRand == 4 || GateTrainerRand == 5)
                            {
                                DataGridView5.Invoke(new Action(() =>
                                {
                                    int rowIndex = DataGridView5.Rows.Add();
                                    var row = DataGridView5.Rows[rowIndex];

                                    row.Cells[0].Value = SeedData.Seed.ToString("X16");
                                    row.Cells[1].Value = SeedData.Year;
                                    row.Cells[2].Value = SeedData.Month;
                                    row.Cells[3].Value = SeedData.Day;
                                    row.Cells[4].Value = SeedData.Hour;
                                    row.Cells[5].Value = SeedData.Minute;
                                    row.Cells[6].Value = SeedData.Second;
                                    row.Cells[7].Value = SeedData.InTimer0.ToString("X4");
                                    row.Cells[8].Value = SeedData.VCount.ToString("X2");
                                }));

                                if (CountCheck == 1) return; //1つ見つけたら終了
                                break;
                            }
                        }
                    }
                }
            }
        }
        return;
    }
    private ulong NextSeed(ulong Seed)
    {
        return Seed * 0x5D588B656C078965UL + 0x269EC3UL;
    }

    //------------------------------------------------------------------------
    public static InSeedData GenSeed(PIDPrm param, DateTime Dt, uint Timer0)
    {
        uint[] Data = new uint[80];

        byte[] YMDD = new byte[4];

        YMDD[3] = toHex(Dt.Year % 100);
        YMDD[2] = toHex(Dt.Month);
        YMDD[1] = toHex(Dt.Day);
        YMDD[0] = (byte)(Dt.DayOfWeek);

        uint Date = BitConverter.ToUInt32(YMDD, 0);

        byte[] HMSZ = new byte[4];

        HMSZ[3] = toHex(Dt.Hour);
        if (Dt.Hour > 11) HMSZ[3] += 0x40;
        HMSZ[2] = toHex(Dt.Minute);
        HMSZ[1] = toHex(Dt.Second);
        HMSZ[0] = 0;

        uint Time = BitConverter.ToUInt32(HMSZ, 0);

        Data[0] = toLittleEndian(param.Values[0]);
        Data[1] = toLittleEndian(param.Values[1]);
        Data[2] = toLittleEndian(param.Values[2]);
        Data[3] = toLittleEndian(param.Values[2] + 0x54);
        Data[4] = toLittleEndian(param.Values[2] + 0x54);
        Data[5] = toLittleEndian((param.Values[3] << 16) + Timer0);
        Data[6] = (param.Values[6]);
        Data[7] = toLittleEndian(((param.Values[8] ^ param.Values[9])) ^ toLittleEndian(param.Values[7]));
        Data[8] = (Date);
        Data[9] = (Time);
        Data[10] = 0;
        Data[11] = 0;
        Data[12] = toLittleEndian(param.Values[10]);
        Data[13] = (param.Values[11]);
        Data[14] = (param.Values[12]);
        Data[15] = (param.Values[13]);



        //------------------------------------------------------
        for (int t = 16; t < 80; t++)
        {
            var w = Data[t - 3] ^ Data[t - 8] ^ Data[t - 14] ^ Data[t - 16];
            Data[t] = (w << 1) | (w >> 31);
        }

        const uint H0 = 0x67452301;
        const uint H1 = 0xEFCDAB89;
        const uint H2 = 0x98BADCFE;
        const uint H3 = 0x10325476;
        const uint H4 = 0xC3D2E1F0;

        uint A, B, C, D, E;
        A = H0; B = H1; C = H2; D = H3; E = H4;

        for (int t = 0; t < 20; t++)
        {
            var temp = ((A << 5) | (A >> 27)) + ((B & C) | ((~B) & D)) + E + Data[t] + 0x5A827999;
            E = D;
            D = C;
            C = (B << 30) | (B >> 2);
            B = A;
            A = temp;
        }
        for (int t = 20; t < 40; t++)
        {
            var temp = ((A << 5) | (A >> 27)) + (B ^ C ^ D) + E + Data[t] + 0x6ED9EBA1;
            E = D;
            D = C;
            C = (B << 30) | (B >> 2);
            B = A;
            A = temp;
        }
        for (int t = 40; t < 60; t++)
        {
            var temp = ((A << 5) | (A >> 27)) + ((B & C) | (B & D) | (C & D)) + E + Data[t] + 0x8F1BBCDC;
            E = D;
            D = C;
            C = (B << 30) | (B >> 2);
            B = A;
            A = temp;
        }
        for (int t = 60; t < 80; t++)
        {
            var temp = ((A << 5) | (A >> 27)) + (B ^ C ^ D) + E + Data[t] + 0xCA62C1D6;
            E = D;
            D = C;
            C = (B << 30) | (B >> 2);
            B = A;
            A = temp;
        }

        ulong Seed = toLittleEndian(H1 + B);
        Seed <<= 32;
        Seed |= toLittleEndian(H0 + A);

        //------------------------------------------------------



        InSeedData SeedData = new InSeedData
        {
            Seed = Seed,
            Year = (byte)(Dt.Year % 100),
            Month = (byte)Dt.Month,
            Day = (byte)Dt.Day,
            Hour = (byte)Dt.Hour,
            Minute = (byte)Dt.Minute,
            Second = (byte)Dt.Second,
            VCount = param.Values[3],
            InTimer0 = Timer0
        };
        return SeedData;
    }

    static byte toHex(int value)
    {
        return (byte)((value / 10) * 6 + value);
    }

    static uint toLittleEndian(uint values)
    {
        return ((values & 0x000000FF) << 24) |
                ((values & 0x0000FF00) << 8) |
                ((values & 0x00FF0000) >> 8) |
                ((values & 0xFF000000) >> 24);
    }
}
