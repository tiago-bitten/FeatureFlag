using AutoMapper;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using Microsoft.EntityFrameworkCore.Query;

namespace FeatureFlag.Aplicacao;

public class RecursoConsumidorProfile : Profile
{
    public RecursoConsumidorProfile()
    {
        CreateMap<RecursoConsumidor, RecursoConsumidorResponse>()
            .ForCtorParam("Recurso", opt => opt.MapFrom(src => src.Recurso.Identificador))
            .ForCtorParam("Consumidor", opt => opt.MapFrom(src => src.Consumidor.Identificador))
            .ForCtorParam("Habilitado", opt => opt.MapFrom(src => src.Status == EnumStatusRecursoConsumidor.Habilitado));

        CreateMap<Consumidor.RecursoConsumidorEmbedded, RecursoConsumidorResponse>()
            .ForCtorParam("Recurso", opt => opt.MapFrom(src => src.Recurso))
            .ForCtorParam("Consumidor", opt => opt.MapFrom((_, context) => context.Items["Consumidor"]!.ToString()))
            .ForCtorParam("Habilitado", opt => opt.MapFrom(src => src.Status == EnumStatusRecursoConsumidor.Habilitado));
    }
}   