namespace ShaqBot.Entities;

public class LastMessage : BaseEntity
{
    public long ChatId { get; set; }
    public int MessageId { get; set; }
    
    public DateTime DateSent { get; set; }
}

public class MessageRepository
{
    private readonly DataContext _dbContext;

    public MessageRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public LastMessage GetLastMessageForChat(long chatId)
    {
        return _dbContext.LastMessages
            .Where(m => m.ChatId == chatId)
            .OrderByDescending(m => m.DateSent)
            .FirstOrDefault();
    }

    public void SaveLastMessage(LastMessage lastMessage)
    {
        _dbContext.LastMessages.Add(lastMessage);
        _dbContext.SaveChanges();
    }

    public void DeleteLastMessage(LastMessage lastMessage)
    {
        _dbContext.LastMessages.Remove(lastMessage);
        _dbContext.SaveChanges();
    }
}