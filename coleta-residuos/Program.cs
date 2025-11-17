using coleta_residuos.Data.Contexts;
using coleta_residuos.Data.Repository;
using coleta_residuos.Data.Repository.Impl;
using coleta_residuos.Models;
using coleta_residuos.Services;
using coleta_residuos.Services.Impl;
using coleta_residuos.ViewModel;
using Microsoft.EntityFrameworkCore;

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

var mapperConfig = new AutoMapper.MapperConfiguration(c => {
    c.AllowNullCollections = true;
    c.AllowNullDestinationValues = true;

    c.CreateMap<AlertaModel, AlertaViewModel>();
    c.CreateMap<ColetaAgendadaModel, ColetaAgendadaViewModel>();
    c.CreateMap<EventoColetaModel, EventoColetaViewModel>();
    c.CreateMap<PontoColetaModel, PontoColetaViewModel>();
    c.CreateMap<ResiduoModel, ResiduoViewModel>();

    c.CreateMap<AlertaViewModel, AlertaModel>();
    c.CreateMap<ColetaAgendadaViewModel, ColetaAgendadaModel>();
    c.CreateMap<EventoColetaViewModel, EventoColetaModel>();
    c.CreateMap<PontoColetaViewModel, PontoColetaModel>();
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
    c.CreateMap<AtualizarPontoColetaViewModel, PontoColetaModel>();
    c.CreateMap<AtualizarResiduoViewModel, EventoColetaModel>();
});

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
