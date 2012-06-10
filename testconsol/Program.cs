using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace testconsol
{
    class Program
    {
        static void Main(string[] args)
        {
            PluginFramework.PluginManage.Current.WinAppLaunch();
            // Console.WriteLine("--------------------WinAppLoad---------------");
            while (true)
            {
                Stopwatch sw = Stopwatch.StartNew();
                GC.Collect();

                int gc0 = GC.CollectionCount(0);
                int gc1 = GC.CollectionCount(1);
                int gc2 = GC.CollectionCount(2);

                for (int index = 0; index < 10000; index++)
                {
                    TestMedoth();
                }

                Console.WriteLine(GC.CollectionCount(0) - gc0);
                Console.WriteLine(GC.CollectionCount(1) - gc1);
                Console.WriteLine(GC.CollectionCount(2) - gc2);
                Console.WriteLine(sw.ElapsedMilliseconds);

                //Console.WriteLine("Activator.LogServer.OutLog(fdafdsafads);");
                Console.ReadKey();
            }
        }
        public static void TestMedoth()
        {

            if (null != Activator.LogServer)
            { }



        }
    }
}
