using EmployeeAtestation.Models;

namespace EmployeeAtestation.ViewModels
{
    public class QuestionViewModel : ViewModelBase<Question>
    {
        public int Number
        {
            get => Model.Number;
            set { Model.Number = value; OnPropertyChanged(nameof(Number)); }
        }

        public string Text
        {
            get => Model.Text;
            set { Model.Text = value; OnPropertyChanged(nameof(Text)); }
        }

        public string Type
        {
            get => Model.Type;
            set { Model.Type = value; OnPropertyChanged(nameof(Type)); }
        }

        private ViewModelList<string, VariantViewModel> _variants;
        public ViewModelList<string, VariantViewModel> Variants
        {
            get => _variants;
            set { _variants = value; OnPropertyChanged(nameof(Variants)); }
        }

        public bool IsAnswerCorrect
        {
            get
            {
                bool isAnswerCorrect = true;

                foreach (var answer in Model.CorrectAnswers)
                {
                    if (Model.EmployeeAnswers.Contains(answer) == false)
                    {
                        isAnswerCorrect = false;
                        break;
                    }
                }

                return isAnswerCorrect;
            }
        }

        public QuestionViewModel(Question model) : base(model)
        {
            Model.Variants ??= new List<string>();
            Variants = new(Model.Variants);
            SetVariantsPrefix();
        }

        public void SetVariantsPrefix()
        {
            for (int i = 0; i < Variants.Count; i++ )
            {
                Variants[i].Prefix = $"{i + 1})";
            }
        }
    }
}
