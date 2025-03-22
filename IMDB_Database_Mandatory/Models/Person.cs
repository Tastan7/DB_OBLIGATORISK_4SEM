using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_Database_Mandatory.Models
{
    public class Person
    {
        public string Nconst { get; set; }
        public string PrimaryName { get; set; }
        public string BirthYear { get; set; }
        public string DeathYear { get; set; }
        public string PrimaryProfession { get; set; }
        public string KnownForTitles { get; set; }
    }
}
