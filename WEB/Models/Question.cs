using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<Option> Options { get; set; }
        public int CorrectAnswer { get; set; }
        [ForeignKey("QuizTable")]
        public int QuizTableId { get; set; }
        public virtual QuizTable? QuizTable { get; set; }
    }
}
