using IMDB_Database_Mandatory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_Database_Mandatory
{
    public class JunctionDataLoader
    {

        //HashSet<string> Genres = new HashSet<string>();



        // Denne klasse skal fylde junction tabelderne efter at de tabelder de referere til er blevet insertet i databasen.
        LoadResult result = new LoadResult();
        public LoadResult LoadTitleGenres(string filepath)
        {
            int lineCount = 0;
            // måske fjerne dette ved ikke helt, lets see how it goes
            // LoadResult result = new LoadResult();

            foreach (string line in File.ReadAllLines(filepath).Skip(1))
            {
                if (lineCount == 50000)
                {
                    break;
                }
                string[] splitLine = line.Split("\t");
                if (splitLine.Length != 9)
                {
                    throw new Exception("Ugyldig linje:" + line);
                }
                string tconst = splitLine[0];
                string genres = splitLine[8];

                if (!string.IsNullOrEmpty(genres))
                {
                    string[] genreArray = genres.Split(',');

                    foreach (string genre in genreArray)
                    {
                        if (LoadResult.GenreIdMap.TryGetValue(genre, out int genreId))
                        {
                            result.TitleGenres.Add(new TitleGenre
                            {
                                Tconst = tconst,
                                GenreId = genreId
                            });
                        }
                        else
                        {
                            //Console.WriteLine($"Advarsel: Genre '{genre}' Ikke fundet i GenreIdMAP.");
                        }
                    }


                }
                lineCount++;
            }

            return result;

        }

        #region PersonProfession Load
        public LoadResult LoadPP(string filepath)
        {
            int lineCount = 0;
            foreach (string line in File.ReadAllLines(filepath).Skip(1))
            {
                if (lineCount == 50000)
                {
                    break;
                }

                string[] splitLine = line.Split("\t");
                if (splitLine.Length != 6)
                {
                    throw new Exception("Invalid line:" + line);
                }
                string nconst = splitLine[0];
                string profession = splitLine[4];

                if (!string.IsNullOrEmpty(profession))
                {
                    string[] professionArray = profession.Split(",");
                    foreach (string prof in professionArray)
                    {
                        if (LoadResult.ProfessionDict.TryGetValue(profession, out int professionId))
                        {
                            result.PersonProfessions.Add(new PersonProfession
                            {
                                Nconst = nconst,
                                ProfessionId = professionId
                            });
                        }
                        else
                        {
                            //Console.WriteLine($"Advarsel: Profession '{prof}' ikke fundet i ProfessionDict.");
                        }
                    }
                }
                lineCount++;
            }
            return result;
            #endregion

        }
    }
}
