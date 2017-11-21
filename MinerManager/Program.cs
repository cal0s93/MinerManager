using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MinerManager
{
    class Program
    {
        
        static void Main(string[] args)
        {
            int i = 0;
            while (i < 1)
            {
                string aux_s;
                Console.WriteLine("   Select: ");
                Console.WriteLine("1) Insert miner");
                Console.WriteLine("2) Edit miner ");
                Console.WriteLine("3) Start miner");
                aux_s = Console.ReadLine().ToString();
                switch (aux_s)
                {
                    case "1":
                        WriteConfig();
                        break;
                    case "2":
                        WriteConfig();
                        break;
                    case "3":
                        WriteConfig();
                        break;
                }


            }
        }
        static void ReadConfig()
        {
           
            string[] aux_s = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\WriteLines2.txt");
            foreach(string line in aux_s)
            {
                
            }
        }

        static void WriteConfig()
        {
            string[] conf = { };
            int i = 0;
            string aux_s;

            while (i < 1)
            {
                conf = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\WriteLines2.txt");
                Console.WriteLine("Insert path Miner:");
                aux_s = Console.ReadLine();
                Array.Resize(ref conf, conf.Length + 1);
                conf[conf.Length - 1] = conf.Length + " ; '" + aux_s;
                Console.WriteLine("insert parameter:");
                aux_s = Console.ReadLine();
                conf[conf.Length - 1] = conf[conf.Length - 1] + "' ; '" + aux_s ;
                Console.WriteLine("insert description:");
                aux_s = Console.ReadLine();
                conf[conf.Length - 1] = conf[conf.Length - 1] + "' ; '" + aux_s + "'";
                Console.WriteLine("Exit? (s/n);");
                aux_s = Console.ReadLine();
                if (aux_s != "y" || aux_s != "Y")
                {
                    i = 2;
                }                 
            }
            
            var file = new System.IO.StreamWriter(Directory.GetCurrentDirectory() + @"\WriteLines2.txt");
            foreach (string line in conf)
            {
                file.WriteLine(line);
            }
            file.Close();
        }
        
    }
}
