using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMTest.WPF.Models.Chloe
{
    [Table(Name = "InvTypes")]
    public class InvType
    {
        [Column(Name = "TypeID", IsPrimaryKey = true)]
        [NonAutoIncrement]
        public int TypeID { get; set; }
        public int? GroupID { get; set; }
        [NotNull]
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
