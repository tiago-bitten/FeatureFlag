namespace FeatureFlag.Dominio.Dtos;

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
    
    public static RecursoConsumidorResponse Ativo(string identificadorRecurso, string identificadorConsumidor)
    {
        return new RecursoConsumidorResponse(
            Recurso: identificadorRecurso,
            Consumidor: identificadorConsumidor,
            Habilitado: true);
    }
    
    public static RecursoConsumidorResponse Desabilitado(string identificadorRecurso, string identificadorConsumidor)
    {
        return new RecursoConsumidorResponse(
            Recurso: identificadorRecurso,
            Consumidor: identificadorConsumidor,
            Habilitado: false);
    }
}