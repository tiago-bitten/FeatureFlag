namespace FeatureFlag.Dominio.RecursoConsumidor.Dtos;

// Todo: solicito apenas a desc e id do recurso, porém, caso o consumidor não exista. como vou saber o id do recurso?
public record RecuperarPorRecursoConsumidorParam(
    string DescricaoRecurso,
    string IdentificadorRecurso);
