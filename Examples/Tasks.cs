using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomConsole.Examples
{
    public class TaskExample
    {
        private Common _common;
        public TaskExample()
        {
            _common = new Common();
        }

        public void UsingParallelInvoke()
        {
            var sw = Stopwatch.StartNew();
            Parallel.Invoke(() => _common.BookCar(), () => _common.BookHotel(), () => _common.BookPlane());
            sw.Stop();

            Console.WriteLine("Time taken: {0}", sw.Elapsed.Seconds);
        }

        public void TestMe()
        {
            var common = new Common();

            // Total time taken 8 sec (time taken by longest running task)
            var sw = Stopwatch.StartNew();

            Task<int> carTask = Task.Factory.StartNew<int>(common.BookCar);
            Task<int> planeTask = Task.Factory.StartNew<int>(common.BookPlane);
            Task<int> hotelTask = Task.Factory.StartNew<int>(common.BookHotel);


            Task hotelFollowUp = hotelTask.ContinueWith(
                taskPrev => Console.WriteLine("Add for booking {0}", taskPrev.Result)
                );

            hotelFollowUp.Wait();

            // No need to wait for tasks if task result is called
            //Task.WaitAll(carTask, hotelTask, planeTask);

            Console.WriteLine("Finished booking. CarId={0}, HotelId={1}, PlaneId={2}", 
                carTask.Result, hotelTask.Result, planeTask.Result);
            Console.WriteLine("Time taken: {0}", sw.Elapsed.Seconds);
        }
    }

    public class Sequential
    {
        public Sequential()
        {
        }

        public void TestMe()
        {
            var common = new Common();
            // Total time taken 5 + 8 + 3 = 16
            var stopWatch = Stopwatch.StartNew();
            int carId = common.BookCar();
            int hotelId = common.BookHotel();
            int planeId = common.BookPlane();
            Console.WriteLine("Finished booking. Time taken: {0}", stopWatch.Elapsed.Seconds);
        }
    }

    public class Common
    {
        public int BookPlane()
        {
            Console.WriteLine("Booking Plane...");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            Console.WriteLine("Done with plane.");
            return new Random().Next(100);
        }

        public int BookHotel()
        {
            Console.WriteLine("Booking Hotel...");
            Thread.Sleep(TimeSpan.FromSeconds(8));
            Console.WriteLine("Done with hotel.");
            return new Random().Next(100);
        }

        public int BookCar()
        {
            Console.WriteLine("Booking Car...");
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Console.WriteLine("Done with car.");
            return new Random().Next(100);
        }
    }

    public class TaskCreation
    {
        public void MethodOne()
        {
            var taskA = new Task(() => 
            {
                Console.WriteLine("Hello from Task A");
            });

            taskA.Start();
            Console.WriteLine("Hello from thread: {0}", Thread.CurrentThread.Name);
            taskA.Wait();
        }
    }
}