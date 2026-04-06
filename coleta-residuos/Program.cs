using AutoMapper;
using coleta_residuos.Data.Contexts;
using coleta_residuos.Data.Repository;
using coleta_residuos.Data.Repository.Impl;
using coleta_residuos.Models;
using coleta_residuos.Services;
using coleta_residuos.Services.Impl;
using coleta_residuos.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


#region Banco de dados
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<DatabaseContext>(
    opt => opt.UseOracle(connectionString).EnableSensitiveDataLogging(true)
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("f+ujXAKHk00L5jlMXo2XhAWawsOoihNP1OiAM25lLSO57+X7uBMQgwPju6yzyePi")),
        ValidateIssuer = false,
        ValidateAudience = false
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

app.Run();
