using AutoMapper;
using FeatureFlag.Domain;
using FeatureFlag.Domain.Dtos;

namespace FeatureFlag.Aplicacao;

public class RecursoProfile : Profile
{
    public RecursoProfile()
    {
        CreateMap<AdicionarRecursoRequest, Recurso>()
            .ForMember(dest => dest.Porcentagem, opt => opt.Ignore())
            .ConstructUsing((src) => new Recurso(src.Identificador, src.Descricao, src.Porcentagem));

        CreateMap<Recurso, RecursoResponse>()
            .ForCtorParam("Identificador", opt => opt.MapFrom(src => src.Identificador))
            .ForCtorParam("Descricao", opt => opt.MapFrom(src => src.Descricao))
            .ForCtorParam("Porcentagem", opt => opt.MapFrom(src => src.Porcentagem));
        
        CreateMap<AlterarRecursoRequest, Recurso>()
            .ForAllMembers(opt => 
                opt.Condition((_, _, srcMember) => srcMember != null));
    }
}