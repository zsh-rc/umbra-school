using System;
using System.ComponentModel.DataAnnotations;

namespace Umbra.School.Data;

    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
