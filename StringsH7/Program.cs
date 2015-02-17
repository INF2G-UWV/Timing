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

            // The size of the array we're sorting
            int arraySize = 3;
            
            //TimerSB(TimerStringBuilder)
            HighResolutionTimer timerSB = new HighResolutionTimer(true);
            //TimerSB(TimerString)
            HighResolutionTimer timerString = new HighResolutionTimer(true);

            // The order of magnitude we're displaying our times in
            TimeResolution timeResolution = TimeResolution.Milliseconds;

            // Helper variables
            int tenPercent = size / 10;
            double SBAverage = 0, SBDuration = 0;
            double SAverage = 0, SDuration = 0;

            //Output settings
            Console.WriteLine("> ArraySize: {0}, size: {1}, TimeResolution: {2} \n", 
                arraySize, size, timeResolution.ToString());

            Console.Write("> Stopwatch: ");
            lock (threadLock)
            {
                // Lock the thread
                for (int i = 0; i <= arraySize; i++)
                {
                    timerSB.Start();
                    BuildSB(size);
                    
                    timerSB.Stop();
                    SBDuration = timerSB.Duration(timeResolution);
                    if (i % tenPercent == 0)
                    {// Display some progress
                        Console.Write("{0}% ",  ((i / tenPercent) * 10));
                    }            
                }
                Console.Write("100%");
            }

            lock (threadLock)
            {
                // Lock the thread
                for (int i = 0; i <= arraySize; i++)
                {
                    timerString.Start();
                    BuildString(size);

                    timerString.Stop();
                    SDuration = timerString.Duration(timeResolution);
                    if (i % tenPercent == 0)
                    {// Display some progress
                        Console.Write(" {0}% ", ((i / tenPercent) * 10));
                    }
                }
                Console.Write("100%");
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Duration " + SDuration + " " + SBDuration);
            SAverage = (SDuration / size);

            Console.WriteLine("> String Average Duration: {0} {1}", 
                SAverage, timeResolution.ToString());

            SBAverage = (SBDuration/ size);
            Console.WriteLine("> StringBuilder Average Duration: {0} {1}", 
                SBAverage, timeResolution.ToString());

            // Calculate percentage
            double highest = Math.Max(SAverage, SBAverage);
            double lowest = Math.Min(SAverage, SBAverage);

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
