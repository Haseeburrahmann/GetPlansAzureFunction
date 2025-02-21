using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System.Net;
using EDE.Function.Models;
using EDE.Function.DTOs;
using EDE.Function.Mappers;
using System.Data;
using Dapper;
using EDE.Function.Enums;

namespace EDE.Function
{
    public class GetPlans
    {
        private readonly ILogger<GetPlans> _logger;

        public GetPlans(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetPlans>();
        }

        [Function("GetPlans")]
        public async Task<HttpResponseData> RunTo(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Processing request to get plans from CMS PUF.");

            try
            {
                // Read and parse the request body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var request = JsonSerializer.Deserialize<RequestModel>(requestBody)
                    ?? throw new ArgumentNullException("Request body is empty");

                if (request.household?.people == null || !request.household.people.Any())
                {
                    throw new ArgumentException("No people data found in request");
                }

                string ages = string.Join(",", request.household.people.Select(p => p.age));
                string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString")
                    ?? throw new InvalidOperationException("SQL connection string is missing.");
                connectionString += ";Connection Timeout=300";

                var internalPlanDto = new InternalPlansDTO();

             using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    

                        _logger.LogInformation($"Executing SP with parameters: State={request.place.state}, Ages={ages}, County={request.place.countyfips}, Year={request.year}, Zip={request.place.zipcode}");
                       
                        var planSpParams = new
                        {
                            Age = ages,
                            StateCode = request.place?.state,
                            CountyCode = request.place?.countyfips,
                            Year = request.year,
                            ZipCode = request.place?.zipcode,
                            CSRCode = string.IsNullOrEmpty(request.csr_override) ? "01" : CSRMappingtoCMSEnum.CSR[request.csr_override],
                            CoverageDate = DateTime.Parse(new DateTime(request.year, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToString("yyyy-MM-dd"), Thread.CurrentThread.CurrentCulture),
                        };

                       
                            var reader = await connection.QueryMultipleAsync(
                                "EDE.prGetPlansFromCMSPUF",
                                param: planSpParams,
                                commandType: CommandType.StoredProcedure);

                            internalPlanDto.Plans = reader.Read<Plans>().ToList();
                            internalPlanDto.PlanBenefits = reader.Read<PlanBenefits>().ToList();
                            //internalPlanDto.Benefits = reader.Read<Benefits>().ToList();
                         
                   }
                var response = PlanHelper.PlanAndBenefitsMapper(internalPlanDto);

                // Apply pagination
                var paginatedPlans = request.order?.ToLower() == "asc"
                    ? response.plans.OrderBy(p => p.name).Skip(request.offset).Take(request.limit).ToList()
                    : request.order?.ToLower() == "desc"
                        ? response.plans.OrderByDescending(p => p.name).Skip(request.offset).Take(request.limit).ToList()
                        : response.plans.Skip(request.offset).Take(request.limit).ToList();

                response.plans = paginatedPlans;
                response.total = paginatedPlans.Count;

                _logger.LogInformation($"Query completed. Retrieved {paginatedPlans.Count} records.");

                var httpResponse = req.CreateResponse(HttpStatusCode.OK);
                await httpResponse.WriteAsJsonAsync(response);
                return httpResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting plans: {ex.Message}");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync(ex.Message);
                return response;
            }
        }
    }
}