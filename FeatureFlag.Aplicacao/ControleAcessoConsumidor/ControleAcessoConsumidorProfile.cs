using AutoMapper;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Aplicacao;

public class ControleAcessoConsumidorProfile : Profile
{
    public ControleAcessoConsumidorProfile()
    {
        CreateMap<ControleAcessoConsumidor, ControleAcessoConsumidorResponse>()
            .ForCtorParam("IdentificadorConsumidor", opt => opt.MapFrom(src => src.Consumidor.Identificador))
            .ForCtorParam("IdentificadoresRecurso", opt => opt.MapFrom(src => src.Recurso.Identificador))
            .ForCtorParam("Tipo", opt => opt.MapFrom(src => src.Tipo));
    }
}