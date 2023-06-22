using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaNumberCreateDTO
	{
		[Required]
		public int VillaNo { get; set; }
		[Required]
		public int VillaID { get; set; }
		public string Specialdetails { get; set; }
	}
}
