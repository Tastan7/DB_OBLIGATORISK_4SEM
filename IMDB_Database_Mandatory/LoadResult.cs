using IMDB_Database_Mandatory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Klassen bruges til at holde styr på alt data, der bliver indlæst fra filerne
namespace IMDB_Database_Mandatory
{
    public class LoadResult
    {
        // Liste med alle film, der bliver indlæst
        public List<Title> Titles { get; set; } = new List<Title>();

        // Genre-relaterede data (Genres og hvilke film de tilhører)
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public List<TitleGenre> TitleGenres { get; set; } = new List<TitleGenre>();

        // HashSet for at undgå dubletter af professioner
        public HashSet<string> Professions { get; set; } = new HashSet<string>();

        // Liste over personer i databasen
        public List<Person> Persons { get; set; } = new List<Person>();
        public List<PersonProfession> PersonProfessions { get; set; } = new List<PersonProfession>();
        public List<TitleWriter> TitleWriters { get; set; } = new List<TitleWriter>();
        public List<TitleDirector> TitleDirector { get; set; } = new List<TitleDirector>();

        // Liste over kendte titler, som personer er forbundet til
        public List<KnownForTitles> KnownForTitles { get; set; } = new List<KnownForTitles>();

        // HashSets bruges til at tjekke, om vi allerede har set et ID før (undgår FK-conflicts)
        public static HashSet<string> TconstHS { get; set; } = new HashSet<string>(); // Tconst = Unikke film-ID'er
        public static HashSet<string> NconstHS { get; set; } = new HashSet<string>(); // Nconst = Unikke person-ID'er

        // Dictionary til hurtigere opslag af kendte titler
        public static Dictionary<string, string> KnownForTitlesDict { get; set; } = new Dictionary<string, string>();

        // Dictionary til at matche professioner med deres ID
        public static Dictionary<string, int> ProfessionDict { get; set; } = new Dictionary<string, int>();

        // Dictionary til at matche genre med deres ID
        public static Dictionary<string, int> GenreIdMap { get; set; } = new Dictionary<string, int>();

        // ikke helt sikker på at der fungerer som det skal, mangler at teste
    }
}
