using FeatureFlag.Domain;
using FeatureFlag.Repositorio.Infra;
using MongoDB.Driver;

namespace FeatureFlag.Repositorio;

public class RepRecurso : RepBase<Recurso>, IRepRecurso
{
    #region Ctor
    public RepRecurso(MongoDbContext context) 
        : base(context.Recursos)
    {
    }
    #endregion

    #region RecuperarPorcentagemPorIdentificador
    public Task<decimal> RecuperarPorcentagemPorIdentificadorAsync(string identificador)
    {
        return Collection
            .Find(x => x.Identificador == identificador)
            .Project(x => x.Porcentagem.Alvo)
            .FirstOrDefaultAsync();
    }
    #endregion

    #region RecuperarPorIdentificador
    public Task<Recurso> RecuperarPorIdentificadorAsync(string identificador)
    {
        return Collection
            .Find(x => x.Identificador == identificador)
            .FirstOrDefaultAsync();
    }
    #endregion
    
    #region ExistePorIdentificador
    public Task<bool> ExistePorIdentificadorAsync(string identificador)
    {
        return Collection
            .Find(x => x.Identificador == identificador)
            .AnyAsync();
    }
    #endregion
}