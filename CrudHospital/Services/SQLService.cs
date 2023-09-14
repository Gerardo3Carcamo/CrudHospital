using Microsoft.Data.SqlClient;
using System.Reflection;

namespace CrudHospital.Services
{
    public class SQLService
    {
        public static List<T> GetItems<T>(SqlDataReader reader) where T : new()
        {
            List<T> items = new List<T>();
            var properties = typeof(T).GetProperties();

            while (reader.Read())
            {
                T item = new T();
                foreach (PropertyInfo property in properties)
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                    {
                        property.SetValue(item, reader[property.Name]);
                    }
                }
                items.Add(item);
            }
            return items;
        }
        public static List<T> SelectMethod<T>(string qry, string? connectionString = null, bool production = false, Dictionary<string, object?>? param = null) where T : new()
        {
            List<T> datalist = new List<T>();
            using (var cnx = new SqlConnection(connectionString))
            {
                try
                {
                    cnx.Open();
                    SqlCommand cmd = new SqlCommand(qry, cnx);
                    if (param != null)
                    {
                        param.ToList().ForEach(x => cmd.Parameters.AddWithValue(x.Key, x.Value ?? DBNull.Value));
                    }
                    cmd.CommandTimeout = 600;
                    SqlDataReader reader = cmd.ExecuteReader();
                    datalist.AddRange(GetItems<T>(reader));
                    return datalist;
                }
                catch (Exception)
                {
                    cnx.Close();
                    cnx.Dispose();
                    throw;
                }
                finally
                {
                    cnx.Close();
                    cnx.Dispose();
                }
            }

        }
        
    }
}
