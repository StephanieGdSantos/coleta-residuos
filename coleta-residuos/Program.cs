using AutoMapper;
using coleta_residuos.Data.Contexts;
using coleta_residuos.Data.Repository;
using coleta_residuos.Data.Repository.Impl;
using coleta_residuos.Models;
using coleta_residuos.Services;
using coleta_residuos.Services.Impl;
using coleta_residuos.Settings;
using coleta_residuos.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

var configuration = builder.Configuration;

#region Environment Validation

var jwtSecret = configuration["JwtSettings:Secret"];
if (string.IsNullOrWhiteSpace(jwtSecret) || jwtSecret.Length < 32)
{
    throw new InvalidOperationException("JwtSettings:Secret não configurado ou muito curto (mín. 32 caracteres).");
}

var oraclePassword = configuration["Oracle:Password"];
if (string.IsNullOrWhiteSpace(oraclePassword))
{
    throw new InvalidOperationException("Oracle:Password não configurado.");
}
#endregion

#region Banco de dados
var oracleUser = configuration["Oracle:User"] ?? "system";
var oracleHost = configuration["Oracle:Host"] ?? "localhost";
var oraclePort = configuration["Oracle:Port"] ?? "1521";
var oracleService = configuration["Oracle:Service"] ?? "xe";
var isDevelopment = builder.Environment.IsDevelopment();

var oracleDataSource = $"{oracleHost}:{oraclePort}/{oracleService}";
var connectionString = $"User Id={oracleUser};Password={oraclePassword};Data Source={oracleDataSource}";

builder.Services.AddDbContext<DatabaseContext>(
    opt => opt.UseOracle(connectionString).EnableSensitiveDataLogging(isDevelopment)
);

#endregion

#region Repositorios
builder.Services.AddScoped<IRepository<AlertaModel>, AlertaRepository>();
builder.Services.AddScoped<IRepository<ColetaAgendadaModel>, ColetaAgendadaRepository>();
builder.Services.AddScoped<IRepository<EventoColetaModel>, EventoColetaRepository>();
builder.Services.AddScoped<IRepository<PontoColetaModel>, PontoColetaRepository>();
builder.Services.AddScoped<IRepository<ResiduoModel>, ResiduoRepository>();
builder.Services.AddScoped<IPontoColetaResiduoRepository, PontoColetaResiduoRepository>();
#endregion

#region Services
builder.Services.AddScoped<IAlertaService, AlertaService>();
builder.Services.AddScoped<IColetaAgendadaService, ColetaAgendadaService>();
builder.Services.AddScoped<IEventoColetaService, EventoColetaService>();
builder.Services.AddScoped<IService<PontoColetaModel>, PontoColetaService>();
builder.Services.AddScoped<IService<ResiduoModel>, ResiduoService>();
builder.Services.AddScoped<IPontoColetaResiduoService, PontoColetaResiduoService>();
#endregion

#region AutoMapper

var mapperConfig = new MapperConfiguration(c => {
    c.AllowNullCollections = true;
    c.AllowNullDestinationValues = true;

    c.CreateMap<AlertaModel, AlertaViewModel>();
    c.CreateMap<ColetaAgendadaModel, ColetaAgendadaViewModel>();
    c.CreateMap<EventoColetaModel, EventoColetaViewModel>();
    c.CreateMap<ResiduoModel, ResiduoViewModel>();
    c.CreateMap<PontoColetaModel, PontoColetaViewModel>()
        .ForMember(dest => dest.Residuos, opt => opt.MapFrom(src =>
            src.PontosColetaResiduos.Select(pcr => pcr.Residuo).ToList()));

    c.CreateMap<AlertaViewModel, AlertaModel>();
    c.CreateMap<ColetaAgendadaViewModel, ColetaAgendadaModel>();
    c.CreateMap<EventoColetaViewModel, EventoColetaModel>();
    c.CreateMap<PontoColetaViewModel, PontoColetaModel>()
    .ForMember(dest => dest.PontosColetaResiduos, opt => opt.MapFrom(src =>
        src.Residuos.Select(residuo => new PontoColetaResiduoModel { ResiduoId = residuo.Id }).ToList()));
    c.CreateMap<ResiduoViewModel, ResiduoModel>();

    c.CreateMap<CriarAlertaViewModel, AlertaModel>();
    c.CreateMap<CriarColetaAgendadaViewModel, ColetaAgendadaModel>();
    c.CreateMap<CriarEventoColetaViewModel, EventoColetaModel>();
    c.CreateMap<CriarResiduoViewModel, ResiduoModel>();
    c.CreateMap<CriarPontoColetaViewModel, PontoColetaModel>()
        .ForMember(dest => dest.PontosColetaResiduos, opt => opt.MapFrom(src =>
            src.ResiduosIds.Select(id => new PontoColetaResiduoModel { ResiduoId = id }).ToList()));

    c.CreateMap<AtualizarAlertaViewModel, AlertaModel>();
    c.CreateMap<AtualizarColetaAgendadaViewModel, ColetaAgendadaModel>();
    c.CreateMap<AtualizarResiduoViewModel, ResiduoModel>();
    c.CreateMap<AtualizarEventoColetaViewModel, EventoColetaModel>();
    c.CreateMap<AtualizarPontoColetaViewModel, PontoColetaModel>()
        .ForMember(dest => dest.PontosColetaResiduos, opt => opt.MapFrom(src =>
        src.PontosColetaResiduos.Select(id => new PontoColetaResiduoModel { ResiduoId = id }).ToList()));
});

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

#endregion

#region Autenticacao
builder.Services.Configure<JwtSettings>(options =>
{
    options.Secret = jwtSecret;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

// Run migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}

app.Run();
