using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Data.SqlClient;
using Dapper;

namespace Customer.Microservice.Features.CustomerFeatures.Commands
{
    public class UpdateCustomerCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>{
             private readonly ConnectionConfiguration connectionConfiguration;

            public UpdateCustomerCommandHandler(ConnectionConfiguration connectionConfiguration)
            {
                this.connectionConfiguration = connectionConfiguration;
            }

            public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
            {
                var customer = new Models.Customer();
                customer.Name = request.Name;
                customer.Address = request.Address;
                customer.Telephone = request.Telephone;
                customer.Email = request.Email;

                using (SqlConnection con = new SqlConnection(this.connectionConfiguration.ConnectionString))
                {
                    await con.OpenAsync();
                    var sqlQuery = $"Update Customer Set Name = @Name, Address = @Address, Telephone = @Telephone, Email = @Email Where Id = {request.Id}";
                    await con.ExecuteAsync(sqlQuery, customer);
                }

                return new Unit();
            }
        }
    }
}