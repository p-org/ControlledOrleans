using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.TestingHost;
using Xunit;
using Nekara.Client;
using System.Linq;
using Assert = Xunit.Assert;

namespace XUnitTestProject1
{

    public interface IHello : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
    }

    public class HelloGrain : Orleans.Grain, IHello
    {
        Task<string> IHello.SayHello(string greeting)
        {
            // logger.LogInformation($"SayHello message received: greeting = '{greeting}'");
            return Task.FromResult($"Hello, World");
        }
    }

    public interface IAccountGrain11 : IGrainWithGuidKey
    {
        Task Withdraw(uint amount);

        Task Deposit(uint amount);

        Task<uint> GetBalance();
    }

    public class AccountGrain11 : Grain, IAccountGrain11
    {
        private uint _balance;

        public AccountGrain11()
        {
            this._balance = 900;
        }

        Task IAccountGrain11.Deposit(uint amount)
        {
            return Task.Run(() => this._balance = this._balance + amount);
        }

        Task IAccountGrain11.Withdraw(uint amount)
        {
            return Task.Run(() => this._balance = this._balance - amount);
        }

        Task<uint> IAccountGrain11.GetBalance()
        {
            return Task.FromResult(this._balance);
        }
    }

    public class UnitTest1
    {
        [Fact]
        public async Task SaysHelloCorrectly()
        {
            var _api = RuntimeEnvironment.Client.Api;
            /* var nekara = client.Api;
            nekara.CreateTask(); */


            var _t1 = new TestClusterBuilder();
            var cluster = _t1.Build();
            cluster.Deploy();

            var friend = cluster.GrainFactory.GetGrain<IHello>(0);

            var greeting = await friend.SayHello("Good morning, my friend!");

            cluster.StopAllSilos();

            Assert.Equal("Hello, World", greeting);
        }

        [Fact]
        public async Task DoAccountingCorrectly()
        {
            NekaraClient client = RuntimeEnvironment.Client;
            client.InitSessionDir();
            var _t3 = RuntimeEnvironment.Client.Api;
            _t3.CreateTask();




            var _t1 = new TestClusterBuilder();
            var cluster = _t1.Build();
            cluster.Deploy();

            Guid acc1 = Guid.NewGuid();

            _ = Task.WhenAll(
                  cluster.GrainFactory.GetGrain<IAccountGrain11>(acc1).Withdraw(200),
                   cluster.GrainFactory.GetGrain<IAccountGrain11>(acc1).Deposit(100));

            uint fromBalance = await cluster.GrainFactory.GetGrain<IAccountGrain11>(acc1).GetBalance();

            cluster.StopAllSilos();

            Assert.Equal(900u, fromBalance);
        }
    }
}
