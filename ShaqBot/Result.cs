namespace ShaqBot;

public class Result
{
    public static int DepressionCount { get; set; }

    public void CountDepression(int answerId)
    {
        DepressionCount += answerId;
    }

    public static int ReturnResult()
    {
        return DepressionCount;
    }

}