using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.Data;

namespace MinerManager
{
   
    class Program
    {
        protected static int origRow;
        protected static int origCol;
        public static DataSet miner = new DataSet();
        public static DataSet pool = new DataSet();
        public static DataSet wallet = new DataSet();
        

        
        
        static void Main(string[] args)
        {
            xmlreadconf();
            
            
            //configuration[] cfg = ReadConfig();
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

                        xmladdconfig();
                        break;
                    case 2:
                        xmleditconf();

                        break;
                    case 3:
                        // 
                        StartMiner();
                        break;
                }


            }
        }


        


        static void StartMiner()
        {
            Console.Clear();
            int n = 0;
            foreach (DataRow row in miner.Tables[0].Rows )
            {
                n++;
                WriteAt(n + " ", 0, n);

                WriteAt(row[0].ToString(), 2, n);
                WriteAt(row[1].ToString(), 20, n);
                WriteAt(row[2].ToString(), 35, n);
                
            }
            WriteAt("Select: ", 0, n+1);
            n = Console.ReadKey().KeyChar - 48;
            Console.WriteLine(miner.Tables[0].Rows[n - 1][2].ToString());


            string filePath = miner.Tables[0].Rows[n-1][2].ToString();
            string arg = miner.Tables[0].Rows[n - 1][3].ToString();


            Console.Clear();
            n = 0;
            foreach (DataRow row in pool.Tables[0].Rows)
            {
                n++;
                WriteAt(n + " ", 0, n);

                WriteAt(row[0].ToString(), 2, n);
                WriteAt(row[2].ToString(), 20, n);
                WriteAt(row[3].ToString(), 35, n);

            }
            WriteAt("Select: ", 0, n + 1);
            n = Console.ReadKey().KeyChar - 48;

            arg.Replace("WALLET",wallet.Tables[0].Rows[n - 1][1].ToString()) ;
            arg.Replace("POOL",pool.Tables[0].Rows[n - 1][0].ToString());
            arg.Replace("PORTA",pool.Tables[0].Rows[n - 1][3].ToString());
            arg.Replace("PASSW",pool.Tables[0].Rows[n - 1][4].ToString());
            arg.Replace("RIG","2x970gtx");

            ProcessStartInfo psi = new ProcessStartInfo(filePath, arg);
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.UseShellExecute = true;
            using (Process.Start(psi)) ; 

            //proc.StartInfo.Arguments = cfg[n-1].param;
            //string cmd =  cfg[n - 1].path + " " + cfg[n - 1].param; 
            //Thread proc = new Thread(() => { MinerManger.ParallelRun.runCommand(cmd); });            
            //proc.IsBackground = true;
            //proc.Priority = ThreadPriority.AboveNormal;
            //proc.IsBackground = true;
            //proc.Start();

            //proc.Close();

        }

        static void xmladdconfig()
        {
            Console.Clear();
            int n = 0;
            string aux_s;
            WriteAt("1) Add Miner", 0, 0);
            WriteAt("2) Add Pool ", 0, 1);
            WriteAt("3) Add Wallet", 0, 2);
            WriteAt("Select: ", 3, 3);
            do
            {
                aux_s = Console.ReadLine();
            } while (int.TryParse(aux_s, out n) == false & n<= 3 );
            switch (n)
            {
                case 1:
                    xmladdrow(miner);
                    break;
                case 2:
                    xmladdrow(pool);
                    break;
                case 3:
                    xmladdrow(wallet);
                    break;
            }
            xmlwriteconf();
        }

        static void xmladdrow(DataSet data)
        {
            int i = 0;
            Console.Clear();
            DataRow row = data.Tables[0].NewRow();
            foreach (var col in row.ItemArray)
            {
                Console.WriteLine(data.Tables[0].Rows[0].Table.Columns[i]);
                row[i] = Console.ReadLine();
                i++;
            }
            
            data.Tables[0].Rows.Add(row);
            
        }


        
        static void xmlwriteconf()
        {
            try
            {
                miner.WriteXml(Directory.GetCurrentDirectory() + @"\Miner.xml");
                Console.WriteLine("XML data written successfully to " + Directory.GetCurrentDirectory() + @"\Miner.xml");
                pool.WriteXml(Directory.GetCurrentDirectory() + @"\Pool.xml");
                Console.WriteLine("XML data written successfully to " + Directory.GetCurrentDirectory() + @"\Pool.xml");
                wallet.WriteXml(Directory.GetCurrentDirectory() + @"\Wallet.xml");
                Console.WriteLine("XML data written successfully to " + Directory.GetCurrentDirectory() + @"\Wallet.xml");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
           
        }
        static void xmlreadconf()
        {
            try
            {
                miner.ReadXml(Directory.GetCurrentDirectory() + @"\Miner.xml");
                pool.ReadXml(Directory.GetCurrentDirectory() + @"\Pool.xml");
                wallet.ReadXml(Directory.GetCurrentDirectory() + @"\Wallet.xml");
                Console.WriteLine("XML data read successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
        
        static void xmleditconf()
        {
            Console.Clear();
            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;

            
            int n = 0;
            string aux_s;

            WriteAt("1) Edit Miner", 0, 0);
            WriteAt("2) Edit Pool ", 0, 1);
            WriteAt("3) Edit Wallet", 0, 2);
            WriteAt("Select: ", 3, 3);
            do
            {
                aux_s = Console.ReadLine();
            } while (int.TryParse(aux_s, out n) == false & n <= 3);
            switch (n)
            {
                case 1:
                    EditRow(miner);
                    break;
                case 2:
                    EditRow(pool);
                    break;
                case 3:
                    EditRow(wallet);
                    break;
            }
            xmlwriteconf();
        }

        static void EditRow(DataSet data)
        {
            
            
            string aux_s;
            int n = 0;
            int i = 0;
            do
            {
                i = 0;
                int i2 = 0;
                Console.Clear();
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    i2 = 0;
                    foreach (var col in row.ItemArray)
                    {
                        WriteAt(col.ToString(), i2 + 2, i);
                        i2 = i2 + 20;
                    }
                    i++;
                    WriteAt(i.ToString(), 0, i - 1);
                }
                WriteAt("Edit:", 0, i);
                aux_s = Console.ReadLine();
            } while (int.TryParse(aux_s, out n) == false || n > i);
            i = 0;
            foreach (var col in data.Tables[0].Rows[n-1].ItemArray)
            {
                Console.WriteLine("Set : " + data.Tables[0].Rows[0].Table.Columns[n-1]);
                Console.WriteLine(col);
                data.Tables[0].Rows[n - 1][i] = Console.ReadLine();
                i++;
            }
            
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
