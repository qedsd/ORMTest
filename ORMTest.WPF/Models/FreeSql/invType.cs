using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMTest.WPF.Models.FreeSql
{
    [Table(Name = "InvType")]
    public class InvType 
    {
        [Column(Name = "TypeID", IsPrimary = true)]
        public int TypeID { get; set; }
        public int? GroupID { get; set; }
        public string TypeName { get; set; }
        [Column(StringLength = -1)]
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
