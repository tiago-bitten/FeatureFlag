using AutoMapper;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Aplicacao;

public class RecursoConsumidorProfile : Profile
{
    public RecursoConsumidorProfile()
    {
        CreateProjection<RecursoConsumidor, RecursoConsumidorResponse>()
            .ForMember(dest => dest.Habilitado, opt => 
                opt.MapFrom(src => src.Status == EnumStatusRecursoConsumidor.Habilitado));
        
        CreateMap<RecursoConsumidor, RecursoConsumidorResponse>()
            .ForMember(dest => dest.Habilitado, opt => 
                opt.MapFrom(src => src.Status == EnumStatusRecursoConsumidor.Habilitado));
    }
}