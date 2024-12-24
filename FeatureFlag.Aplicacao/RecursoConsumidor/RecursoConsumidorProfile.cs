using AutoMapper;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Aplicacao;

public class RecursoConsumidorProfile : Profile
{
    public RecursoConsumidorProfile()
    {
        CreateMap<RecursoConsumidor, RecursoConsumidorResponse>()
            .ForCtorParam("Recurso", opt => opt.MapFrom(src => src.Recurso.Identificador))
            .ForCtorParam("Consumidor", opt => opt.MapFrom(src => src.Consumidor.Identificador))
            .ForCtorParam("Habilitado", opt => opt.MapFrom(src => src.Status == EnumStatusRecursoConsumidor.Habilitado));

    }
}   