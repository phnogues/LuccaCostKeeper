using System.ComponentModel.DataAnnotations;

namespace LuccaCostKeeper.Model.Entities;

public class BaseEntity
{
	[Key]
	public Guid Id { get; set; } = Guid.NewGuid();
}
