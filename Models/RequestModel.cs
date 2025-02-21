using System;
using System.Collections.Generic;
using System.Linq;

namespace EDE.Function
{
    public class RequestModel
    {
        public Household? household { get; set; }
        public string? market { get; set; }
        public Place? place { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public string? order { get; set; }
        public int year { get; set; }
    }

    public class Household
    {
        public decimal income { get; set; }
        public List<Person>? people { get; set; }
        public bool has_married_couple { get; set; }
        public string? effective_date { get; set; }
    }

    public class Person
    {
        public bool aptc_eligible { get; set; }
        public string? age { get; set; }
        public bool has_mec { get; set; }
        public bool is_pregnant { get; set; }
        public bool is_parent { get; set; }
        public bool uses_tobacco { get; set; }
        public string? gender { get; set; }
        public string? utilization_level { get; set; }
    }

    public class Place
    {
        public string? countyfips { get; set; }
        public string? state { get; set; }
        public string? zipcode { get; set; }
    }

    public class InternalPlansDTO
    {
        public List<Plan> Plans { get; set; }
        public List<PlanBenefit> PlanBenefits { get; set; }
    }

     public class Plan
    {
        public string PlanName { get; set; }
        public string PlanCode { get; set; }
        public string FullPlanCode { get; set; }
        public string StateCode { get; set; }
        public string MedalDescription { get; set; }
        public string CarrierShortName { get; set; }
        public string LOBCode { get; set; }
        public string CSRLevelCode { get; set; }
        public bool EasyPricing { get; set; }
        public string RatingArea { get; set; }
        public string ServiceAreaID { get; set; }
        public string ProductDivision { get; set; }
        public string DiseaseManagementProgramsOffered { get; set; }
        public bool HSAEligible { get; set; }
        public decimal EHBPremiumCost { get; set; }
        public decimal MonthlyCost { get; set; }
        public string URL { get; set; }
    }

    public class PlanBenefit
    {
        public string FullPlanCode { get; set; }
        public string Name { get; set; }
        public string NetWorkValue { get; set; }
        public string FamilyNetworkValue { get; set; }
        public string OutOfNetworkValue { get; set; }
        public string FamilyOutOfNetworkValue { get; set; }
    }

    public class QouteResopnse
    {
        public List<PlanResponse> plans { get; set; }
        public int total { get; set; }
        public Rate_Area rate_area { get; set; }
        public Facet_Groups[] facet_groups { get; set; }
    }

      public class PlanResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public float premium { get; set; }
        public float premium_w_credit { get; set; }
        public float ehb_premium { get; set; }
        public float pediatric_ehb_premium { get; set; }
        public float aptc_eligible_premium { get; set; }
        public string metal_level { get; set; }
        public string type { get; set; }
        public string design_type { get; set; }
        public string state { get; set; }
        public string market { get; set; }
        public string product_division { get; set; }
        public string benefits_url { get; set; }
        public string network_url { get; set; }
        public Issuer issuer { get; set; }
        public bool hsa_eligible { get; set; }
        public string service_area_id { get; set; }
        public string[] disease_mgmt_programs { get; set; }
        public Quality_Rating quality_rating { get; set; }
        public Benefit[] benefits { get; set; }
        public Deductible[] deductibles { get; set; }
        public Moop[] moops { get; set; }
    }

     public class Issuer
    {
        public string name { get; set; }
    }

      public class Quality_Rating
    {
        public int global_rating { get; set; }
        public int clinical_quality_management_rating { get; set; }
        public int enrollee_experience_rating { get; set; }
        public int plan_efficiency_rating { get; set; }
    }

   public class Benefit
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string name { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="Benefit"/> is covered.
    /// </summary>
    /// <value><c>true</c> if covered; otherwise, <c>false</c>.</value>
    public bool covered { get; set; }
    /// <summary>
    /// Gets or sets the cost sharings.
    /// </summary>
    /// <value>The cost sharings.</value>
    public Cost_Sharings[] cost_sharings { get; set; }
    /// <summary>
    /// Gets or sets the explanation.
    /// </summary>
    /// <value>The explanation.</value>
    public string explanation { get; set; }
    /// <summary>
    /// Gets or sets the exclusions.
    /// </summary>
    /// <value>The exclusions.</value>
    public string exclusions { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance has limits.
    /// </summary>
    /// <value><c>true</c> if this instance has limits; otherwise, <c>false</c>.</value>
    public bool has_limits { get; set; }
    /// <summary>
    /// Gets or sets the limit unit.
    /// </summary>
    /// <value>The limit unit.</value>
    public string limit_unit { get; set; }
    /// <summary>
    /// Gets or sets the limit quantity.
    /// </summary>
    /// <value>The limit quantity.</value>
    public int limit_quantity { get; set; }
}


   public class Cost_Sharings
{
     /// <summary>
     /// Gets or sets the coinsurance rate.
     /// </summary>
     /// <value>The coinsurance rate.</value>
     public float coinsurance_rate { get; set; }
     /// <summary>
     /// Gets or sets the coinsurance options.
     /// </summary>
     /// <value>The coinsurance options.</value>
     public string coinsurance_options { get; set; }
     /// <summary>
     /// Gets or sets the copay amount.
     /// </summary>
     /// <value>The copay amount.</value>
     public float copay_amount { get; set; }
     /// <summary>
     /// Gets or sets the copay options.
     /// </summary>
     /// <value>The copay options.</value>
     public string copay_options { get; set; }
     /// <summary>
     /// Gets or sets the network tier.
     /// </summary>
     /// <value>The network tier.</value>
     public string network_tier { get; set; }
     /// <summary>
     /// Gets or sets the CSR.
     /// </summary>
     /// <value>The CSR.</value>
     public string csr { get; set; }
     /// <summary>
     /// Gets or sets the display string.
     /// </summary>
     /// <value>The display string.</value>
     public string display_string { get; set; }
}

    public class Deductible
    {
        public string type { get; set; }
        public string network_tier { get; set; }
        public string family_cost { get; set; }
        public float amount { get; set; }
        public string csr { get; set; }
    }

    public class Moop
    {
        public string type { get; set; }
        public string network_tier { get; set; }
        public string family_cost { get; set; }
        public float amount { get; set; }
    }

    public class Rate_Area
    {
        public string state { get; set; }
        public int area { get; set; }
    }

    public class Facet_Groups
    {
        public string name { get; set; }
        public Facet[] facets { get; set; }
    }

    public class Facet
    {
        public string value { get; set; }
        public int count { get; set; }
    }
}