using Microsoft.VisualStudio.TestTools.UnitTesting;
using YzcmCSharpUtility.DesignPattern;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace YzcmCSharpUtilityTest
{
    [TestClass]
    public class StateMachineTest
    {
        [TestMethod]
        public void StateMachine_01()
        {
            var config = new Config();
            var stateMachine = new StateMachine<Config>(new State_Test01());
            stateMachine.Run(config);

            Assert.AreEqual<int>(3, config.Sum);
            Console.WriteLine($"thread id :{Thread.CurrentThread.ManagedThreadId}, sum :" + config.Sum);
        }
        [TestMethod]
        public void StateMachine_02()
        {
            var config = new Config();
            var stateMachine = new StateMachine<Config>(new State_Test01(), new State_TestTearDown());
            stateMachine.Run(config);

            Assert.AreEqual<int>(4, config.Sum);
            Console.WriteLine($"thread id :{Thread.CurrentThread.ManagedThreadId}, sum :" + config.Sum);
        }
        class Config
        {
            internal int Sum = 0;
        }
        class State_Test01 : IState<Config>
        {
            IState<Config> IState<Config>.Execute(Config config)
            {
                Console.WriteLine(this.GetType());
                Console.WriteLine($"thread id :{Thread.CurrentThread.ManagedThreadId}, config.Sum :{config.Sum}");
                Interlocked.Increment(ref config.Sum);
                Console.WriteLine($"thread id :{Thread.CurrentThread.ManagedThreadId}, config.Sum :{config.Sum}");

                return new State_Test02();
            }
        }
        class State_Test02 : IState<Config>
        {
            async Task<IState<Config>> IState<Config>.ExecuteAsync(Config config)
            {
                Console.WriteLine(this.GetType());
                Console.WriteLine($"thread id :{Thread.CurrentThread.ManagedThreadId}, config.Sum :{config.Sum}");
                Interlocked.Increment(ref config.Sum);
                Console.WriteLine($"thread id :{Thread.CurrentThread.ManagedThreadId}, config.Sum :{config.Sum}");

                Console.WriteLine("1. thread id :" + Thread.CurrentThread.ManagedThreadId);
                await Task.Run(() =>
                {
                    Console.WriteLine("2-1. thread id :" + Thread.CurrentThread.ManagedThreadId);
                    Task.Delay(1000);
                    Console.WriteLine("2-2. thread id :" + Thread.CurrentThread.ManagedThreadId);
                });
                Console.WriteLine("3. thread id :" + Thread.CurrentThread.ManagedThreadId);

                return new State_Test03();
            }
        }
        class State_Test03 : IState<Config>
        {
            IState<Config> IState<Config>.Execute(Config config)
            {
                Console.WriteLine(this.GetType());
                Console.WriteLine($"thread id :{Thread.CurrentThread.ManagedThreadId}, config.Sum :{config.Sum}");
                Interlocked.Increment(ref config.Sum);
                Console.WriteLine($"thread id :{Thread.CurrentThread.ManagedThreadId}, config.Sum :{config.Sum}");

                return null;
            }
        }
        class State_TestTearDown : IState<Config>
        {
            IState<Config> IState<Config>.Execute(Config config)
            {
                Console.WriteLine(this.GetType());
                Console.WriteLine($"thread id :{Thread.CurrentThread.ManagedThreadId}, config.Sum :{config.Sum}");
                Interlocked.Increment(ref config.Sum);
                Console.WriteLine($"thread id :{Thread.CurrentThread.ManagedThreadId}, config.Sum :{config.Sum}");

                return null;
            }
        }
    }
}
