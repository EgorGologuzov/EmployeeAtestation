namespace EmployeeAtestation.Models
{
    public class QuestionResult
    {
        public int Number { get; set; }
        public IList<string> Answers { get; set; }
        public bool IsCorrect { get; set; }
    }
}
