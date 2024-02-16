namespace EmployeeAtestation.Models
{
    public class Question
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public int Score { get; set; }
        public IList<string> Variants { get; set; }
        public IList<string> CorrectAnswers { get; set; }
        public IList<string> EmployeeAnswers { get; set; }

        public Question()
        {
            Variants = new List<string>();
            CorrectAnswers = new List<string>();
            EmployeeAnswers = new List<string>();
        }
    }
}
