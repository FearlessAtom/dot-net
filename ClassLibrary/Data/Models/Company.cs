using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.Data.Models;

public class Company
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LogoPath { get; set; } = string.Empty;
}