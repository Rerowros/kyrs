using System.ComponentModel.DataAnnotations;

namespace kyrs.Models;

public class Currency
{
    [Key] 
    public int Id { get; set; }
    public string Code { get; set; }
    public decimal Rate { get; set; }
}