using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Data.Models;

public class Bank
{
    [NotMapped] public static Bank? CurrentBank { get; set;}

    [Key] public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public int CompanyId { get; set; }
}