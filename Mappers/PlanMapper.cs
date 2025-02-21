using EDE.Function.Models;
using EDE.Function.DTOs;


namespace EDE.Function.Mappers
{
    public static class PlanHelper
    {
        public static QouteResopnse PlanAndBenefitsMapper(InternalPlansDTO planBenefitsDTO)
        {
            try
            {
                var result = new QouteResopnse();
                if (planBenefitsDTO != null && planBenefitsDTO.Plans != null && planBenefitsDTO.Plans.Count > 0)
                {
                    result.plans = new List<Plan>();
                    foreach (var plans in planBenefitsDTO.Plans)
                    {
                        var plan = new Plan()
                        {
                            id = plans.PlanCode,
                            name = plans.PlanName,
                            premium = (float)plans.MonthlyCost,
                            premium_w_credit = (float)plans.MonthlyCost,
                            ehb_premium = (float)plans.EHBPremiumCost,
                            pediatric_ehb_premium = 0,
                            aptc_eligible_premium = (float)plans.EHBPremiumCost,
                            metal_level = plans.MedalDescription.Replace("Expanded", "").Trim(),
                            type = plans.LOBCode,
                            design_type = !plans.EasyPricing ? "NOT_APPLICABLE" : "DESIGN1",
                            state = plans.StateCode,
                            market = "Individual",
                            product_division = plans.ProductDivision,
                            benefits_url = plans.URL,
                            network_url = "",
                            issuer = new Issuer()
                            {
                                name = plans.CarrierShortName
                            },
                            hsa_eligible = plans.HSAEligible,
                            service_area_id = plans.ServiceAreaID,
                            disease_mgmt_programs = plans.DiseaseManagementProgramsOffered?.Split(",").Select(program => program.Trim()).ToArray()
                        };

                        int qualityRating = 0;
                        int.TryParse(plans.QualityGlobalRating, out qualityRating);
                        int memberExperienceRating = 0;
                        int.TryParse(plans.MemberExperienceRatingValue, out memberExperienceRating);
                        int medicalCareRating = 0;
                        int.TryParse(plans.MedicalCareRatingValue, out medicalCareRating);
                        int planAdministrationRating = 0;
                        int.TryParse(plans.PlanAdministrationRatingValue, out planAdministrationRating);
                        
                        plan.quality_rating = new Quality_Rating()
                        {
                            global_rating = qualityRating,
                            clinical_quality_management_rating = planAdministrationRating,
                            enrollee_experience_rating = memberExperienceRating,
                            plan_efficiency_rating = medicalCareRating,
                        };

                        if (planBenefitsDTO?.PlanBenefits != null)
                        {
                            var planBenefitsData = planBenefitsDTO.PlanBenefits?.Where(x => x.FullPlanCode == plans.FullPlanCode);
                            if (planBenefitsData.Any())
                                plan.benefits = new Benefit[planBenefitsData.Count()];
                            foreach (var planBenefit in planBenefitsData.Select((value, index) => new { value, index }))
                            {
                                var benefits = new Benefit()
                                {
                                    name = planBenefit.value.Name,
                                    covered = true,
                                    cost_sharings = new Cost_Sharings[]
                                    {
                                        new Cost_Sharings()
                                        {
                                            network_tier = "In-Network",
                                            display_string = planBenefit.value.NetWorkValue
                                        },
                                        new Cost_Sharings()
                                        {
                                            network_tier = "Out-of-Network",
                                            display_string = planBenefit.value.OutOfNetworkValue
                                        }
                                    }
                                };

                                plan.benefits[planBenefit.index] = benefits;
                            }
                        }

                        if (plans.IsHealthDrugBenefitIntegrated_DED)
                        {
                            plan.deductibles = new Deductible[]
                            {
                                new Deductible()
                                {
                                    type = "Combined Medical and Drug EHB Deductible",
                                    network_tier = "In-Network",
                                    family_cost = plans.BenefitText?.Replace("total", "").Trim(),
                                    amount = float.Parse(plans.Deductible ?? "0"),
                                    csr = plans.CSRLevelCode
                                }
                            };
                        }
                        else
                        {
                            plan.deductibles = new Deductible[plans.ProductDivision == "Dental" ? 1 : 2];
                            plan.deductibles[0] = new Deductible()
                            {
                                type = plans.ProductDivision == "Dental" ? 
                                    "Combined Medical and Drug EHB Deductible" : "Medical EHB Deductible",
                                network_tier = "In-Network",
                                family_cost = plans.BenefitText?.Replace("total", "").Trim(),
                                amount = float.Parse(plans.HealthDeductible ?? "0"),
                                csr = plans.CSRLevelCode
                            };

                            if (plans.ProductDivision != "Dental")
                            {
                                plan.deductibles[1] = new Deductible()
                                {
                                    type = "Drug EHB Deductible",
                                    network_tier = "In-Network",
                                    family_cost = plans.BenefitText?.Replace("total", "").Trim(),
                                    amount = float.Parse(plans.DrugDeductible ?? "0"),
                                    csr = plans.CSRLevelCode
                                };
                            }
                        }

                        plan.moops = new Moop[]
                        {
                            new Moop()
                            {
                                type = "Maximum Out of Pocket for Medical and Drug EHB Benefits (Total)",
                                network_tier = "In-Network",
                                family_cost = plans.BenefitText?.Replace("total", "").Trim(),
                                amount = float.Parse(plans.OOPValue ?? "0")
                            }
                        };
                        result.plans.Add(plan);
                    }

                    result.total = planBenefitsDTO.Plans.Count;
                    result.rate_area = new Rate_Area()
                    {
                        state = planBenefitsDTO.Plans.FirstOrDefault()?.StateCode,
                        area = string.IsNullOrEmpty(planBenefitsDTO.Plans.FirstOrDefault()?.RatingArea) ? 
                            0 : 
                            Convert.ToInt32(planBenefitsDTO.Plans.FirstOrDefault()?.RatingArea.Replace("Rating Area", "").Trim())
                    };

                    var facetGroupData = planBenefitsDTO.Plans
                        .GroupBy(x => x.CarrierShortName)
                        .Select(s => new { name = s.Key, count = s.Count() })
                        .ToList();

                    result.facet_groups = new Facet_Groups[1];
                    var facetGroup = new Facet_Groups()
                    {
                        name = "issuers"
                    };

                    if (facetGroupData.Count > 0)
                        facetGroup.facets = new Facet[facetGroupData.Count];

                    foreach (var facetGroups in facetGroupData.Select((value, index) => new { value, index }))
                    {
                        var facet = new Facet()
                        {
                            value = facetGroups.value.name,
                            count = facetGroups.value.count,
                        };
                        facetGroup.facets[facetGroups.index] = facet;
                    }
                    result.facet_groups[0] = facetGroup;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in PlanAndBenefitsMapper: {ex.Message}");
            }
        }
    }
}