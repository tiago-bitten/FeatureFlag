namespace FeatureFlag.Dominio.Dtos;

public record RecursoConsumidorResponse(string Consumidor,
                                        string Recurso,
                                        bool Habilitado)
{
    public static RecursoConsumidorResponse Ativo(string identificadorConsumidor, string identificadorRecurso)
    {
        return new RecursoConsumidorResponse(
            Consumidor: identificadorConsumidor,
            Recurso: identificadorRecurso,
            Habilitado: true);
    }
    
    public static RecursoConsumidorResponse Desabilitado(string identificadorConsumidor, string identificadorRecurso)
    {
        return new RecursoConsumidorResponse(
            Consumidor: identificadorConsumidor,
            Recurso: identificadorRecurso,
            Habilitado: false);
    }
}