using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Til at håndtere databasekald
namespace IMDB_Database_Mandatory
{
    public class DatabaseService
    {
        // Connection string til databasen når den er sat korrekt op
        // private string ConnString = "server=localhost,1433;database="IKKE DEFINERET";user id="IKKE DEFINERET";password="IKKE DEFINERET";TrustServerCertificate=true";
        // Da connection string ikke er sat op endnu fordi DB er ikke oprettet i MSSQL så er der fejl i koden
        public void SearchForMovieByTitle(string title)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnString))
                try
                {
                    sqlConn.Open();

                    using (SqlCommand cmd = new SqlCommand("SearchByTitle", sqlConn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Title", title));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string? primaryTitle = reader["PrimaryTitle"].ToString();
                                    string? year = reader["StartYear"].ToString();
                                    // flere fields kan tilføjes her

                                    Console.WriteLine($"Title: {primaryTitle}, Year: {year}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No movies found with the title.");
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL error occured: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                }
        }

        public void SearchByName(string name)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnString))
                try
                {
                    sqlConn.Open();

                    using (SqlCommand cmd = new SqlCommand("SearchByName", sqlConn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Name", name));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string? primaryName = reader["PrimaryName"].ToString();
                                    // flere fields kan tilføjes her

                                    Console.WriteLine($"Name: {primaryName}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Persons with given name not found.");
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL error occured: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                }
        }
        // Midleritidig placering for AddMovieToDatabase metoden indtil den er flyttet til den korrekte fil 
        public void AddMovieToDatabase(string primaryTitle, string originalTitle, int startYear, int runtimeMinutes)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnString))
                try
                {
                    sqlConn.Open();

                    using (SqlCommand cmd = new SqlCommand("AddMovie", sqlConn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@PrimaryTitle", primaryTitle));
                        cmd.Parameters.Add(new SqlParameter("@OriginalTitle", originalTitle));
                        cmd.Parameters.Add(new SqlParameter("@StartYear", startYear));
                        cmd.Parameters.Add(new SqlParameter("@RuntimeMinutes", runtimeMinutes));

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("an SQL error occured: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("an error occured: " + ex.Message);
                }
        }

        // Midleritidig placering for AddPersonToDatabase metoden indtil den er flyttet til den korrekte fil
        public void AddPersonToDatabase(string actorName, int birthYear)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnString))
                try
                {
                    sqlConn.Open();

                    using (SqlCommand cmd = new SqlCommand("AddPerson", sqlConn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Name", actorName));
                        cmd.Parameters.Add(new SqlParameter("@BirthYear", birthYear));

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL error occured: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                }
        }

        // hvordan laver vi en metode til at opdatere en film?
        public void UpdateMovie(string movieId, string primaryTitle, string originalTitle, int startYear, int runtimeMinutes)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnString))
                try
                {
                    sqlConn.Open();

                    using (SqlCommand cmd = new SqlCommand("UpdateMovie", sqlConn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        // Hvordan får brugeren MovieId?
                        cmd.Parameters.Add(new SqlParameter("@MovieId", movieId));

                        // User inputs vil blive brugt til at opdatere filmen
                        cmd.Parameters.Add(new SqlParameter("@PrimaryTitle", primaryTitle));
                        cmd.Parameters.Add(new SqlParameter("@OriginalTitle", originalTitle));
                        cmd.Parameters.Add(new SqlParameter("@StartYear", startYear));
                        cmd.Parameters.Add(new SqlParameter("@RuntimeMinutes", runtimeMinutes));

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL error occured: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                }
        }
    }
}
