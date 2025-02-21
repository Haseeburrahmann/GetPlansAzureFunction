namespace EDE.Function.Models
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

        public string? csr_override { get; set; }
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
}