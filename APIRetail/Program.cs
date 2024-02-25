using APIRetail.CacheList;
using APIRetail.Crypto;
using APIRetail.Jobs;
using APIRetail.Jobs.IJobs;
using APIRetail.Middleware;
using APIRetail.Models.Database;
using APIRetail.Repository;
using APIRetail.Repository.IRepository;
using APIRetail.Services;
using APIRetail.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
DecryptMD5 decryptMD5 = new DecryptMD5();
var connectionString = decryptMD5.MD5Decrypt(builder.Configuration.GetConnectionString("retailContext").ToString());
MasterList.ConnectMySQL = connectionString;
TransactionList.ConnectMySQL = connectionString;
builder.Services.AddDbContext<retail_systemContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<ICrypto, CryptoRepository>();
builder.Services.AddScoped<ILogin, LoginRepository>();
builder.Services.AddScoped<IBranch, BranchRepository>();
builder.Services.AddScoped<ICompany, CompanyRepository>();
builder.Services.AddScoped<ICustomer, CustomerRepository>();
builder.Services.AddScoped<ISupplier, SupplierRepository>();
builder.Services.AddScoped<IDailyStock, DailyStockRepository>();
builder.Services.AddScoped<IMenu, MenuRepository>();
builder.Services.AddScoped<IMonthlyStock, MonthlyStockRepository>();
builder.Services.AddScoped<IProduct, ProductRepository>();
builder.Services.AddScoped<IProductType, ProductTypeRepository>();
builder.Services.AddScoped<IProfil, ProfilRepository>();
builder.Services.AddScoped<IProfilMenu, ProfilMenuRepository>();
builder.Services.AddScoped<IProfilUser, ProfilUserRepository>();
builder.Services.AddScoped<IStockOpname, StockOpnameRepository>();
builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<IUserMenu, UserMenuRepository>();
builder.Services.AddScoped<IGenerateNumber, GenerateNumberRepository>();
builder.Services.AddScoped<ISalesOrder, SalesOrderRepository>();
builder.Services.AddScoped<IPurchaseOrder, PurchaseOrderRepository>();
builder.Services.AddScoped<IRankingProduct, RankingProductRepository>();
builder.Services.AddScoped<IMessage, MessageRepository>();
builder.Services.AddScoped<ISchedule, ScheduleRepository>();
builder.Services.AddScoped<ISendWhatsApp, SendWhatsAppRepository>();
builder.Services.AddScoped<ISendEmail, SendEmailRepository>();
builder.Services.AddScoped<ISendMessage, SendMessage>();
builder.Services.AddScoped<ILogError, LogErrorRepository>();

builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IDailyStockService, DailyStockService>();
builder.Services.AddScoped<ILogErrorService, LogErrorService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMonthlyStockService, MonthlyStockService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddScoped<IProfilMenuService, ProfilMenuService>();
builder.Services.AddScoped<IProfilService, ProfilService>();
builder.Services.AddScoped<IProfilUserService, ProfilUserService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IRankingProductService, RankingProductService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IStockOpnameService, StockOpnameService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IUserMenuService, UserMenuService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IMasterJobList, MasterJobList>();
builder.Services.AddScoped<ITransactionJobList, TransactionJobList>();
builder.Services.AddCors();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Web API Retail",
        Description = "Retail System"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token"

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


var app = builder.Build();

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseSwagger();
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Retail API v1.0"));
}

app.UseCors(policy => policy.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
