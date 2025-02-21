namespace EDE.Function.Enums
{

public static class CSRMappingtoCMSEnum
{
     public static Dictionary<string, string> CSR = new Dictionary<string, string>()
     {
         { "NON_EXCHANGE_VARIANT", "01" },
         { "ZeroCSR", "02" },
         { "LimitedCSR", "03" },
         { "CSR73", "04" },
         { "CSR87", "05" },
         { "CSR94", "06" }
     };
 
     public static Dictionary<string, string> PlanVariant = new Dictionary<string, string>()
     {
         // { "01", "NON_EXCHANGE_VARIANT" },
         { "01", "EXCHANGE_VARIANT_NO_CSR" },
         { "02", "ZERO_COST_SHARING_PLAN_VARIATION" },
         { "03", "LIMITED_COST_SHARING_PLAN_VARIATION" },
         { "04", "73PCT_AV_LEVEL_SILVER_PLAN_CSR" },
         { "05", "87PCT_AV_LEVEL_SILVER_PLAN_CSR" },
         { "06", "94PCT_AV_LEVEL_SILVER_PLAN_CSR" }
     };
}
}