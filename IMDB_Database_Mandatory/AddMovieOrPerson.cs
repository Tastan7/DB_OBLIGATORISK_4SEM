using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Klassen AddMovieOrPerson håndterer databaseoperationer for at tilføje, opdatere og slette film og personer i en IMDB-lignende database. Den bruger SqlConnection og SqlCommand til at udføre lagrede procedurer i SQL Server.
namespace IMDB_Database_Mandatory
{
    public static class AddMovieOrPerson
    {


        public static void AddMovie(string titleType, string primaryTitle, string originalTitle, bool isAdult, int startYear, int? endYear, int? runtimeMinutes, SqlConnection conn)
        {

            using (SqlCommand cmd = new SqlCommand("AddMovie", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TitleType", titleType);
                cmd.Parameters.AddWithValue("@PrimaryTitle", primaryTitle);
                cmd.Parameters.AddWithValue("@OriginalTitle", originalTitle);
                cmd.Parameters.AddWithValue("@IsAdult", isAdult);
                cmd.Parameters.AddWithValue("@StartYear", startYear);
                cmd.Parameters.AddWithValue("@EndYear", (object)endYear ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@RuntimeMinutes", (object)runtimeMinutes ?? DBNull.Value);


                cmd.ExecuteNonQuery();

            }

        }
        public static void AddPerson(string primaryName, int birthYear, int? deathYear, SqlConnection conn)
        {

            using (SqlCommand cmd = new SqlCommand("AddPerson", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PrimaryName", primaryName);
                cmd.Parameters.AddWithValue("@BirthYear", birthYear);
                cmd.Parameters.AddWithValue("@DeathYear", (object)deathYear ?? DBNull.Value);


                cmd.ExecuteNonQuery();
            }

        }



        public static void UpdateMovie(string tconst, SqlConnection conn)
        {
            SqlConnection SqlConn = conn;
          
            // Beder brugeren om hver felt; tryk Enter for at springe opdateringen af det felt over

            Console.WriteLine("Opdaterer filmoplysninger. Tryk Enter for at beholde den eksisterende værdi.");

            // Title Type
            Console.Write("Indtast ny Titel Type: ");
            string titleType = Console.ReadLine();
            titleType = string.IsNullOrWhiteSpace(titleType) ? null : titleType;

            // Primary Title
            Console.Write("Indtast ny Primær Titel: ");
            string primaryTitle = Console.ReadLine();
            primaryTitle = string.IsNullOrWhiteSpace(primaryTitle) ? null : primaryTitle;

            // Original Title
            Console.Write("Indtast ny Original Titel: ");
            string originalTitle = Console.ReadLine();
            originalTitle = string.IsNullOrWhiteSpace(originalTitle) ? null : originalTitle;

            // Is Adult
            Console.Write("Er voksen (0 for Nej, 1 for Ja, tryk Enter for at springe over): ");
            string isAdultInput = Console.ReadLine();
            bool? isAdult = string.IsNullOrWhiteSpace(isAdultInput) ? (bool?)null : isAdultInput == "1";

            // Start Year
            Console.Write("Indtast nyt Startår: ");
            string startYearInput = Console.ReadLine();
            int? startYear = string.IsNullOrWhiteSpace(startYearInput) ? (int?)null : int.Parse(startYearInput);

            // End Year
            Console.Write("Indtast nyt Slutår: ");
            string endYearInput = Console.ReadLine();
            int? endYear = string.IsNullOrWhiteSpace(endYearInput) ? (int?)null : int.Parse(endYearInput);

            // Runtime Minutes
            Console.Write("Indtast ny Spilletid i minutter: ");
            string runtimeMinutesInput = Console.ReadLine();
            int? runtimeMinutes = string.IsNullOrWhiteSpace(runtimeMinutesInput) ? (int?)null : int.Parse(runtimeMinutesInput);

            // Call the method to execute the update
            ExecuteUpdateMovie(conn, tconst, titleType, primaryTitle, originalTitle, isAdult, startYear, endYear, runtimeMinutes);
        }

        private static void ExecuteUpdateMovie(SqlConnection conn, string tconst, string titleType = null, string primaryTitle = null, string originalTitle = null, bool? isAdult = null, int? startYear = null, int? endYear = null, int? runtimeMinutes = null)
        {

            using (SqlCommand cmd = new SqlCommand("UpdateMovie", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Tconst", tconst);
                cmd.Parameters.AddWithValue("@TitleType", (object)titleType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PrimaryTitle", (object)primaryTitle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@OriginalTitle", (object)originalTitle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsAdult", (object)isAdult ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StartYear", (object)startYear ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EndYear", (object)endYear ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@RuntimeMinutes", (object)runtimeMinutes ?? DBNull.Value);


                cmd.ExecuteNonQuery();
            }

        }


        public static void DeleteMovie(string tconst, SqlConnection conn)
        {

            using (SqlCommand cmd = new SqlCommand("DeleteMovie", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Tconst", tconst);


                cmd.ExecuteNonQuery();
            }

        }



    }


}
