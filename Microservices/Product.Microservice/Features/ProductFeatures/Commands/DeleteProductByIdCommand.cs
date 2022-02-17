using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Microservice.Context;

namespace Product.Microservice.Features.ProductFeatures.Commands
{
    public class DeleteProductByIdCommand : IRequest<int>
    {
        public int Id { get; set; }

        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, int>{
            private readonly IApplicationContext context;

            public DeleteProductByIdCommandHandler(IApplicationContext context)
            {
                this.context = context;
            }

            public async Task<int> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
            {
                var product = await context.Products.FirstOrDefaultAsync(o=>o.Id == request.Id);

                if(product == null){
                    return default;
                }

                context.Products.Remove(product);
                await context.SaveChangesAsync();

                return product.Id;
            }
        }
    }
}