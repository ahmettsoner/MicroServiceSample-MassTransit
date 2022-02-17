using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Microservice.Context;

namespace Product.Microservice.Features.ProductFeatures.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Models.Product>>
    {
        public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Models.Product>>{
            private readonly IApplicationContext context;

            public GetAllProductsQueryHandler(IApplicationContext context)
            {
                this.context = context;
            }

            public async Task<IEnumerable<Models.Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
            {
                var products = await context.Products.ToListAsync();

                return products;
            }
        }
    }
}