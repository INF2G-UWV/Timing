using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADDLL;

namespace StringsH7
{
    class Program
    {
        static void Main(string[] args)
        {
            int threadCount = 0;
            Process proc = Process.GetCurrentProcess();
            foreach (ProcessThread pt in proc.Threads)
            {// Set all our threads to use core 1 of the CPU
                pt.IdealProcessor = 0;
                pt.ProcessorAffinity = (IntPtr)0x1;
                pt.PriorityLevel = ThreadPriorityLevel.Highest;
                threadCount++;
            }

            Console.WriteLine("> Thread affinity changes: {0}", threadCount);

            object threadLock = new object(); 
 
            //Size of the string
            int size = 10000;
            
            //Timer for the StringBuilder
            HighResolutionTimer timerSB = new HighResolutionTimer(true);
            //Timer for the string
            HighResolutionTimer timerString = new HighResolutionTimer(true);

            // The order of magnitude we're displaying our times in
            TimeResolution timeResolution = TimeResolution.Milliseconds;

            // Helper variables
            double SBDuration = 0, SDuration = 0;
 
            //Output settings
            Console.WriteLine("> size: {0}, TimeResolution: {1} \n", 
                size, timeResolution.ToString());

            Console.Write("> Stopwatch: ");
            lock (threadLock)
            {
                // Lock the thread
                timerSB.Start();
                BuildSB(size);
                    
                timerSB.Stop();
                SBDuration = timerSB.Duration(timeResolution);
            }

            lock (threadLock)
            {
                // Lock the thread
                timerString.Start();
                BuildString(size);

                timerString.Stop();
                SDuration = timerString.Duration(timeResolution);
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Duration " + SDuration + " " + SBDuration);

            Console.WriteLine("> String Duration: {0} {1}", 
                SDuration, timeResolution.ToString());

            Console.WriteLine("> StringBuilder Duration: {0} {1}", 
                SBDuration, timeResolution.ToString());

            // Calculate percentage
            double highest = Math.Max(SBDuration, SDuration);
            double lowest = Math.Min(SBDuration, SDuration);

            Console.WriteLine("Verschil: " + (highest - lowest));
        
            Console.ReadLine();
        }

        /*
         * Static function that build strings with the StringBuilder class
         */
        static void BuildSB(int size)
        {
            StringBuilder sbObject = new StringBuilder();
            for (int i = 0; i <= size; i++)
            {
                sbObject.Append("a");
            }

          //  Console.WriteLine(sbObject.ToString());
        }

        /*
         * Static function that builds strings with the string class
         */
        static void BuildString(int size) 
        {
            string stringObject = "";
            for (int i = 0; i <= size; i++)
            {
                stringObject += "a";
               // Console.WriteLine(stringObject.ToString());
            }
        }

        /*
         * Test to use the StringBuilder
         */
        public static void TestStringBuilder()
        {
            StringBuilder stBuff = new StringBuilder("Ken Thompson");
            Console.WriteLine("Length of stBuff3: " + stBuff.Length);
            Console.WriteLine("Capacity of stBuff3: " + stBuff.Capacity);
            Console.WriteLine("Maximum capacity of stBuff3: " +
            stBuff.MaxCapacity);
            Console.Read();
        }
    }
}
