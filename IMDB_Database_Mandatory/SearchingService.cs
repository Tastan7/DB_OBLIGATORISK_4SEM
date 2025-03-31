using IMDB_Database_Mandatory.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_Database_Mandatory
{
    public class JunctionPreparedInserter
    {
        private HashSet<string> existingNConstCache = new HashSet<string>();

        public void Insert(List<TitleGenre> titleGenres, List<PersonProfession> pp, List<KnownForTitles> knownForTitles, List<TitleDirector> titleDirector, List<TitleWriter> titleWriter, SqlConnection sqlConn, SqlTransaction transAction)
        {



            string TitleGenreSQL = "INSERT INTO [TitleGenres] ([Tconst], [GenreId]) VALUES (@Tconst, @GenreId)";
            SqlCommand titleGenreSqlComm = new SqlCommand(TitleGenreSQL, sqlConn, transAction);

            // Opret parametre for tconst og genre
            SqlParameter tconstTGPar = new SqlParameter("@Tconst", SqlDbType.VarChar, 50);
            SqlParameter genreIDPar = new SqlParameter("@GenreId", SqlDbType.Int);
            titleGenreSqlComm.Parameters.Add(tconstTGPar);
            titleGenreSqlComm.Parameters.Add(genreIDPar);
            titleGenreSqlComm.Prepare();

            Console.WriteLine("TitleGenre forberedt..");

            foreach (var titleGenre in titleGenres)
            {
                tconstTGPar.Value = titleGenre.Tconst;
                genreIDPar.Value = titleGenre.GenreId;

                titleGenreSqlComm.ExecuteNonQuery();


            }
            Console.WriteLine("TitleGenre udført..");

            #region PersonProfession indsættelse

            string ppSQL = "INSERT INTO [PersonsProfessions]([ProfessionId], [Nconst]) VALUES (@ProfessionId, @Nconst)";
            SqlCommand ppSQLComm = new SqlCommand(ppSQL, sqlConn, transAction);

            // Parametre for pp
            SqlParameter professionidPar = new SqlParameter("@ProfessionId", SqlDbType.Int);
            SqlParameter nconstPar = new SqlParameter("@Nconst", SqlDbType.NVarChar, 50);
            ppSQLComm.Parameters.Add(professionidPar);
            ppSQLComm.Parameters.Add(nconstPar);
            ppSQLComm.Prepare();

            Console.WriteLine("PersonsProfession forberedt..");

            foreach (var personProfession in pp)
            {
                professionidPar.Value = personProfession.ProfessionId;
                nconstPar.Value = personProfession.Nconst;

                ppSQLComm.ExecuteNonQuery();
            }

            Console.WriteLine("PersonProfession udført..");
            #endregion

            #region KnownForTitle indsættelse
            string KFTSQL = "INSERT INTO [KnownForTitles] ([Tconst], [Nconst]) VALUES (@Tconst, @Nconst)";
            SqlCommand KFTSQLComm = new SqlCommand(KFTSQL, sqlConn, transAction);

            SqlParameter tconstKFTPar = new SqlParameter("@Tconst", SqlDbType.NVarChar, 50);
            SqlParameter nconstKFTPar = new SqlParameter("@Nconst", SqlDbType.NVarChar, 50);

            // Tilføj parametre til kommandoen
            KFTSQLComm.Parameters.Add(tconstKFTPar);
            KFTSQLComm.Parameters.Add(nconstKFTPar);

            KFTSQLComm.Prepare();
            Console.WriteLine("KnownForTitle sql kommando forberedt..");

            foreach (KnownForTitles kft in knownForTitles)
            {
                if (LoadResult.TconstHS.Contains(kft.Tconst))
                {
                    try
                    {
                        tconstKFTPar.Value = kft.Tconst;
                        nconstKFTPar.Value = kft.Nconst;


                        KFTSQLComm.ExecuteNonQuery();
                    }
                    catch (SqlException ex) when (ex.Number == 2627)
                    {
                        //Console.WriteLine($"Duplikat post for Tconst {kft.tconst}, nconst {kft.nconst} - springer indsættelse over");
                    }
                }
                else
                {
                    //Console.WriteLine($"Springer KnownForTitle over for Tconst {kft.tconst} - Ikke inkluderet i vores indlæste data");

                }
            }
            Console.WriteLine("KnownForTitle sql kommando udført...");
            #endregion
            #region Writer indsættelse
            if (titleWriter == null)
            {
                Console.WriteLine("Fejl: titleWriter listen er null.");
                return;
            }

            if (sqlConn == null || transAction == null)
            {
                Console.WriteLine("Fejl: SQL forbindelse eller transaktion er null.");
                return;
            }

            string writerSQL = "INSERT INTO [TitleWriters]([Tconst], [Nconst]) VALUES (@Tconst, @Nconst)";
            SqlCommand writerSQLComm = new SqlCommand(writerSQL, sqlConn, transAction);
            SqlParameter titPar = new SqlParameter("@Tconst", SqlDbType.NVarChar, 50);
            SqlParameter perPar = new SqlParameter("@Nconst", SqlDbType.NVarChar, 50);
            writerSQLComm.Parameters.Add(titPar);
            writerSQLComm.Parameters.Add(perPar);

            try
            {
                writerSQLComm.Prepare();
                Console.WriteLine("Writers SQL kommando forberedt...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kunne ikke forberede SQL kommando: " + ex.Message);
                return;
            }

            // Et HashSet til at holde styr på de tconst, nconst par der er blevet inserted.
            var insertedTitleWriters = new HashSet<(string TConst, string NConst)>();

            foreach (TitleWriter writer in titleWriter)
            {
                if (writer == null || string.IsNullOrEmpty(writer.Nconst) || string.IsNullOrEmpty(writer.Tconst))
                {
                    //Console.WriteLine("Fejl: Stødte på null TitleDirector objekt eller manglende nconst.");
                    continue;
                }

                var writerKey = (writer.Tconst, writer.Nconst);
                if (insertedTitleWriters.Contains(writerKey))
                {
                    //Console.WriteLine($"Springer duplikat TitleWriter post over for TConst={writer.TConst}, NConst={writer.NConst}");
                    continue;
                }


                if (!existingNConstCache.Contains(writer.Nconst))
                {
                    if (CheckPersonExists(sqlConn, transAction, writer.Nconst) && CheckTitleExists(sqlConn, transAction, writer.Tconst))
                    {
                        existingNConstCache.Add(writer.Nconst);
                    }
                    else
                    {
                        // Console.WriteLine($"Springer ikke-eksisterende Person post over for nconst {director.NConst}");
                        continue;
                    }
                }

                try
                {
                    titPar.Value = checkObjectForNull(writer.Tconst);
                    perPar.Value = checkObjectForNull(writer.Nconst);

                    //Console.WriteLine($"Indsætter Director med TConst={titlePar.Value}, NConst={personPar.Value}");
                    writerSQLComm.ExecuteNonQuery();
                    insertedTitleWriters.Add(writerKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Indsættelse fejlede for Tconst={writer.Tconst}, Nconst={writer.Nconst}: {ex.Message}");
                }
            }
            Console.WriteLine("Director SQL kommando udført..");

            #endregion

            #region Director indsættelse
            if (titleDirector == null)
            {
                Console.WriteLine("Fejl: titleDirector listen er null.");
                return; // Afslut hvis null for at undgå null reference problemer.
            }

            if (sqlConn == null || transAction == null)
            {
                Console.WriteLine("Fejl: SQL forbindelse eller transaktion er null.");
                return;
            }

            string directorSQL = "INSERT INTO [TitleDirectors]([Tconst], [Nconst]) VALUES (@Tconst, @Nconst)";
            SqlCommand directorSQLComm = new SqlCommand(directorSQL, sqlConn, transAction);
            SqlParameter titlePar = new SqlParameter("@Tconst", SqlDbType.NVarChar, 50);
            SqlParameter personPar = new SqlParameter("@Nconst", SqlDbType.NVarChar, 50);
            directorSQLComm.Parameters.Add(titlePar);
            directorSQLComm.Parameters.Add(personPar);

            try
            {
                directorSQLComm.Prepare();
                Console.WriteLine("Directors SQL kommando forberedt...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kunne ikke forberede SQL kommando: " + ex.Message);
                return;
            }

            foreach (TitleDirector director in titleDirector)
            {
                if (director == null || string.IsNullOrEmpty(director.Nconst))
                {
                    //Console.WriteLine("Fejl: Stødte på null TitleDirector objekt eller manglende nconst.");
                    continue;
                }

                if (!existingNConstCache.Contains(director.Nconst))
                {
                    if (CheckPersonExists(sqlConn, transAction, director.Nconst) && CheckTitleExists(sqlConn, transAction, director.Tconst))
                    {
                        existingNConstCache.Add(director.Nconst);
                    }
                    else
                    {
                        // Console.WriteLine($"Springer ikke-eksisterende Person post over for nconst {director.NConst}");
                        continue;
                    }
                }

                try
                {
                    titlePar.Value = checkObjectForNull(director.Tconst);
                    personPar.Value = checkObjectForNull(director.Nconst);

                    //Console.WriteLine($"Indsætter Director med TConst={titlePar.Value}, NConst={personPar.Value}");
                    directorSQLComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Indsættelse fejlede for Tconst={director.Tconst}, Nconst={director.Nconst}: {ex.Message}");
                }
            }
            Console.WriteLine("Director SQL kommando udført..");
            #endregion

        }

        public bool CheckPersonExists(SqlConnection sqlConn, SqlTransaction transAction, string nconst)
        {
            if (sqlConn == null || transAction == null || string.IsNullOrEmpty(nconst))
            {
                Console.WriteLine("CheckPersonExists stødte på null eller tom parameter(er).");
                return false;
            }

            using (SqlCommand cmd = new SqlCommand("CheckPersonExists", sqlConn, transAction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Nconst", SqlDbType.NVarChar, 50) { Value = nconst });

                // Tilføj output parameter
                SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(existsParam);

                try
                {
                    cmd.ExecuteNonQuery();
                    bool exists = (bool)existsParam.Value;
                    Console.WriteLine($"CheckPersonExists resultat for nconst={nconst}: {exists}");
                    return exists;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fejl i CheckPersonExists for nconst={nconst}: {ex.Message}");
                    return false;
                }
            }
        }

        public bool CheckTitleExists(SqlConnection sqlConn, SqlTransaction transAction, string tconst)
        {
            if (sqlConn == null || transAction == null || string.IsNullOrEmpty(tconst))
            {
                Console.WriteLine("CheckPersonExists stødte på null eller tom parameter(er).");
                return false;
            }
            using (SqlCommand cmd = new SqlCommand("CheckTitleExists", sqlConn, transAction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Tconst", SqlDbType.NVarChar, 50) { Value = tconst });

                // Tilføj output parameter
                SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(existsParam);

                try
                {
                    cmd.ExecuteNonQuery();
                    bool exists = (bool)existsParam.Value;
                    // Console.WriteLine($"CheckPersonExists resultat for nconst={tconst}: {exists}");
                    return exists;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Fejl i CheckPersonExists for nconst={tconst}: {ex.Message}");
                    return false;
                }
            }


        }

        public object checkObjectForNull(Object? value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }

    }
}
