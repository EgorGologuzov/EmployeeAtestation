namespace EmployeeAtestation.Models
{
    public class Block
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public int PassingScore { get; set; } = 80;
        public IList<string> Files { get; set; }
        public IList<Question> Questions { get; set; }

        public Block()
        {
            Files = new List<string>();
            Questions = new List<Question>();
        }
    }
}
