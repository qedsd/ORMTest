using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMTest.WPF.Models.EF
{
    public class InvType
    {
        [Column("TypeID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TypeID { get; set; }
        public int? GroupID { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public double? Mass { get; set; }
        public double? Volume { get; set; }
        public double? Capacity { get; set; }
        public int? PortionSize { get; set; }
        public int? RaceID { get; set; }
        public double? BasePrice { get; set; }
        public int? Published { get; set; }
        public int? MarketGroupID { get; set; }
        public int? IconID { get; set; }
        public int? SoundID { get; set; }
        public int? GraphicID { get; set; }
    }
}
