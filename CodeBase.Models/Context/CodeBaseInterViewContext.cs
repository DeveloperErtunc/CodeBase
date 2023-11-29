namespace CodeBase.Models.Context;
public class CodeBaseInterViewContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public CodeBaseInterViewContext(DbContextOptions<CodeBaseInterViewContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(b =>
        {
            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<ApplicationRole>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });


    }
    public async Task SeedData()
    {
        try
        {
            var count = await Roles.CountAsync();
            if (count > 0)
                return;
            var userId = "8e445865-a24d-4543-a6c6-9443d048cdb9";
            var roles = new List<ApplicationRole>()
            {
              new ApplicationRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = AccountRoles.Admin.ToString(), NormalizedName = AccountRoles.Admin.ToString().ToUpper().Replace("İ", "I") },
              new ApplicationRole { Id = "1c5e174e-3b0e-446f-86af-483d56fd7210", Name = AccountRoles.Customer.ToString(), NormalizedName = AccountRoles.Customer.ToString().ToUpper().Replace("İ", "I") }
            };
            var hasher = new PasswordHasher<IdentityUser>();
            var users = new List<ApplicationUser>{
                new ApplicationUser
                {
                    Id = userId, // primary key
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "admin@gmail.com".ToUpper().Replace("İ", "I"),
                    Email = "admin@gmail.com",
                    Name = "Admin",
                    Surname = "Admin",
                    PasswordHash = hasher.HashPassword(null, "Admin_135"),
                    UserRoles = new List<ApplicationUserRole>{  new ApplicationUserRole
                            {
                                RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                                UserId = userId
                            }
                    }
                }
            };
            var books = new List<Book>
            {
                new Book
                {
                    Author ="İlber Ortaylı",
                    Name ="İmzalı - Cumhuriyet'in Doğuşu - Kurtuluş ve Kuruluş Yılları",
                    Price = 75,
                    Id = Guid.NewGuid(),
                },
                new Book{
                    Author ="İlber Ortaylı",
                    Name ="Yakın Tarihin Gerçekleri",
                    Price = 87  ,
                    Id = Guid.NewGuid(),
                },
                new Book{
                    Author ="İlber Ortaylı",
                    Name ="İstanbul - Şehir ve Kültür",
                    Price = 75,
                    Id = Guid.NewGuid(),
                },
                new Book{
                    Author ="Ercan Nurcan Yılmaz",
                    Name ="Örneklerle Uygulamalı C ve C++",
                    Price = 75,
                    Id = Guid.NewGuid(),
                }
            };
            books.ForEach(x => x.Created(userId));
            books.ForEach(x => x.UpdateStock(100, null, userId));
            //HttpClient client = new HttpClient();
            //var result = await client.GetAsync("https://www.googleapis.com/books/v1/volumes&filter=free-ebooks&key=yourAPIKey");
            //var booksEntegre = await result.Content.ReadFromJsonAsync<Root>();
            //var books = booksEntegre.items.Where(x => x.saleInfo != null && (x.volumeInfo.authors != null && !x.volumeInfo.authors.Any(x => string.IsNullOrEmpty(x))))
            //   .Select(x => new Book
            //   {
            //       Author = string.Join(",", x.volumeInfo.authors.Select(x => x)),
            //       Name = x.volumeInfo.title,
            //       Price =
            //       x?.saleInfo?.listPrice?.amount != null ? Convert.ToDecimal(x.saleInfo.listPrice.amount) :
            //       x?.saleInfo?.retailPrice?.amount != null ? Convert.ToDecimal(x.saleInfo.retailPrice.amount) :
            //       x?.saleInfo?.retailPrice?.amountInMicros != null ? Convert.ToDecimal(x.saleInfo.retailPrice.amountInMicros) : 0,
            //       Id = Guid.NewGuid(),
            //   }).ToList();

            //books.ForEach(x => x.Created("8e445865-a24d-4543-a6c6-9443d048cdb9"));
            Roles.AddRange(roles);
            Books.AddRange(books);
            Users.AddRange(users);
            await SaveChangesAsync();
        }
        catch (Exception ex)
        {

        }
    }
    public DbSet<Order>? Orders { get; set; }
    public DbSet<Book>? Books { get; set; }
    public DbSet<InventoryTransaction>? InventoryTransactions { get; set; }
    public DbSet<OrderItem>? OrderItems { get; set; }
}
