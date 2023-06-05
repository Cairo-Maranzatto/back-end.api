using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgendeMe.Data.Entity
{
    public class ClienteEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Nome { get; set; }

        [Required]
        [StringLength(80)]
        public string Email { get; set; }

        public DateTime? DataNascimento { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DataCadastro { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DataAlteracao { get; set; }

        public List<ClienteTelefoneEntity> ClienteTelefones { get; set; }
    }
}
