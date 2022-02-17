using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using System;

namespace CustomerProduct.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerProductController : ControllerBase
    {
        private readonly IBus busService;

        public CustomerProductController(IBus busService)
        {
            this.busService = busService;
        }


        [HttpPost]
        public async Task<string> CreateProduct(Shared.Models.Models.CustomerProduct product){
            if(product != null){
                product.AddedOnDate = DateTime.Now;
                Uri uri = new Uri("rabbitmq://localhost/productQueue");
                var endpoint = await busService.GetSendEndpoint(uri);
                await endpoint.Send(product);
                return "true";
            }

            return "false";
        }

    }
}