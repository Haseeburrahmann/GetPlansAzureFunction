namespace EDE.Function.Models
{
    public class QouteResopnse
    {
        public List<Plan> plans { get; set; }
        public int total { get; set; }
        public Rate_Area rate_area { get; set; }
        public Facet_Groups[] facet_groups { get; set; }
        public Ranges ranges { get; set; }
    }

    public class Plan
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
        public bool is_ineligible { get; set; }
        public bool rx_3mo_mail_order { get; set; }
        public bool covers_nonhyde_abortion { get; set; }
        public string ExceptionType { get; set; }
        public bool isSuspendedPlan { get; set; }
        public bool is_standardized_plan { get; set; }
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

    public class Ranges
    {
        // Add properties if needed
    }

    public class Moop
    {
        public string type { get; set; }
        public float amount { get; set; }
        public string csr { get; set; }
        public string network_tier { get; set; }
        public string family_cost { get; set; }
        public bool individual { get; set; }
        public bool family { get; set; }
        public string display_string { get; set; }
    }

    public class Deductible
    {
        public string type { get; set; }
        public float amount { get; set; }
        public string csr { get; set; }
        public string network_tier { get; set; }
        public string family_cost { get; set; }
        public bool individual { get; set; }
        public bool family { get; set; }
        public string display_string { get; set; }
    }

    public class Cost_Sharings
    {
        public float coinsurance_rate { get; set; }
        public string coinsurance_options { get; set; }
        public float copay_amount { get; set; }
        public string copay_options { get; set; }
        public string network_tier { get; set; }
        public string csr { get; set; }
        public string display_string { get; set; }
    }

    public class Benefit
    {
        public string name { get; set; }
        public bool covered { get; set; }
        public Cost_Sharings[] cost_sharings { get; set; }
        public string explanation { get; set; }
        public string exclusions { get; set; }
        public bool has_limits { get; set; }
        public string limit_unit { get; set; }
        public int limit_quantity { get; set; }
    }

    public class Quality_Rating
    {
        public bool available { get; set; }
        public int year { get; set; }
        public int global_rating { get; set; }
        public int clinical_quality_management_rating { get; set; }
        public int enrollee_experience_rating { get; set; }
        public int plan_efficiency_rating { get; set; }
        public string global_not_rated_reason { get; set; }
        public string clinical_quality_management_not_rated_reason { get; set; }
        public string enrollee_experience_not_rated_reason { get; set; }
        public string plan_efficiency_not_rated_reason { get; set; }
    }

    public class Issuer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string[] eligible_dependents { get; set; }
        public string state { get; set; }
        public string individual_url { get; set; }
        public string shop_url { get; set; }
        public string toll_free { get; set; }
        public string tty { get; set; }
    }
}