namespace EmployeeAtestation.Models
{
    public class TestResult
    {
        public Employee Employee { get; set; }
        public List<BlockResult> Blocks { get; set; } = new();
    }
}
