using APIClinic.CacheList;
using APIClinic.CacheList.ProcessDataList;
using APIClinic.CacheList.ProcessDataList.Interface;
using APIClinic.Crypto;
using APIClinic.Middleware;
using APIClinic.Models.Database;
using APIClinic.Repository;
using APIClinic.Repository.IRepository;
using APIClinic.Service;
using APIClinic.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
DecryptMD5 decryptMD5 = new DecryptMD5();
var connectionString = decryptMD5.MD5Decrypt(builder.Configuration.GetConnectionString("clinicContext").ToString());

builder.Services.AddDbContext<clinic_systemContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IQueueNo, QueueRepository>();
builder.Services.AddScoped<ICrypto, CryptoRepository>();
builder.Services.AddScoped<IMenu, MenuRepository>();
builder.Services.AddScoped<IProfil, ProfilRepository>();
builder.Services.AddScoped<IProfilMenu, ProfilMenuRepository>();
builder.Services.AddScoped<IProfilUser, ProfilUserRepository>();
builder.Services.AddScoped<IUserMenu, UserMenuRepository>();
builder.Services.AddScoped<IUsers, UserRepository>();
builder.Services.AddScoped<IAbsenDoctor, AbsenDoctorRepository>();
builder.Services.AddScoped<IBranch, BranchRepository>();
builder.Services.AddScoped<IClinic, ClinicRepository>();
builder.Services.AddScoped<IDoctor, DoctorRepository>();
builder.Services.AddScoped<ISpecialist, SpecialistRepository>();
builder.Services.AddScoped<IDrug, DrugRepository>();
builder.Services.AddScoped<ILaboratorium, LaboratoriumRepository>();
builder.Services.AddScoped<IExaminationDoctor, ExaminationDoctorRepository>();
builder.Services.AddScoped<IExaminationLab, ExaminationLabRepository>();
builder.Services.AddScoped<IPatientRegistration, PatientRegistrationRepository>();
builder.Services.AddScoped<IPatientRegistrationLab, PatientRegistrationLabRepository>();
builder.Services.AddScoped<ISchedule, ScheduleRepository>();
builder.Services.AddScoped<ISpecialDoctor, SpecialDoctorRepository>();
builder.Services.AddScoped<ITransactionHeaderPatient, TransactionHeaderPatientRegistrationRepository>();
builder.Services.AddScoped<ITransactionDetailPatient, TransactionDetailPatientRegistrationRepository>();
builder.Services.AddScoped<ITransactionLab, TransactionHeaderPatientLabRepository>();
builder.Services.AddScoped<ILogError, LogErrorRepository>();

builder.Services.AddScoped<IProfilService, ProfilService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IProfilMenuService, ProfilMenuService>();
builder.Services.AddScoped<IProfilUserService, ProfilUserService>();
builder.Services.AddScoped<IUserMenuService, UserMenuService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAbsenDoctorService, AbsenDoctorService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<ISpecialService, SpecialService>();
builder.Services.AddScoped<IDrugService, DrugService>();
builder.Services.AddScoped<ILaboratoriumService, LaboratoriumService>();
builder.Services.AddScoped<IExaminationDoctorService, ExaminationDoctorService>();
builder.Services.AddScoped<IExaminationLabService, ExaminationLabService>();
builder.Services.AddScoped<IPatientRegistrationService, PatientRegistrationService>();
builder.Services.AddScoped<IPatientRegistrationLabService, PatientRegistrationLabService>();
builder.Services.AddScoped<IScheduleDoctorService, ScheduleDoctorService>();
builder.Services.AddScoped<ISpecialDoctorService, SpecialDoctorService>();
builder.Services.AddScoped<ITransactionHeaderPatientService, TransactionHeaderPatientRegistrationService>();
//builder.Services.AddScoped<ITransactionDetailPatientRegistrationService, TransactionDetailPatientRegistrationService>();
builder.Services.AddScoped<ITransactionHeaderPatientLabService, TransactionHeaderPatientLabService>();

builder.Services.AddScoped<IMasterDataList, MasterDataList>();
builder.Services.AddScoped<ITransactionDataList, TransactionDataList>();
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
        Title = "Web API Clinic",
        Description = "Clinic System"
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


// Configure the HTTP request pipeline.
app.UseSwagger();
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clinic API v1.0"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
