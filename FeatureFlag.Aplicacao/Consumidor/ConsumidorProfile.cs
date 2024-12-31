using AutoMapper;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Aplicacao;

public class ConsumidorProfile : Profile
{
    public ConsumidorProfile()
    {
        CreateMap<AdicionarConsumidorRequest, Consumidor>();

        CreateMap<Consumidor, ConsumidorResponse>();
        
        CreateMap<AlterarConsumidorRequest, Consumidor>()
            .ForAllMembers(opt => 
                opt.Condition((_, _, srcMember) => srcMember != null));
    }
}