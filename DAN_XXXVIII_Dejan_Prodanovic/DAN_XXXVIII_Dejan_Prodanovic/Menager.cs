using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAN_XXXVIII_Dejan_Prodanovic
{
    class Menager
    {
        EventWaitHandle waitHandle = new AutoResetEvent(false);
        EventWaitHandle waitHandleForAnnounce = new AutoResetEvent(false);
        Random rnd = new Random();
        List<int> generatedNumbers = new List<int>();
        public List<int> bestRoutes = new List<int>();
        List<int> potentialyBestRoutes = new List<int>();
        bool menagerSignaled = false;
        bool signaledBack = false;


        /// <summary>
        /// method that generates random numbers and writes them to file
        /// it signals thread that chooses the best routes from generated numbers when it is done
        /// </summary>
        public void GenerateRandomNumbers()
        {
            Thread.Sleep(20);
            Random rnd = new Random();
            if (File.Exists("../../Routes.txt"))
            {
                System.IO.File.WriteAllText(@"../../Routes.txt", string.Empty);
            }


            Thread.Sleep(10);
            StreamWriter sw = File.AppendText("../../Routes.txt");
            for (int i = 0; i < 1000; i++)
            {
                int rndNumber = rnd.Next(1, 5001);
                generatedNumbers.Add(rndNumber);
                sw.WriteLine(rndNumber);
            }

            sw.Close();

            waitHandle.Set();



        }

        /// <summary>
        /// method that chooses best rootes from the 1000 random numbers from file
        /// best routes are smallest numbers divisible by 3 that have to be unique
        /// when it's done it sends signal to thread that has to announce the best routes to user
        /// </summary>
        /// <param name="bestRoutes"></param>
        public void ChooseBestRoutes()
        {
            List<int> divisibleByThree = new List<int>();

            waitHandle.WaitOne();
            using (StreamReader sr = new StreamReader("../../Routes.txt"))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    int number = Int32.Parse(line);
                    if (number % 3 == 0)
                    {
                        divisibleByThree.Add(number); ;
                    }
                }
            }

            divisibleByThree.Sort();
            bestRoutes = divisibleByThree.Distinct().Take(10).ToList();
           
            //here we are sending signal to thread that performs MenagerAnnounceTheBestRoutes method

            menagerSignaled = true;
            waitHandleForAnnounce.Set();

            if (signaledBack)
            {
                bestRoutes = potentialyBestRoutes;
            }


        }
        /// <summary>
        /// method that informs user about the choosen routes
        /// in case that it waited more than 3s for signal it will choose 10 random numbers 
        /// from 1000 numbers that we generated before to be the best routes
        /// </summary>
        public void MenagerAnnounceTheBestRoutes()
        {
            waitHandleForAnnounce.WaitOne(3000);
            if (menagerSignaled)
            {
                Console.WriteLine("Menadzer je odabrao najbolje rute a to su:");
                foreach (var item in bestRoutes)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("Menadzer je odabrao najbolje rute a to su:");
                potentialyBestRoutes = generatedNumbers.OrderBy(x => rnd.Next()).Take(10).ToList();
                foreach (var item in potentialyBestRoutes)
                {
                    Console.WriteLine(item);
                }
                signaledBack = true;
            }


        }
    }
}
