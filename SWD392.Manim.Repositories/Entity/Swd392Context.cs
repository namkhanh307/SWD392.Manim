using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace SWD392.Manim.Repositories.Entity;

public class Swd392Context : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaims, ApplicationUserRoles, ApplicationUserLogins, ApplicationRoleClaims, ApplicationUserTokens>
{
    public Swd392Context()
    {
    }

    public Swd392Context(DbContextOptions<Swd392Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Chapter> Chapters { get; set; }
    public virtual DbSet<Deposit> Deposits { get; set; }
    public virtual DbSet<Problem> Problems { get; set; }
    public virtual DbSet<Solution> Solutions { get; set; }
    public virtual DbSet<SolutionOutput> SolutionOutputs { get; set; }
    public virtual DbSet<SolutionType> SolutionTypes { get; set; }
    public virtual DbSet<ProblemParameter> SolutionParameters { get; set; }
    public virtual DbSet<Parameter> Parameters { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<Topic> Topics { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<Wallet> Wallets { get; set; }
    public virtual DbSet<OTP> OTPs { get; set; }

    private string? GetConnectionString()
    {
        //IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SWD392.Manim.API")).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        //return configuration["ConnectionStrings:DefautDB"];
        return "server=manim.database.windows.net;database=swd-manim;uid=adminmanim;pwd=Jpassword@;TrustServerCertificate=True";

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(GetConnectionString());
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            string tableName = entityType.GetTableName() ?? "";
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
        modelBuilder.Entity<ProblemParameter>()
            .HasKey(sp => new { sp.ParameterId, sp.ProblemId });

        modelBuilder.Entity<ProblemParameter>()
            .HasOne(sp => sp.Parameter)
            .WithMany(s => s.ProblemParameters)
            .HasForeignKey(sp => sp.ParameterId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        modelBuilder.Entity<ProblemParameter>()
            .HasOne(sp => sp.Problem)
            .WithMany(p => p.ProblemParameters)
            .HasForeignKey(sp => sp.ProblemId)
            .OnDelete(DeleteBehavior.Cascade); // Allows cascade delete for Parameter

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<Wallet>(w => w.UserId);

        modelBuilder.Entity<Solution>()
            .HasOne(a => a.User)
            .WithMany(w => w.Solutions)
            .HasForeignKey(w => w.UserId);

        modelBuilder.Entity<Solution>().ToTable("Solutions");
        modelBuilder.Entity<SolutionOutput>().ToTable("SolutionOutputs");
        modelBuilder.Entity<Parameter>().ToTable("Parameters");
        modelBuilder.Entity<SolutionType>().ToTable("SolutionTypes");
        modelBuilder.Entity<ProblemParameter>().ToTable("ProblemParameters");

        modelBuilder.Entity<SolutionType>()
            .HasOne(s => s.Solution)
            .WithOne(so => so.SolutionType)
            .HasForeignKey<Solution>(so => so.SolutionTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Solution>()
            .HasOne(s => s.SolutionOutput)
            .WithOne(so => so.Solution)
            .HasForeignKey<SolutionOutput>(so => so.SolutionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Deposit>()
            .HasOne(sp => sp.User)
            .WithMany(p => p.Deposits)
            .HasForeignKey(sp => sp.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Allows cascade delete for Parameter
    }

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
