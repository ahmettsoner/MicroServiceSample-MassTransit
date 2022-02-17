using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;

namespace Customer.Microservice.Features.CustomerFeatures.Queries
{
    public class GetAllCustomersQuery : IRequest<IEnumerable<Models.Customer>>
    {
        public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<Models.Customer>>{
             private readonly ConnectionConfiguration connectionConfiguration;

            public GetAllCustomersQueryHandler(ConnectionConfiguration connectionConfiguration)
            {
                this.connectionConfiguration = connectionConfiguration;
            }

           public async Task<IEnumerable<Models.Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<Models.Customer> customers;

                using (SqlConnection con = new SqlConnection(this.connectionConfiguration.ConnectionString))
                {
                    await con.OpenAsync();
                    var sqlQuery = "Select * From Customer";
                    customers = await con.QueryAsync<Models.Customer>(sqlQuery);
                }


                return customers;
            }
        }
    }
}