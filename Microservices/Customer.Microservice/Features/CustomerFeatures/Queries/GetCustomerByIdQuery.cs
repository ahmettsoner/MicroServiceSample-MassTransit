using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;

namespace Customer.Microservice.Features.CustomerFeatures.Queries
{
    public class GetCustomerByIdQuery : IRequest<Models.Customer>
    {
        public int Id { get; set; }

        public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Models.Customer>{
            private readonly ConnectionConfiguration connectionConfiguration;

            public GetCustomerByIdQueryHandler(ConnectionConfiguration connectionConfiguration)
            {
                this.connectionConfiguration = connectionConfiguration;
            }

            public async Task<Models.Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
            {
                Models.Customer customer;

                using (SqlConnection con = new SqlConnection(this.connectionConfiguration.ConnectionString))
                {
                    await con.OpenAsync();
                    var sqlQuery = $"Select * From Customer Where Id = {request.Id}";
                    customer = await con.QuerySingleOrDefaultAsync<Models.Customer>(sqlQuery);
                }

                return customer;
            }
        }
    }
}