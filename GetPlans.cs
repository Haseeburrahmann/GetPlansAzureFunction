using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace EDE.Function
{
    public class GetPlansFunction
    {
        private readonly ILogger<GetPlansFunction> _logger;

        public GetPlansFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetPlansFunction>();
        }

        [Function("GetPlansFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req,
            FunctionContext context)
        {
            _logger.LogInformation("Processing request to get plans from CMS PUF.");

            try
            {
                // Read and parse the request body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var requestData = JsonSerializer.Deserialize<RequestModel>(requestBody)
                    ?? throw new ArgumentNullException("Request body is empty");

                if (requestData.household?.people == null || !requestData.household.people.Any())
                {
                    throw new ArgumentException("No people data found in request");
                }

                // Collect all ages into a comma-separated string
                string ages = string.Join(",", requestData.household.people.Select(p => p.age));

                // Get the SQL connection string
                string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString")
                    ?? throw new InvalidOperationException("SQL connection string is missing.");
                connectionString += ";Connection Timeout=300";

                // Fetch data from the database
                var plans = new List<Plan>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("EDE.prGetPlansFromCMSPUF", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 300;

                        command.Parameters.AddWithValue("@StateCode", requestData.place?.state ?? throw new ArgumentNullException("state"));
                        command.Parameters.AddWithValue("@Age", ages);
                        command.Parameters.AddWithValue("@CountyCode", int.Parse(requestData.place?.countyfips ?? throw new ArgumentNullException("countyfips")));
                        command.Parameters.AddWithValue("@Year", requestData.year);
                        command.Parameters.AddWithValue("@ZipCode", requestData.place?.zipcode ?? throw new ArgumentNullException("zipcode"));

                        _logger.LogInformation($"Executing SP with parameters: State={requestData.place.state}, Ages={ages}, County={requestData.place.countyfips}, Year={requestData.year}, Zip={requestData.place.zipcode}");

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var plan = new Plan
                                {
                                    PlanName = reader["PlanName"] as string,
                                    PlanCode = reader["PlanCode"] as string,
                                    FullPlanCode = reader["FullPlanCode"] as string,
                                    StateCode = reader["StateCode"] as string,
                                    MedalDescription = reader["MedalDescription"] as string,
                                    CarrierShortName = reader["CarrierShortName"] as string,
                                    LOBCode = reader["LOBCode"] as string,
                                    CSRLevelCode = reader["CSRLevelCode"] as string,
                                    EasyPricing = Convert.ToBoolean(reader["EasyPricing"]),
                                    RatingArea = reader["RatingArea"] as string,
                                    ServiceAreaID = reader["ServiceAreaID"] as string,
                                    ProductDivision = reader["ProductDivision"] as string,
                                    DiseaseManagementProgramsOffered = reader["DiseaseManagementProgramsOffered"] as string,
                                    HSAEligible = Convert.ToBoolean(reader["HSAEligible"]),
                                    EHBPremiumCost = Convert.ToDecimal(reader["EHBPremiumCost"]),
                                    MonthlyCost = Convert.ToDecimal(reader["MonthlyCost"])
                                };
                                plans.Add(plan);
                            }
                        }
                    }
                }

                // Map the plans to the desired output format
                var mappedPlans = plans.Select(p => new PlanResponse
                {
                    id = p.PlanCode,
                    name = p.PlanName,
                    premium = (float)p.MonthlyCost,
                    premium_w_credit = (float)p.MonthlyCost,
                    ehb_premium = (float)p.EHBPremiumCost,
                    pediatric_ehb_premium = 0,
                    aptc_eligible_premium = (float)p.EHBPremiumCost,
                    metal_level = p.MedalDescription.Replace("Expanded", "").Trim(),
                    type = p.LOBCode,
                    design_type = p.EasyPricing ? "DESIGN1" : "NOT_APPLICABLE",
                    state = p.StateCode,
                    market = "Individual",
                    product_division = p.ProductDivision,
                    benefits_url = "",
                    network_url = "",
                    issuer = new Issuer { name = p.CarrierShortName },
                    hsa_eligible = p.HSAEligible,
                    service_area_id = p.ServiceAreaID,
                    disease_mgmt_programs = p.DiseaseManagementProgramsOffered?.Split(',').Select(program => program.Trim()).ToArray(),
                    quality_rating = new Quality_Rating
                    {
                        global_rating = 0,
                        clinical_quality_management_rating = 0,
                        enrollee_experience_rating = 0,
                        plan_efficiency_rating = 0
                    },
                    benefits = null, // Assuming no benefits data is provided
                    deductibles = new Deductible[]
                    {
                        new Deductible
                        {
                            type = "Medical EHB Deductible",
                            network_tier = "In-Network",
                            family_cost = "",
                            amount = 0,
                            csr = p.CSRLevelCode
                        },
                        new Deductible
                        {
                            type = "Drug EHB Deductible",
                            network_tier = "In-Network",
                            family_cost = "",
                            amount = 0,
                            csr = p.CSRLevelCode
                        }
                    },
                    moops = new Moop[]
                    {
                        new Moop
                        {
                            type = "Maximum Out of Pocket for Medical and Drug EHB Benefits (Total)",
                            network_tier = "In-Network",
                            family_cost = "",
                            amount = 0
                        }
                    }
                }).ToList();

                // Apply pagination
                var paginatedResults = mappedPlans
                    .Skip(requestData.offset)
                    .Take(requestData.limit)
                    .ToList();

                // Apply ordering if specified
                if (!string.IsNullOrEmpty(requestData.order))
                {
                    paginatedResults = requestData.order.ToLower() == "asc"
                        ? paginatedResults.OrderBy(p => p.name).ToList()
                        : paginatedResults.OrderByDescending(p => p.name).ToList();
                }

                _logger.LogInformation($"Query completed. Retrieved {paginatedResults.Count} records.");

                // Create the response
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(paginatedResults);
                return response;
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

    // Define the Plan model
   
}