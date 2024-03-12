using System.Collections;

namespace ShaqBot;

public class QuestionAnswerPair
{
    public int? Id { get; set; }
    public string? Question { get; set; }
    public List<string>? Answers { get; set; }
    
    public QuestionAnswerPair(string id, string question, List<string> answers)
    {
        Id = int.Parse(id);
        Question = question;
        Answers = answers;
    }
}
