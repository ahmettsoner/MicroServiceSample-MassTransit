using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Microservice.Context;

namespace Product.Microservice.Features.ProductFeatures.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>{
            private readonly IApplicationContext context;

            public CreateProductCommandHandler(IApplicationContext context)
            {
                this.context = context;
            }

            public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var product = new Models.Product();
                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;

                context.Products.Add(product);
                await context.SaveChangesAsync();

                return product.Id;
            }
        }
    }
}