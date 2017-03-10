using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCanalClient;

namespace NCannal.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var connector = new SimpleCanalConnector("127.0.0.1", "", "", "example");
            connector.Subscribe();
            TempClass.TestMessage(connector);
            Console.WriteLine("回车结束");
            Console.ReadLine();
            connector.UnSubscribe();
            connector.Disconnect();
        }
    }
}
