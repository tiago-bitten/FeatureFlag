using FeatureFlag.Domain;
using FeatureFlag.Repositorio.Infra;

namespace FeatureFlag.Repositorio;

public class RepMemoRecurso : RepMemoBase<Domain.Recurso>, IRepRecurso
{
    public Task<decimal> RecuperarPorcentagemPorIdentificadorAsync(string identificador)
    {
        return Task.FromResult(Items
            .Where(x => x.Identificador == identificador)
            .Select(x => x.Porcentagem)
            .FirstOrDefault());
    }

    public Task<Recurso?> RecuperarPorIdentificadorAsync(string identificador)
    {
        return Task.FromResult(Items
            .FirstOrDefault(x => x.Identificador == identificador));
    }
}