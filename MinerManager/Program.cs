using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace MinerManager
{
    class Program
    {
        protected static int origRow;
        protected static int origCol;

        public configuration[] cfg { get; private set; }

        public struct configuration
        {
            public string path;
            public string param;
            public string desc; 
        };
        
        static void Main(string[] args)
        {
            configuration[] cfg = ReadConfig();
            int i = 0;
            int n = 0;
            while (i < 1)
            {
                Console.Clear();
                origRow = Console.CursorTop;
                origCol = Console.CursorLeft;
               

                string aux_s;
                
                WriteAt("1) Insert miner",0,0);
                WriteAt("2) Edit miner ",0,1);
                WriteAt("3) Start miner",0,2);
                WriteAt("Select: ", 3, 3);

                do
                {
                    aux_s = Console.ReadLine();
                } while (int.TryParse(aux_s, out n) == false);
                switch (n)
                {
                    case 1:
                        WriteConfig();
                        cfg = ReadConfig();
                        break;
                    case 2:
                        EditConf(cfg);
                        break;
                    case 3:
                        StartMiner(cfg);
                        break;
                }


            }
        }

        


        static void StartMiner(configuration[] cfg)
        {
            Console.Clear();
            int n = 0;
            foreach (configuration line in cfg)
            {
                n++;
                Console.WriteLine(n + " " + line.desc + " " + line.path + " " + line.param);
            }
            n = Console.ReadKey().KeyChar - 48;




            //proc.StartInfo.Arguments = cfg[n-1].param;
            string cmd = "'" + cfg[n - 1].path + "' " + cfg[n - 1].param; 
            Thread proc = new Thread(() => { MinerManger.ParallelRun.runCommand(cmd); });            
            proc.IsBackground = true;
            proc.Priority = ThreadPriority.AboveNormal;
            proc.IsBackground = true;
            proc.Start();
            //proc.Close();

        }



        public static configuration[] ReadConfig()
        {
            int n = 0;
            string[] aux_s = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Mining.conf");
            configuration[] cfg = new configuration[aux_s.Length];
            foreach (string line in aux_s)
            {
                int i = 0;
                int aux_i = 0;
                
                aux_i = line.IndexOf(";", i);
                Int32.TryParse(line.Substring(i, aux_i), out n);
                i = aux_i;
                aux_i = line.IndexOf(";", i + 1)-i;
                cfg[n-1].path = line.Substring(i +1, aux_i - 1 ) ;

                i = i + aux_i;
                aux_i = line.IndexOf(";", i + 1) - i;
                cfg[n-1].param = line.Substring(i+1, aux_i - 1);

                i = i + aux_i;
                aux_i = line.IndexOf(";", i +1) - i;
                cfg[n-1].desc = line.Substring(i+1, aux_i - 1);
            }
            return (cfg); 
        }
        static void EditConf(configuration[] cfg)
        {            
            int n = 0;
            while ( n <= cfg.Length)
            {
                Console.Clear();
                n = 0;
                foreach (configuration line in cfg)
                {
                    n++;
                    Console.WriteLine(n +" "+ line.desc + " " + line.path + " " + line.param);
                }
                n = 0;
                n = Console.ReadKey().KeyChar-48;
                if(n <= cfg.Length&n>0)
                {
                    Console.WriteLine(" Insert path Miner:"+ cfg[n-1].path);
                    cfg[n-1].path = Console.ReadLine();
                    Console.WriteLine("Insert param:" + cfg[n-1].param);
                    cfg[n-1].param = Console.ReadLine();
                    Console.WriteLine("Insert desc:" + cfg[n-1].desc);
                    cfg[n-1].desc = Console.ReadLine();
                }

            }
            n = 0;
            var file = new System.IO.StreamWriter(Directory.GetCurrentDirectory() + @"\Mining.conf");
            foreach (configuration line in cfg)
            {
                n++;
                file.WriteLine(n+";"+line.path+";"+line.param+";"+line.desc+";");
            }
            file.Close();
        }
        static void WriteConfig()
        {
            Console.Clear();
            string[] conf = { };
            int i = 0;
            string aux_s;

            while (i < 1)
            {
                conf = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Mining.conf");
                Console.WriteLine("Insert path Miner:");
                aux_s = Console.ReadLine();
                Array.Resize(ref conf, conf.Length + 1);
                conf[conf.Length - 1] = conf.Length + ";" + aux_s;
                Console.WriteLine("insert parameter:");
                aux_s = Console.ReadLine();
                conf[conf.Length - 1] = conf[conf.Length - 1] + ";" + aux_s ;
                Console.WriteLine("insert description:");
                aux_s = Console.ReadLine();
                conf[conf.Length - 1] = conf[conf.Length - 1] + ";" + aux_s +";";
                Console.WriteLine("Exit? (s/n);");
                aux_s = Console.ReadLine();
                if (aux_s != "y" || aux_s != "Y")
                {
                    i = 2;
                }                 
            }
            
            var file = new System.IO.StreamWriter(Directory.GetCurrentDirectory() + @"\Mining.conf");
            foreach (string line in conf)
            {
                file.WriteLine(line);
            }
            file.Close();
        }
        
        protected static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(origCol + x, origRow + y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        public class APIData
        {
            public AlgorithmType AlgorithmID;
            public AlgorithmType SecondaryAlgorithmID;
            public string AlgorithmName;
            public double Speed;
            public double SecondarySpeed;
            public APIData(AlgorithmType algorithmID, AlgorithmType secondaryAlgorithmID = AlgorithmType.NONE)
            {
                this.AlgorithmID = algorithmID;
                this.SecondaryAlgorithmID = secondaryAlgorithmID;
                this.AlgorithmName = AlgorithmNiceHashNames.GetName(DualAlgorithmID());
                this.Speed = 0.0;
                this.SecondarySpeed = 0.0;
            }
            public AlgorithmType DualAlgorithmID()
            {
                if (AlgorithmID == AlgorithmType.DaggerHashimoto)
                {
                    switch (SecondaryAlgorithmID)
                    {
                        case AlgorithmType.Decred:
                            return AlgorithmType.DaggerDecred;
                        case AlgorithmType.Lbry:
                            return AlgorithmType.DaggerLbry;
                        case AlgorithmType.Pascal:
                            return AlgorithmType.DaggerPascal;
                        case AlgorithmType.Sia:
                            return AlgorithmType.DaggerSia;
                    }
                }
                return AlgorithmID;
            }
        }

        public static class AlgorithmNiceHashNames
        {
            public static string GetName(AlgorithmType type)
            {
                if ((AlgorithmType.INVALID <= type && type <= AlgorithmType.Skunk) || (AlgorithmType.DaggerSia <= type && type <= AlgorithmType.DaggerPascal))
                {
                    return Enum.GetName(typeof(AlgorithmType), type);
                }
                return "NameNotFound type not supported";
            }
        }

        public enum AlgorithmType : int
        {
            // dual algos for grouping
            DaggerSia = -6,
            DaggerDecred = -5,
            DaggerLbry = -4,
            DaggerPascal = -3,
            INVALID = -2,
            NONE = -1,
            #region NiceHashAPI
            //Scrypt_UNUSED = 0,
            //SHA256_UNUSED = 1,
            //ScryptNf_UNUSED = 2,
            //X11_UNUSED = 3,
            //X13 = 4,
            Keccak = 5,
            //X15 = 6,
            Nist5 = 7,
            //NeoScrypt = 8,
            //Lyra2RE = 9,
            //WhirlpoolX = 10,
            //Qubit = 11,
            //Quark = 12,
            //Axiom_UNUSED = 13,
            //Lyra2REv2 = 14,
            //ScryptJaneNf16_UNUSED = 15,
            //Blake256r8 = 16,
            //Blake256r14 = 17, // NOT USED ANYMORE?
            //Blake256r8vnl = 18,
            //Hodl = 19,
            //DaggerHashimoto = 20,
            //Decred = 21,
            //CryptoNight = 22,
            //Lbry = 23,
            //Equihash = 24,
            //Pascal = 25
            // UNUSED START
            Scrypt_UNUSED = 0,
            SHA256_UNUSED = 1,
            ScryptNf_UNUSED = 2,
            X11_UNUSED = 3,
            X13_UNUSED = 4,
            //Keccak_UNUSED = 5,
            X15_UNUSED = 6,
            //Nist5_UNUSED = 7,

            WhirlpoolX_UNUSED = 10,
            Qubit_UNUSED = 11,
            Quark_UNUSED = 12,
            Axiom_UNUSED = 13,

            ScryptJaneNf16_UNUSED = 15,
            Blake256r8_UNUSED = 16,
            Blake256r14_UNUSED = 17,
            Blake256r8vnl_UNUSED = 18,
            // UNUSED END

            NeoScrypt = 8,
            Lyra2RE = 9,

            Lyra2REv2 = 14,

            Hodl = 19,
            DaggerHashimoto = 20,
            Decred = 21,
            CryptoNight = 22,
            Lbry = 23,
            Equihash = 24,
            Pascal = 25,
            X11Gost = 26,
            Sia = 27,
            Blake2s = 28,
            Skunk = 29
            #endregion // NiceHashAPI
        }
        
    }
}
