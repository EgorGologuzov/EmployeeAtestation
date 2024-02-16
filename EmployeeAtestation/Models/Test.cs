namespace EmployeeAtestation.Models
{
    public class Test
    {
        public string Title { get; set; }
        public IList<Block> Blocks { get; set; }

        public Test()
        {
            Blocks = new List<Block>();
        }
    }
}
