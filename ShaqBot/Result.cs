namespace ShaqBot;

public class Result
{
    public int DepressionCount { get; set; }

    public void CountDepression(int answerId)
    {
        DepressionCount += answerId;
    }

    public int ReturnResult()
    {
        return DepressionCount;
    }

}