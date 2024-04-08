using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL;
using VisStatsBL.Interfaces;
using VisStatsBL.Model;

namespace VisStatsDL_SQL {
    public class VisStatsRepository : IVisStatsRepository
    {
        private string connectionString;

        public VisStatsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool HeeftVissoort(Vissoort vissoort)
        {
            string SQL = "SELECT count(*) FROM Soort WHERE naam=@naam";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@naam", vissoort.Naam);
                int n = (int)cmd.ExecuteScalar();
                if (n > 0) return true; else return false;
            }
        }

        public void SchrijfVissoort(Vissoort vissoort)
        {
            string SQL = "INSERT INTO Soort(naam) VALUES(@naam)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                cmd.Parameters["@naam"].Value = vissoort.Naam;
                cmd.ExecuteNonQuery();
            }
        }

        public bool HeeftHaven(Haven haven)
        {
            string SQL = "SELECT COUNT(*) FROM Havens WHERE naam=@naam";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@naam", haven.Naam);
                int n = (int)cmd.ExecuteScalar();
                if (n > 0) return true; else return false;
            }
        }

        public void SchrijfHaven(Haven haven)
        {
            string SQL = "INSERT INTO Havens (naam) VALUES (@naam)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                cmd.Parameters["@naam"].Value = haven.Naam;
                cmd.ExecuteNonQuery();
            }
        }

        public bool IsOpgeladen(string fileName)
        {
            string SQL = "SELECT COUNT(*) FROM Upload WHERE filename=@filename";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@filename", fileName.Substring(fileName.LastIndexOf("\\") + 1));
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception e)
                {
                    throw new Exception("LeesSoorten", e);
                }
            }
        }

        public List<Haven> LeesHavens()
        {
            string SQL = "SELECT * FROM Havens";
            List<Haven> havens = new List<Haven>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        havens.Add(new Haven((int)reader["id"], (string)reader["naam"]));
                    }
                    return havens;
                }
                catch (Exception e)
                {
                    throw new Exception("LeesHavens", e);
                }
            }
        }


        public List<Vissoort> LeesVissoorten()
        {
            string SQL = "SELECT * FROM Soort";
            List<Vissoort> soorten = new List<Vissoort>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        soorten.Add(new Vissoort((int)reader["id"], (string)reader["naam"]));
                    }
                    return soorten;
                }
                catch (Exception e)
                {
                    throw new Exception("LeesSoorten", e);
                }
            }
        }
        public void SchrijfStatistieken(List<VisStatsDataRecord> data, string fileName)
        {
            string SQLdata = "INSERT INTO VisStats(jaar,maand,havens_id,soort_id,gewicht,waarde) VALUES(@jaar,@maand,@havens_id,@soort_id,@gewicht,@waarde)";
            string SQLupload = "INSERT INTO Upload(filename,datum,pad) VALUES(@filename,@datum,@pad)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    //schrijven data
                    cmd.CommandText = SQLdata;
                    cmd.Transaction = conn.BeginTransaction();
                    cmd.Parameters.Add(new SqlParameter("@jaar", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@maand", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@havens_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@soort_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@gewicht", SqlDbType.Float));
                    cmd.Parameters.Add(new SqlParameter("@waarde", SqlDbType.Float));
                    foreach (VisStatsDataRecord dataRecord in data)
                    {
                        cmd.Parameters["@jaar"].Value = dataRecord.Jaar;
                        cmd.Parameters["@maand"].Value = dataRecord.Maand;
                        cmd.Parameters["@havens_id"].Value = dataRecord.Haven.id;
                        cmd.Parameters["@soort_id"].Value = dataRecord.Vissoort.id;
                        cmd.Parameters["@gewicht"].Value = dataRecord.Gewicht;
                        cmd.Parameters["@waarde"].Value = dataRecord.Waarde;
                        cmd.ExecuteNonQuery();
                    }


                    //schrijven upload
                    cmd.CommandText = SQLupload;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@filename", fileName.Substring(fileName.LastIndexOf("\\") + 1));
                    cmd.Parameters.AddWithValue("@pad", fileName.Substring(fileName.LastIndexOf("\\") + 1));
                    cmd.Parameters.AddWithValue("@datum", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfStatistieken", ex);
                }
            }
        }


        public List<int> LeesJaartallen()
        {
            List<int> jaren = new List<int>();
            string SQL = "SELECT DISTINCT jaar FROM VisStats";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jaren.Add((int)reader["jaar"]);
                    }
                    return jaren;
                }
                catch (Exception e)
                {
                    throw new Exception("LeesJaren", e);
                }
            }
        }


        public List<JaarVangst> LeesStatistieken(int jaar, Haven haven, List<Vissoort> vissoorten, Eenheid eenheid)
        {
            string kolom = "";
            switch (eenheid)
            {
                case Eenheid.kg: kolom = "gewicht"; break;
                case Eenheid.euro: kolom = "waarde"; break;
            }
            string paramSoorten = "";
            for(int i = 0; i < vissoorten.Count; i++) paramSoorten += $"@ps{i},";
            paramSoorten=paramSoorten.Remove(paramSoorten.Length - 1);
            string SQL = $"SELECT naam,jaar,min({kolom})minimum,max({kolom})maximum,avg({kolom})gemiddelde,sum({kolom})totaal\r\n\r\n  FROM VisStats vs inner join soort s on vs.soort_id=s.id\r\n  WHERE jaar=@jaar and soort_id IN({paramSoorten}) and haven_id=@haven_id\r\n  group by s.naam,jaar,haven_id\r\n";
            List<JaarVangst> vangst = new();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@jaar", jaar);
                    cmd.Parameters.AddWithValue("@havens_id", haven.id);
                    for(int i = 0;i < vissoorten.Count;i++) cmd.Parameters.AddWithValue($"@ps{i}", vissoorten[i].id);
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vangst.Add(new JaarVangst((string)reader["naam"], (double)reader["totaal"], (double)reader["minimum"],
                            (double)reader["maximum"], (double)reader["gemiddelde"]));
                    }
                    return vangst;
                }
                catch (Exception e)
                {
                    throw new Exception("LeesStatistieken", e);
                }
            }
        }
    }
}
