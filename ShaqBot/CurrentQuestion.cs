using ShaqBot.Migrations;

namespace ShaqBot;

public class CurrentQuestion
{
    public int _currentQuestionIndex = -1;

    public void Add()
    {
        _currentQuestionIndex++;
    }
}