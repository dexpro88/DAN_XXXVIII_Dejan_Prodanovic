using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAN_XXXVIII_Dejan_Prodanovic
{
    class Program
    {
        static void Main(string[] args)
        {
            TruckLoad truckLoad = new TruckLoad();
            Menager menager = new Menager();

            Thread t1 = new Thread(() => menager.GenerateRandomNumbers());
            Thread t2 = new Thread(() => menager.ChooseBestRoutes());
            Thread t3 = new Thread(() => menager.MenagerAnnounceTheBestRoutes());

            t3.Start();
            t2.Start();
            t1.Start();

            t1.Join();
            t2.Join();
            t3.Join();

            Console.WriteLine();
            truckLoad.PrepareTrucks(menager.bestRoutes);
            Console.WriteLine();
            truckLoad.StartDestinationThreads(menager.bestRoutes);

            truckLoad.ChooseRoutesForTrucks(menager.bestRoutes);

            Console.ReadLine();
            Console.ReadLine();
        }
    }
}
