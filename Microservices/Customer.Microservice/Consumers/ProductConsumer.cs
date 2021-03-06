using System;
using System.Threading.Tasks;
using MassTransit;
using Shared.Models.Models;

namespace Customer.Microservice.Consumers
{
    public class ProductConsumer : IConsumer<CustomerProduct>
    {
        public async Task Consume(ConsumeContext<CustomerProduct> context)
        {
            await Task.Run(() =>{
                var obj = context.Message;
                Console.WriteLine("Consumer:" + obj.ProductName);
            });
        }
    }
}