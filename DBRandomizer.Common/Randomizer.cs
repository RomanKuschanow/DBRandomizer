using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DBRandomizer
{
    public class Randomizer
    {
        private DataBaseClient dataBaseClient;
        private Dictionary<string, List<string>> dataSet = new();
        private string tableName;

        public DataBaseClient DataBaseClient 
        { 
            get => dataBaseClient;
            init => dataBaseClient = value; 
        }

        public Dictionary<string, List<string>> DataSet => dataSet = dataSet.OrderBy(c => DataBaseClient.DBInfo[TableName].Select(c => c.Name).ToList().IndexOf(c.Key)).ToDictionary(c => c.Key, c => c.Value);

        public string TableName
        {
            get => tableName;
            set => tableName = value;
        }

        public Randomizer(DataBaseClient dataBaseClient, string tableName)
        {
            this.dataBaseClient = dataBaseClient;
            this.tableName = tableName;
        }

        public bool Randomize(int count)
        {
            var tableInfo = new List<CollumnInfo>();

            if (!DataBaseClient.DBInfo.TryGetValue(TableName, out tableInfo)) return false;

            foreach (var name in DataSet.Select(c => c.Key))
                if (!tableInfo.Select(t => t.Name).Contains(name)) return false;

            Random rand = new();

            foreach (var collumn in DataSet.Where(c => tableInfo.Find(_c => c.Key == _c.Name).U))
                if (collumn.Value.Count < count)
                    count = collumn.Value.Count;

            for (int i = 0; i < count;)
            {
                string query = $"INSERT INTO {TableName}({string.Join(", ", DataSet.Select(c => c.Key))}) VALUES('{string.Join("', '", DataSet.Select(c => c.Value[rand.Next(c.Value.Count)]))}')";

                try
                {
                    DataBaseClient.ExecuteNonQuery(query);
                }
                catch
                {
                    continue;
                }
                i++;
            }

            return true;
        }

        public bool Combine()
        {
            var tableInfo = new List<CollumnInfo>();

            if (!DataBaseClient.DBInfo.TryGetValue(TableName, out tableInfo)) return false;

            foreach (var name in DataSet.Select(c => c.Key))
                if (!tableInfo.Select(t => t.Name).Contains(name)) return false;

            foreach (var item in DataSet.Select(c => c.Value).CartesianProduct())
            {
                string query = $"INSERT INTO {TableName}({string.Join(", ", DataSet.Select(c => c.Key))}) VALUES('{string.Join("', '", item)}')";

                try
                {
                    DataBaseClient.ExecuteNonQuery(query);
                }
                catch
                {
                }
            }

            return true;
        }
    }
}
