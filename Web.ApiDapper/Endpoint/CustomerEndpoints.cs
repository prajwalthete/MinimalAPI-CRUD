using Dapper;
using Web.ApiDapper.Models;
using Web.ApiDapper.Services;

namespace Web.ApiDapper.Endpoint
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder builder)
        {

            builder.MapGet("customers", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = "SELECT * FROM Customers";

                var customers = await connection.QueryAsync<Customer>(sql);

                return Results.Ok(customers);
            });



            builder.MapGet("customers/{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = """ 
                            SELECT Id, FirstName,LastName,Email,DateOfBirth
                            FROM Customers
                            WHERE Id = @CustomerId
                            """;

                /*The triple-double quotes allow you to define a multi-line string without having 
                 * to escape special characters like newline (\n) or quotation marks (").
                 * This makes SQL queries more readable and maintainable within your C# code.
                 * for ex-
                 * const string sql = @" 
                                 SELECT Id, FirstName, LastName, Email, DateOfBirth
                                 FROM Customers
                                 WHERE Id = @CustomerId ";
                  */
                var customer = await connection.QueryAsync<Customer>(sql, new { CustomerId = id });

                return customer is not null ? Results.Ok(customer) : Results.NotFound();
            });


            builder.MapPost("customers", async (Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = """
                                  INSERT INTO Customers(FirstName,LastName,Email,DateOfBirth)
                                  VALUES (@FirstName,@LastName,@Email,@DateOfBirth)
                                  """;
                await connection.ExecuteAsync(sql, customer);
                return Results.Ok();

            });

            builder.MapPut("customers/{id}", async (int id, Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                customer.Id = id;

                const string sql = """
                                  UPDATE Customers 
                                        SET FirstName=@FirstName,
                                        LastName=@LastName,
                                        Email=@Email,
                                        DateOfBirth=@DateOfBirth
                                  WHERE Id=@Id
                                  """;
                await connection.ExecuteAsync(sql, customer);
                return Results.NoContent();
            });

            builder.MapDelete("customers/{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = "DELETE FROM Customers WHERE Id = @CustomerId";
                await connection.ExecuteAsync(sql, new { customerId = id }); // annonomus Object
                return Results.NoContent();
            });

        }





    }
}
