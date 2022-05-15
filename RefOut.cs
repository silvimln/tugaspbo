using System;
using Npgsql;
using System.Data;
using System.Configuration;

namespace RefourOutDb
{
    class program
    {
        static void Main()
        {
            string sql = "SELECT * FROM karyawan WHERE alamat = @alamat AND jenis_kelamin = @jenis_kelamin";
            NpgsqlParameter[] parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("alamat", "Jember"),
                new NpgsqlParameter("jenis_kelamin", "L"),
            };
            DataSet ds = new DataSet();
            bool succes = SqlDbHelper.ExecuteDataSet(ref ds, sql, parameters);
            if (succes)
            {
                DataTableReader reader = ds.CreateDataReader();
                while (reader.Read())
                {
                    Console.Write(reader.GetString(1) + " ");
                    Console.WriteLine(reader.GetString(2));
                }
            }

        }
    }

    static class SqlDbHelper
    {
        internal static bool ExecuteDataSet(ref DataSet ds, string sql, NpgsqlParameter[] parameters)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);

                connection.Open();

                foreach (NpgsqlParameter param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                new NpgsqlDataAdapter(cmd).Fill(ds);

                connection.Close();

                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error was happen in database");
                Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
