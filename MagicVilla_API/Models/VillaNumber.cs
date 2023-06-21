using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Models
{
	public class VillaNumber
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        [ForeignKey("Villa")]
        public int VillaID { get; set; }
        public Villa Villa { get; set; } //navigation property
        public string Specialdetails { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
