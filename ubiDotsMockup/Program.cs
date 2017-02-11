using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ubidots;

namespace ubiDotsMockup
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int a = 0; a < 10; a++)
            {
//oprova modifica per commit 
                ApiClient client = new ApiClient("81ccc02cba03d9ae5a2ee1406603f064118233a8");

                Variable myTemp = client.GetVariable("589c3d0276254208983247d2");
                System.Console.WriteLine("Temperatura massima: " + myTemp.GetMax());
                System.Console.WriteLine("Temperatura minima: " + myTemp.GetMin());
                System.Console.WriteLine("Temperatura media: " + myTemp.GetMean());

                for (int i = 0; i < 5; i++)
                {
                    Random rm = new Random();
                    Double newTmp = rm.Next(-20, 50);
                    System.Console.WriteLine("Nuova lettura: " + newTmp);
                    myTemp.SaveValue(newTmp);
                    Thread.Sleep(1000);
                }
            }

           
        }
    }
}
