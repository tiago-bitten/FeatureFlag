using FeatureFlag.Dominio;
using FeatureFlag.Repositorio.Infra;

namespace FeatureFlag.Repositorio;

public class RepMemoControleAcessoConsumidor : RepMemoBase<ControleAcessoConsumidor>, IRepControleAcessoConsumidor
{
    public IQueryable<ControleAcessoConsumidor> RecuperarPorTipo(string identificadorRecurso, EnumTipoControle tipo)
    {
        return Items
            .Where(x => 
                x.Recurso.Identificador == identificadorRecurso &&
                x.Tipo == tipo)
            .AsQueryable();
    }
}