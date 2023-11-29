namespace CodeBase.Services;
public  static class ConfigIServiceCollection
{
    public static IServiceCollection ConfigurationIServiceCollection(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigurationServices(configuration);
        services.AddConfigurationDb(configuration);
        services.AddUserAuthorization(configuration);
        services.AddSwaggerClient();
        return services;
    }
    public static IServiceCollection ConfigurationServices(this  IServiceCollection services,IConfiguration configuration)
    {
        services.AddAuthentication();
        services.AddMemoryCache();
        services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IBookService,BookService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddUserAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var _settings = configuration.Get<GetSettings>();
        var key = Encoding.UTF8.GetBytes(_settings.JwtSettings.JwtSecret);
        services.AddSingleton(_settings.JwtSettings);
        services.AddScoped<ICurrentUserService,CurrentUserService>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience =_settings.JwtSettings.Audience,
                ValidIssuer = _settings.JwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtSettings.JwtSecret))
            };
        });


        ////services.AddSingleton<IAuthorizationMiddlewareResultHandler, CodeBaeAuthorizationMiddlewareResultHandler>();
        return services;
    }
    public static IServiceCollection AddSwaggerClient(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            //This is to generate the Default UI of Swagger Documentation  
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
            });
            // To Enable authorization using Swagger (JWT)  
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                      new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                }
            });
        });

        return services;
    }
    public static IServiceCollection AddConfigurationDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        services.AddDbContext<CodeBaseInterViewContext>(options =>
    options.UseSqlServer(connectionString));
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<CodeBaseInterViewContext>()
            .AddDefaultTokenProviders();
        ///services.AddAuthorization();
        return services;
    }
}
