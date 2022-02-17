using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Microservice.Context;

namespace Product.Microservice.Features.ProductFeatures.Queries
{
    public class GetProductByIdQuery : IRequest<Models.Product>
    {
        public int Id { get; set; }

        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Models.Product>{
            private readonly IApplicationContext context;

            public GetProductByIdQueryHandler(IApplicationContext context)
            {
                this.context = context;
            }

            public async Task<Models.Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                var product = await context.Products.Where(o => o.Id == request.Id)
                    .FirstOrDefaultAsync();

                return product;
            }
        }
    }
}