using WEB.Models;

namespace WEB.ViewModel
{
    public class AddQuizVM
    {
        public string? QuizName { get; set; }

        public List<Question> QuestionsText { get; set; }

        public List<Option> Options { get; set; }
    }
}
