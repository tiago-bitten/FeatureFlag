﻿using FeatureFlag.Dominio;
using FeatureFlag.Repositorio.Infra;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FeatureFlag.Repositorio;

public class RepControleAcessoConsumidor : RepBase<ControleAcessoConsumidor>, IRepControleAcessoConsumidor
{
    #region Ctor
    public RepControleAcessoConsumidor(MongoDbContext context) 
        : base(context.ControleAcessoConsumidores)
    {
    }
    #endregion

    #region PossuiPorTipoAsync
    public Task<bool> PossuiPorTipoAsync(string identificadorRecurso, string identificadorConsumidor, EnumTipoControle tipoControle)
    {
        return Collection
            .Find(x => x.Recurso.Identificador == identificadorRecurso &&
                       x.Consumidor.Identificador == identificadorConsumidor && 
                       x.Tipo == tipoControle)
            .AnyAsync();
    }
    #endregion
    
    #region RecuperarPorRecursoAsync
    public Task<List<ControleAcessoConsumidor>> RecuperarPorRecursoAsync(ObjectId codigoRecurso)
    {
        return Collection
            .Find(x => x.Recurso.Id == codigoRecurso)
            .ToListAsync();
    }
    #endregion
    
    #region RecuperarPorConsumidorAsync
    public Task<List<ControleAcessoConsumidor>> RecuperarPorConsumidorAsync(ObjectId codigoConsumidor)
    {
        return Collection
            .Find(x => x.Consumidor.Id == codigoConsumidor)
            .ToListAsync();
    }
    #endregion
    
    #region RecuperarPorRecursoConsumidorAsync
    public Task<ControleAcessoConsumidor> RecuperarPorRecursoConsumidorAsync(ObjectId codigoRecurso, ObjectId codigoConsumidor)
    {
        return Collection
            .Find(x => x.Recurso.Id == codigoRecurso && x.Consumidor.Id == codigoConsumidor)
            .FirstOrDefaultAsync();
    }
    #endregion
}