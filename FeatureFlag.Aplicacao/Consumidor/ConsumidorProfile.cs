using AutoMapper;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Aplicacao;

public class ConsumidorProfile : Profile
{
    public ConsumidorProfile()
    {
        CreateMap<AlterarConsumidorRequest, Consumidor>()
            .ForAllMembers(opt => 
                opt.Condition((_, _, srcMember) => srcMember != null));
    }
}