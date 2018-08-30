using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace 多线程池试验
{
    class Program
    {
        public static void Main()
        {
            TimerState();
            ManualResetEvent();
        }




        class TimerExampleState
        {
            public int counter = 0;
            public Timer tmr;
        }

        static void TimerState()
        {
            TimerExampleState s = new TimerExampleState();

            // 创建代理对象 System.Threading.TimerCallback，该代理将被定时调用 
            TimerCallback timerDelegate = new TimerCallback(CheckStatus);

            // 创建一个时间间隔为 1s 的定时器 
            // 第1个参数：指定了 TimerCallback 委托，表示要执行的方法； 
            // 第2个参数：一个包含回调方法要使用的信息的对象，或者为空引用； 
            // 第3个参数：延迟时间--计时开始的时刻距现在的时间，单位是毫秒,指定为"0"表示 立即启动计时器； 
            // 第4个参数：定时器的时间间隔--计时开始后，每隔这么长的一段时间，TimerCallback 所代表的方法将被调用一次 
            Timer timer = new Timer(timerDelegate, s, 1000, 1000);
            s.tmr = timer;

            // 主线程停下来等待 Timer 对象的终止 
            while (s.tmr != null)
            {
                Thread.Sleep(0);
            }
            Console.WriteLine("Timer example done.");
            Console.ReadLine();
        }

        /// <summary>
        /// 下面是被定时调用的方法
        /// </summary>
        /// <param name="state"></param> 
        static void CheckStatus(Object state)
        {
            TimerExampleState s = (TimerExampleState)state;
            s.counter++;
            Console.WriteLine("{0} Checking Status {1}.", DateTime.Now.TimeOfDay, s.counter);
            if (s.counter == 5)
            {
                //使用 Change 方法改变了时间间隔为2秒,再等待10秒 
                (s.tmr).Change(10000, 2000);
                Console.WriteLine("changed");
            }
            if (s.counter == 10)
            {
                Console.WriteLine("disposing of timer!");
                s.tmr.Dispose();
                s.tmr = null;
            }
        }



        public static void ManualResetEvent()
        {

            //新建ManualResetEvent对象并且初始化为无信号状态
            ManualResetEvent eventX = new ManualResetEvent(false);
            ThreadPool.SetMaxThreads(5, 5);
            Threads t = new Threads(15, eventX);
            for (int i = 0; i < 15; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(t.ThreadProc), i);

                ThreadPool.QueueUserWorkItem(new WaitCallback(Joson.ThreadProc), i);

            }
            //等待事件的完成，即线程调用ManualResetEvent.Set()方法
            //eventX.WaitOne  阻止当前线程，直到当前 WaitHandle 收到信号为止。 
            eventX.WaitOne(Timeout.Infinite, true);
            Console.WriteLine("断点测试");
            Thread.Sleep(1000);
            Console.WriteLine("运行结束");

            Console.Read();

        }





        public class Threads
        {
            public Threads(int count, ManualResetEvent mre)
            {
                iMaxCount = count;
                eventX = mre;
            }

            public static int iCount = 0;
            public static int iMaxCount = 0;
            public ManualResetEvent eventX;
            public void ThreadProc(object i)
            {
                Console.WriteLine("Thread[" + i.ToString() + "]");
                Thread.Sleep(1000);
                //Interlocked.Increment()操作是一个原子操作，作用是:iCount++ 具体请看下面说明 
                //原子操作，就是不能被更高等级中断抢夺优先的操作。你既然提这个问题，我就说深一点。
                //由于操作系统大部分时间处于开中断状态，
                //所以，一个程序在执行的时候可能被优先级更高的线程中断。
                //而有些操作是不能被中断的，不然会出现无法还原的后果，这时候，这些操作就需要原子操作。
                //就是不能被中断的操作。
                Interlocked.Increment(ref iCount);
                if (iCount == iMaxCount)
                {
                    Console.WriteLine("发出结束信号!");
                    eventX.Set();
                }
            }
        }
    }




    public class Joson
    {
        public static void ThreadProc(object i)
        {
            Console.WriteLine("Joson[" + i.ToString() + "]");
            Thread.Sleep(1000);
        }

    }

}
