using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMTest.WPF.Models.Chloe
{
    [Table(Name = "InvGroups")]
    public class InvGroup
    {
        [Column(Name = "GroupID", IsPrimaryKey = true)]
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public string GroupName { get; set; }
        public int? IconID { get; set; }
        public bool UseBasePrice { get; set; }
        public bool Anchored { get; set; }
        public bool Anchorable { get; set; }
        public bool FittableNonSingleton { get; set; }
        public bool Published { get; set; }
    }
}
