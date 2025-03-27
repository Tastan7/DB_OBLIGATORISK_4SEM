using IMDB_Database_Mandatory.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_Database_Mandatory
{
    public interface IInserter
    {

        void Insert(List<Title> titles, List<Person> persons, List<Genre> genres, HashSet<string> professions, SqlConnection sqlConn, SqlTransaction transAction);
    }
}
