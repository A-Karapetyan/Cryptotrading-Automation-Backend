using System;
using System.ComponentModel.DataAnnotations;

public interface IBaseEntity
{
    [Key]
    int Id { get; set; }
    bool IsDeleted { get; set; }
    DateTime CreateDt { get; set; }
    DateTime UpdateDt { get; set; }
}