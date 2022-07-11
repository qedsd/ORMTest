using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMTest.WPF.Models.FreeSql
{
    [Table(Name = "InvGroup")]
    public class InvGroup
    {
        [Column(Name = "GroupID", IsPrimary = true)]
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public string GroupName { get; set; }
        [Column(IsNullable = true)]
        public int IconID { get; set; }
        public bool UseBasePrice { get; set; }
        public bool Anchored { get; set; }
        public bool Anchorable { get; set; }
        public bool FittableNonSingleton { get; set; }
        public bool Published { get; set; }
    }
}
