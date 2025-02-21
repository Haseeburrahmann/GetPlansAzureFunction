 namespace DTOs
    {
        public class InternalPlansDTO
        {
            public List<PlanDTO> Plans { get; set; }
            public List<PlanBenefitDTO> PlanBenefits { get; set; }
        }

        public class PlanDTO
        {
            public string PlanCode { get; set; }
            public string PlanName { get; set; }
            public decimal MonthlyCost { get; set; }
            public decimal EHBPremiumCost { get; set; }
            public string MedalDescription { get; set; }
            public string LOBCode { get; set; }
            public bool EasyPricing { get; set; }
            public string StateCode { get; set; }
            public string ProductDivision { get; set; }
            public string URL { get; set; }
            public string CarrierShortName { get; set; }
            public bool HSAEligible { get; set; }
            public string ServiceAreaID { get; set; }
            public string DiseaseManagementProgramsOffered { get; set; }
            public string QualityGlobalRating { get; set; }
            public string MemberExperienceRatingValue { get; set; }
            public string MedicalCareRatingValue { get; set; }
            public string PlanAdministrationRatingValue { get; set; }
            public string FullPlanCode { get; set; }
            public string BenefitText { get; set; }
            public bool IsHealthDrugBenefitIntegrated_DED { get; set; }
            public string CSRLevelCode { get; set; }
            public decimal Deductible { get; set; }
            public decimal HealthDeductible { get; set; }
            public decimal DrugDeductible { get; set; }
            public decimal OOPValue { get; set; }
            public string RatingArea { get; set; }
        }

        public class PlanBenefitDTO
        {
            public string FullPlanCode { get; set; }
            public string Name { get; set; }
            public string NetWorkValue { get; set; }
            public string OutOfNetworkValue { get; set; }
            public string FamilyNetworkValue { get; set; }
            public string FamilyOutOfNetworkValue { get; set; }
        }

        
    }