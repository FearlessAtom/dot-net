using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLassLibrary.Data.Models;

public class ATM
{
    [NotMapped] public static ATM? CurrentATM {get; set;}

    [Key] public int Id { get; set; }
    public decimal Balance { get; set; }
    public string Address { get; set; } = string.Empty;
    public int CompanyId { get; set; }
}