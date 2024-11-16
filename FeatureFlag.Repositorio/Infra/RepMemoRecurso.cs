using FeatureFlag.Domain;

namespace FeatureFlag.Repositorio.Infra;

public class RepMemoRecurso : RepMemoBase<Recurso>, IRepRecurso
{
    public Task<decimal> RecuperarPorcentagemPorIdentificadorAsync(string identificador)
    {
        return Task.FromResult(Items
            .Where(x => x.Identificador == identificador)
            .Select(x => x.Porcentagem)
            .FirstOrDefault());
    }
}