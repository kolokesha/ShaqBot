using Microsoft.EntityFrameworkCore;
using ShaqBot.Entities;

namespace ShaqBot;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public  DbSet<PollChatMap> PollChatMaps { get; set; }
    public DbSet<LastMessage> LastMessages { get; set; }
}