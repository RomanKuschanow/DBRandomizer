using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

            foreach (var name in tableInfo.Select(c => c.Name))
                if (!dataSet.ContainsKey(name)) return false;

            Dictionary<string, int> maxIndexes = DataSet.Select(c => new KeyValuePair<string, int>(c.Key, c.Value.Count - 1)).ToDictionary(c => c.Key, c => c.Value);

            Dictionary<string, int> indexes = DataSet.Select(c => new KeyValuePair<string, int>(c.Key, 0)).ToDictionary(c => c.Key, c => c.Value);


            while (true)
            {
                string query = $"INSERT INTO {TableName}({string.Join(", ", DataSet.Select(c => c.Key))}) VALUES('{string.Join("', '", DataSet.Select(t => t.Value[indexes[t.Key]]))}')";

                try
                {
                    DataBaseClient.ExecuteNonQuery(query);
                }
                catch
                {
                }

                if (indexes.Where(c => c.Value == maxIndexes[c.Key]).Count() == indexes.Count) break;
                if (DataBaseClient.DBInfo[TableName].Where(c => c.U).Where(c => indexes[c.Name] == maxIndexes[c.Name]).Count() > 0) break;

                indexes[indexes.Where(c => c.Value < indexes[c.Key]).First().Key]++;

                foreach (var collumn in DataBaseClient.DBInfo[TableName].Where(c => c.U))
                    indexes[collumn.Name]++;
            }

            return true;
        }
    }
}
