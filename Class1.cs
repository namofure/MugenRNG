using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MugenRNG
{
    public class MugenFloor
    {


        public Room[,,] Rooms = new Room[4, 4, 4];
        public (int Y, int X, int Z) Boss;

        StringBuilder log = new StringBuilder();

        public MugenFloor()  //Roomの初期化
        {
            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                    for (int z = 0; z < 4; z++)
                        Rooms[y, x, z] = new Room();
        }

        public ulong Generate(ulong Seed)
        {
            Seed = NextSeed(Seed);

            ulong High32 = Seed >> 32;
            int BossFloor = (int)((High32 * 4) >> 32);
            log.AppendLine($"[Boss Floor] Seed = {High32:X8} → Rnad={BossFloor}");

            Seed = NextSeed(Seed);

            High32 = Seed >> 32;
            int BossRand = (int)(((High32 * 11) >> 32));
            log.AppendLine($"[Boss Room] Seed = {High32:X8} → Rand = {BossRand}");

            (int Y, int X)[] BossTable = new (int, int)[]
            {
                (0,1),(0,2),(0,3),
                (1,0),(1,1),(1,2),(1,3),
                (2,0),(2,1),(2,2),(2,3)
            };

            Boss = (BossTable[BossRand].Y, BossTable[BossRand].X, BossFloor);
            Rooms[Boss.Y, Boss.X, BossFloor].Boss = true;
            Console.WriteLine($"[BossRoom] Rand = {BossRand} => ({Boss.Y},{Boss.X})");
            log.AppendLine($"[Boss Room] Seed = {High32:X8} → Rand = {BossRand} → ({Boss.Y},{Boss.X})");

            Seed = NextSeed(Seed);

            // 通路生成
            Stack<(int Y, int X)> stack = new Stack<(int Y, int X)>();


            for (int z = 0; z < 4; z++)
            {
                int x = 0, y = 0;
                int StepCount = 15;

                if (BossFloor == z)
                {
                    StepCount = 14;
                }

                log.AppendLine($"{z + 1}階層");
                for (int Step = 0; Step < StepCount;)
                {
                    Console.WriteLine($"({y},{x})\n");

                    
                    Rooms[y, x, z].Visited = true;
                    var FirstRound = true;

                    var dirs = GetAvailableDirs(y, x, z, FirstRound);
                    log.AppendLine($"\n[Step {Step}] Current=({y},{x})  AvailableDirs=[{string.Join(",", dirs)}]");

                    if (dirs.Count == 0)
                    {
                        if (stack.Count > 0)
                        {
                            (y, x) = stack.Pop();
                            log.AppendLine($"→ Dead end, backtrack to ({y},{x})");
                            continue;
                        }
                        else break;
                    }

                    High32 = Seed >> 32;
                    int RandVal = (int)((High32 * (ulong)dirs.Count) >> 32);
                    log.AppendLine($"Seed = {High32:X8}  RandVal={RandVal}  dirs.Count={dirs.Count}");
                    log.AppendLine($"Seed = {High32:X8}  RandVal = {RandVal}  ({y},{x})");
                    Console.WriteLine($"Seed = {High32:X8}  RandVal={RandVal}  dirs.Count={dirs.Count}, ({y},{x}), AvailableDirs=[{string.Join(",", dirs)}]"); ;

                    Seed = NextSeed(Seed);

                    int dirIndex = 0;
                    int chosenDir = 0;
                    for (int d = 0; d < 4; d++)
                    {
                        if (!dirs.Contains(d)) continue;
                        if (dirIndex == RandVal)
                        {
                            chosenDir = d;
                            break;
                        }
                        dirIndex++;
                    }
                    Console.WriteLine($"{RandVal},{chosenDir}");

                    (int ny, int nx) = MoveDir(y, x, chosenDir);
                    Console.WriteLine($"({ny},{nx})");

                    LinkRooms(y, x, z, chosenDir);

                    log.AppendLine($"[Link] dir={DirToStr(chosenDir)}");

                    stack.Push((y, x));
                    log.AppendLine($"→ Move to ({ny},{nx}) dir={DirToStr(chosenDir)}");

                    Console.WriteLine($"({y},{x})\n");


                    x = nx; y = ny;
                    Step++;
                }
                log.AppendLine($"");
                x = 1; y = 0;

                //----------------------------------------------------------

                for (int Step = 0; Step < 15;)
                {
                    Console.WriteLine($"({y},{x})\n");
                    if (Rooms[y, x, z].Boss)
                    {
                        x++;
                        if (x == 4)
                        {
                            x = 0;
                            y++;
                        }
                        continue;
                    }

                    var FirstRound = false;
                    var dirs = GetAvailableDirs(y, x, z, FirstRound);
                    log.AppendLine($"\n[Step {Step}] Current=({y},{x})  AvailableDirs=[{string.Join(",", dirs)}]");

                    if (dirs.Count == 0)
                    {
                        if (stack.Count > 0)
                        {
                            (y, x) = stack.Pop();
                            log.AppendLine($"→ Dead end, backtrack to ({y},{x})");
                            continue;
                        }
                        else break;
                    }

                    High32 = Seed >> 32;
                    int RandVal = (int)((High32 * (ulong)dirs.Count) >> 32);
                    log.AppendLine($"Seed = {High32:X8}  RandVal={RandVal}  dirs.Count={dirs.Count}");
                    log.AppendLine($"Seed = {High32:X8}  RandVal = {RandVal}  ({y},{x})");
                    Console.WriteLine($"Seed = {High32:X8}  RandVal={RandVal}  dirs.Count={dirs.Count}, ({y},{x}), AvailableDirs=[{string.Join(",", dirs)}]"); ;

                    Seed = NextSeed(Seed);

                    int dirIndex = 0;
                    int chosenDir = 0;
                    for (int d = 0; d < 4; d++)
                    {
                        if (dirs.Contains(d))
                        {
                            if (dirIndex == RandVal)
                            {
                                chosenDir = d;
                                break;
                            }
                            dirIndex++;
                        }
                    }
                    Console.WriteLine($"{RandVal},{chosenDir}");

                    (int ny, int nx) = MoveDir(y, x, chosenDir);
                    Console.WriteLine($"({ny},{nx})");

                    LinkRooms(y, x, z, chosenDir);

                    // フラグ確認出力
                    Room r = Rooms[y, x, z];
                    string flagNow = $"({y},{x}) N:{(r.North ? 1 : 0)} S:{(r.South ? 1 : 0)} E:{(r.East ? 1 : 0)} W:{(r.West ? 1 : 0)}";
                    log.AppendLine($"[Link] dir={DirToStr(chosenDir)}");
                    log.AppendLine($"  Current {flagNow}");


                    log.AppendLine($"→ Move to ({ny},{nx}) dir={DirToStr(chosenDir)}");



                    if (x == 3 && y == 3) break;
                    Console.WriteLine($"({y},{x})\n");

                    x++;
                    if (x == 4)
                    {
                        x = 0;
                        y++;
                    }

                    Step++;
                }
                log.AppendLine($"");
            }

            log.AppendLine($"=== Floor 1 ===");

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Room r = Rooms[y, x, 0];
                    string flags = $"N:{(r.North ? 1 : 0)} S:{(r.South ? 1 : 0)} E:{(r.East ? 1 : 0)} W:{(r.West ? 1 : 0)}";
                    string info = $"({y},{x}) {flags} {(r.Boss ? "[Boss]" : "")}";
                    log.AppendLine(info);
                    Console.WriteLine(info);
                }
                log.AppendLine("");
            }

            File.WriteAllText("DebugLog.txt", log.ToString());
            return Seed;
        }

        static string DirToStr(int dir) => dir switch
        {
            0 => "Left",
            1 => "Right",
            2 => "Down",
            3 => "Up",
            _ => "?"
        };

        List<int> GetAvailableDirs(int y, int x, int z, bool FirstRound)    //(y.x)
        {
            List<int> list = new List<int>();

            int[] dy = { 0, 0, 1, -1 };
            int[] dx = { 1, -1, 0, 0 };

            for (int d = 0; d < 4; d++)
            {
                int nx = x + dx[d];
                int ny = y + dy[d];
                if (nx < 0 || nx >= 4) continue;
                if (ny < 0 || ny >= 4) continue;
                if (Rooms[ny, nx, z].Visited && FirstRound == true) continue;
                if (Rooms[ny, nx, z].Boss)
                {
                    Rooms[ny, nx, z].South = true;
                    continue;
                }
                list.Add(d);
            }
            return list;
        }

        (int, int) MoveDir(int y, int x, int chosenDir)
        {
            return chosenDir switch
            {
                0 => (y, x + 1),
                1 => (y, x - 1),
                2 => (y + 1, x),
                3 => (y - 1, x),
                _ => (y, x)
            };
        }

        void LinkRooms(int y, int x, int z, int chosenDir)  //ここよくわからん
        {
            Room r1 = Rooms[y, x, z];

            if (chosenDir == 0) r1.East = true;  // 0: Left
            if (chosenDir == 1) r1.West = true;  // 1: Right
            if (chosenDir == 2) r1.South = true; // 2: Down
            if (chosenDir == 3) r1.North = true; // 3: Up
        }

        public ulong NextSeed(ulong Seed)
        {
            return Seed * 0x5D588B656C078965UL + 0x269EC3UL;
        }

    }

    public class Room
    {
        public bool FirstRound;
        public bool Visited;
        public bool Boss;
        public bool North, South, East, West;
    }
}