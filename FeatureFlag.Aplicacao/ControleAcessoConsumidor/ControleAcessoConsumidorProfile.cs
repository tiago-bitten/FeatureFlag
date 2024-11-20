using AutoMapper;
using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Aplicacao;

public class ControleAcessoConsumidorProfile : Profile
{
    public ControleAcessoConsumidorProfile()
    {
        CreateMap<CriarControleAcessoConsumidorRequest, ControleAcessoConsumidorResponse>();
    }
}