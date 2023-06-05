namespace AgendeMe.Data.Entity
{
    public enum TipoTelefone
    {
        Casa,
        Trabalho,
        Celular
    }

    public class ClienteTelefoneEntity
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public TipoTelefone Descricao { get; set; }
        public string Numero { get; set; }
    }
}
