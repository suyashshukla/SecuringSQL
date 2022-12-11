using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.Common;
using System.Data.SqlClient;

namespace SecuringSQL
{
    /// <summary>
    /// SQL Connection Health Check
    /// </summary>
    public class SQLConnectionHealthCheck : IHealthCheck
    {
        /// <summary>
        /// The SQL DB Connection String
        /// </summary>
        private string? ConnectionString { get; set; }

        /// <summary>
        /// The SQL Connection Health Check Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public SQLConnectionHealthCheck(IConfiguration configuration)
        {
            this.ConnectionString = configuration["DBString"];
        }

        /// <summary>
        /// This function verifies whether the application is able to make a connection to SQL Server
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT 1";
                    await command.ExecuteNonQueryAsync(cancellationToken);

                }
                catch (DbException ex)
                {
                    return HealthCheckResult.Unhealthy(ex.Message);
                }
            }

            return HealthCheckResult.Healthy("Coonection established successfully!");
        }
    }
}
