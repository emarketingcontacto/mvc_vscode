
using System.ComponentModel.DataAnnotations;

namespace mvcEnterprise.Models{

public class Employee{
	[Key]
	public int empId { get; set; }
	public string? empName { get; set; }
	public string? empLName { get; set; }
	[Required]
	public int catId { get; set; }
	//public string? catName { get; set;}
	//public virtual Categories? Categories { get; set; } No work 
	// public virtual IList<Category>? Categories { get; set;}
	// public ICollection<Category>? Categories { get; set; }

	}
}
