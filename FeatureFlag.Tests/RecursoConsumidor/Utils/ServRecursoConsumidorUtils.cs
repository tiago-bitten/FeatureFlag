using FeatureFlag.Domain;
using FeatureFlag.Dominio;

namespace FeatureFlag.Tests.Utils;

public static class RecursoConsumidorTestUtils
{
    /// <summary>
    /// Cria um recurso com valores padrão ou personalizados.
    /// </summary>
    public static Recurso CriarRecurso(
        string identificador = "RecursoDefault",
        string descricao = "Descrição do recurso",
        decimal porcentagem = 50)
    {
        return Recurso.Criar(identificador, descricao, porcentagem);
    }

    /// <summary>
    /// Cria um recurso consumidor com valores padrão ou personalizados.
    /// </summary>
    public static RecursoConsumidor CriarRecursoConsumidor(
        Recurso recurso,
        Guid? codigoRecurso = null,
        Guid? codigoConsumidor = null,
        EnumStatusRecursoConsumidor? status = null)
    {
        var recursoConsumidor = RecursoConsumidor.Criar(
            codigoRecurso ?? Guid.NewGuid(),
            codigoConsumidor ?? Guid.NewGuid());

        recursoConsumidor.Recurso = recurso;

        if (status.HasValue)
        {
            switch (status.Value)
            {
                case EnumStatusRecursoConsumidor.Habilitado:
                    recursoConsumidor.Habilitar();
                    break;
                case EnumStatusRecursoConsumidor.Desabilitado:
                    recursoConsumidor.Desabilitar();
                    break;
            }
        }

        return recursoConsumidor;
    }

    /// <summary>
    /// Cria uma lista de recursos consumidores habilitados.
    /// </summary>
    public static List<RecursoConsumidor> CriarListaRecursoConsumidores(
        int quantidade,
        EnumStatusRecursoConsumidor status,
        Recurso recurso)
    {
        var lista = new List<RecursoConsumidor>();

        for (int i = 0; i < quantidade; i++)
        {
            lista.Add(CriarRecursoConsumidor(
                codigoRecurso: recurso?.Id,
                status: status,
                recurso: recurso));
        }

        return lista;
    }
}
