using AutoMapper;
using FeatureFlag.Domain;
using FeatureFlag.Domain.Dtos;

namespace FeatureFlag.Aplicacao;

public class RecursoProfile : Profile
{
    public RecursoProfile()
    {
        CreateMap<CriarRecursoRequest, Recurso>()
            .ConstructUsing((src, dest) => Recurso.Criar(src.Identificador, src.Descricao, src.Porcentagem));

        CreateMap<Recurso, RecursoResponse>();
        
        CreateMap<AlterarRecursoRequest, Recurso>()
            .ForAllMembers(opt => 
                opt.Condition((_, _, srcMember) => srcMember != null));
    }
}