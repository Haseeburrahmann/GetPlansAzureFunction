namespace EDE.Function.DTOs
{
    public class InternalPlansDTO
    {
        public List<Plans> Plans { get; set; }
        public List<PlanBenefits> PlanBenefits { get; set; }
        public List<Benefits> Benefits { get; set; }
    }

    public class Plans
    {
        public string PlanName { get; set; }
        public string PlanCode { get; set; }
        public string FullPlanCode { get; set; }
        public int JetMUPlanID { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string LOBCode { get; set; }
        public int JetMULOBID { get; set; }
        public decimal MonthlyCost { get; set; }
        public int JetMUCarrierID { get; set; }
        public string CarrierShortName { get; set; }
        public int PlanType { get; set; }
        public int JetMUPlanMedalID { get; set; }
        public string MedalDescription { get; set; }
        public int CSRLevelID { get; set; }
        public string CSRLevelCode { get; set; }
        public int EXchangeTypeID { get; set; }
        public bool EasyPricing { get; set; }
        public string County { get; set; }
        public string RatingArea { get; set; }
        public string ServiceAreaID { get; set; }
        public string ProductDivision { get; set; }
        public string DiseaseManagementProgramsOffered { get; set; }
        public bool HSAEligible { get; set; }
        public decimal EHBPremiumCost { get; set; }
        public string QualityGlobalRating { get; set; }
        public string MemberExperienceRatingValue { get; set; }
        public string MedicalCareRatingValue { get; set; }
        public string PlanAdministrationRatingValue { get; set; }
        public string Deductible { get; set; }
        public string DeductibleValue { get; set; }
        public string OOPValue { get; set; }
        public string OutofPocketLimit { get; set; }
        public string URL { get; set; }
        public string SpecialistVisit { get; set; }
        public string GenericDrug { get; set; }
        public string DoctorVisit { get; set; }
        public bool IsHealthDrugBenefitIntegrated_DED { get; set; }
        public string HealthDeductible { get; set; }
        public string DrugDeductible { get; set; }
        public bool IsHealthDrugBenefitIntegrated_OOP { get; set; }
        public string HealthOOP { get; set; }
        public string DrugOOP { get; set; }
        public string BenefitText { get; set; }
    }

    public class PlanBenefits
    {
        public int JetMUPlanBenefitID { get; set; }
        public string NetWorkValue { get; set; }
        public string DisplayNetWorkValue { get; set; }
        public string OutOfNetworkValue { get; set; }
        public string FamilyNetworkValue { get; set; }
        public string DisplayFamilyNetworkValue { get; set; }
        public string FamilyOutOfNetworkValue { get; set; }
        public int JetMUBenefitID { get; set; }
        public string Name { get; set; }
        public int JetMUPlanID { get; set; }
        public string FieldType { get; set; }
        public string ForFilterFieldType { get; set; }
        public string FullPlanCode { get; set; }
    }

    public class Benefits
    {
        public int JetMUBenefitID { get; set; }
        public string Name { get; set; }
        public string MaxNetWorkValue { get; set; }
        public string MaxFamilyNetWorkValue { get; set; }
        public string FieldType { get; set; }
    }
}