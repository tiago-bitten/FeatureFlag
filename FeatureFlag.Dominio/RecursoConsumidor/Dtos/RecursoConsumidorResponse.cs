using FeatureFlag.Domain;

namespace FeatureFlag.Dominio.RecursoConsumidor.Dtos;

public record RecursoConsumidorResponse(
    string Recurso,
    string Consumidor,
    bool Habilitado)
{
    public static RecursoConsumidorResponse ConsumidorSemRecurso(Consumidor consumidor, string recurso)
    {
        return new RecursoConsumidorResponse(
            Recurso: recurso,
            Consumidor: consumidor.Identificador,
            Habilitado: false);
    }
}