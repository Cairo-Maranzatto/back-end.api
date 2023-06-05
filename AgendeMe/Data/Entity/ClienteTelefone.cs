using System;
using System.ComponentModel.DataAnnotations;

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
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ClienteId { get; set; }

        [Required]
        public ClienteEntity Cliente { get; set; }

        [Required]
        [EnumDataType(typeof(TipoTelefone))]
        public TipoTelefone Descricao { get; set; }

        [Required]
        [StringLength(20)]
        public string Numero { get; set; }
    }
}
