using EFGetStarted;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public string DbPath { get; }

    public BloggingContext()
    {
        IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        IConfiguration config = configBuilder.Build();

        DbPath = config.GetConnectionString("DbBlog");
    }

    #region Required
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new BlogEntityTypeConfiguration().Configure(modelBuilder.Entity<Blog>());
    }
    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(DbPath);
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; } = new();
}

[Table("Posts")]
public class Post
{
    public int PostId { get; set; }

    [Required]
    [MaxLength(10, ErrorMessage = "Título muito longo. O limite é de 100 caracteres")]
    public string Title { get; set; }
    [MaxLength(500, ErrorMessage = "O tamanho máximo foi ultrapassado")]
    public string Content { get; set; }

    [Required]
    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}