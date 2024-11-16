using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Repositorio.Infra;

public class RepMemoBase<T> : IRepBase<T> where T : EntidadeBase
{
    protected readonly List<T> Items = [];
        
    public Task AdicionarAsync(T entidade)
    {
        if (entidade == null) throw new ArgumentNullException(nameof(entidade));
        Items.Add(entidade);

        return Task.CompletedTask;
    }

    public Task<T?> RecuperarPorIdAsync(Guid id, params string[]? includes)
    {
        var entidade = Items.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(entidade);
    }

    public IQueryable<T> RecuperarTodos(params string[]? includes)
    {
        return Items.AsQueryable();
    }

    public void Atualizar(T entidade)
    {
        if (entidade == null) throw new ArgumentNullException(nameof(entidade));
        var existingEntity = Items.FirstOrDefault(x => x.Id == entidade.Id);

        if (existingEntity != null)
        {
            Items[Items.IndexOf(existingEntity)] = entidade;
        }
        else
        {
            throw new ArgumentException("Entidade não encontrada para atualização.", nameof(entidade));
        }
    }

    public void Remover(Guid id)
    {
        var entidade = Items.FirstOrDefault(x => x.Id == id);
        if (entidade != null)
        {
            Items.Remove(entidade);
        }
        else
        {
            throw new ArgumentException("Entidade não encontrada para remoção.", nameof(id));
        }
    }

    public Task<int> CountAsync()
    {
        return Task.FromResult(Items.Count);
    }
}