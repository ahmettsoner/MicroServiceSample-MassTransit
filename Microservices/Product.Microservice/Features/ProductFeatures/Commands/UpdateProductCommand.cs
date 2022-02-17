using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Microservice.Context;

namespace Product.Microservice.Features.ProductFeatures.Commands
{
    public class UpdateProductCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>{
            private readonly IApplicationContext context;

            public UpdateProductCommandHandler(IApplicationContext context)
            {
                this.context = context;
            }

            public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                var product = await context.Products.FirstOrDefaultAsync(o=>o.Id == request.Id);
                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;

                if(product == null){
                    return default;
                }

                await context.SaveChangesAsync();

                return product.Id;
            }
        }
    }
}