namespace EmployeeAtestation.Models
{
    public class BlockResult
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Dictionary<int, IList<string>> Answers { get; set; } = new();
        public double Result { get; set; }
    }
}
