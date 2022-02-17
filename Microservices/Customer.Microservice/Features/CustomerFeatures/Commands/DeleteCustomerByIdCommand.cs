using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;

namespace Customer.Microservice.Features.CustomerFeatures.Commands
{
    public class DeleteCustomerByIdCommand : IRequest
    {
        public int Id { get; set; }

        public class DeleteCustomerByIdCommandHandler : IRequestHandler<DeleteCustomerByIdCommand>{
            private readonly ConnectionConfiguration connectionConfiguration;

            public DeleteCustomerByIdCommandHandler(ConnectionConfiguration connectionConfiguration)
            {
                this.connectionConfiguration = connectionConfiguration;
            }


            public async Task<Unit> Handle(DeleteCustomerByIdCommand request, CancellationToken cancellationToken)
            {
                using (SqlConnection con = new SqlConnection(this.connectionConfiguration.ConnectionString))
                    {
                        await con.OpenAsync();
                        var sqlQuery = $"Delete From Customer Where Id = {request.Id}";
                        await con.ExecuteAsync(sqlQuery);
                    }

                return new Unit();
            }
        }
    }
}