using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio.Infra
{
    public static class DocumentsRegistration
    {
        public static void Registrar()
        {
            // Instanciação explícita dos configuradores
            RegistrarConfigurador(new ConsumidorConfig());
            RegistrarConfigurador(new RecursoConfig());
            RegistrarConfigurador(new RecursoConsumidorConfig());
            RegistrarConfigurador(new ControleAcessoConsumidorConfig());
        }

        private static void RegistrarConfigurador<T>(IDocumentConfiguration<T> configurador) where T : class
        {
            // Verifica se o mapa já foi registrado para evitar duplicação
            if (BsonClassMap.IsClassMapRegistered(typeof(T))) return;

            // Cria o mapa e aplica a configuração
            var bsonClassMap = new BsonClassMap<T>(configurador.Configurar);

            // Registra o mapa
            BsonClassMap.RegisterClassMap(bsonClassMap);
        }
    }
}