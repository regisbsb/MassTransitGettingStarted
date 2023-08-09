namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using System;


    public class GettingStartedConsumer :
        IConsumer<GettingStarted>
    {
        public Task Consume(ConsumeContext<GettingStarted> context)
        {
            //if (context.GetRetryAttempt() < 5)
            throw new Exception("Erro");
            Console.WriteLine("Received Text: {0}", context.Message.Value);
            return Task.CompletedTask;
        }
    }


    public class FaultyGettingStartedConsumer :
        IConsumer<Fault<GettingStarted>>
    {

        public Task Consume(ConsumeContext<Fault<GettingStarted>> context)
        {
            Console.WriteLine("Fault: {0}", context.Message.Message.Value);
            return Task.CompletedTask;
        }

    }
}