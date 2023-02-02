using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DBRandomizer
{
    public class DataBaseClient : IDisposable
    {
        private readonly string connectionStr;

        public Dictionary<string, List<CollumnInfo>> DBInfo
        {
            get
            {
                var reader = ExecuteReader("SELECT name, sql FROM sqlite_master WHERE type='table' AND name != 'sqlite_sequence'");

                Dictionary<string, List<CollumnInfo>> tables = new();

                while (reader.Read())
                {
                    var table = GetTable(reader.GetString(0));

                    var collumns = new List<CollumnInfo>();

                    string sql = reader.GetString(1).ToLower();

                    for (int i = 0; i < table.FieldCount; i++)
                    {
                        var name = table.GetName(i);
                        var type = table.GetFieldType(i);
                        Regex inline = new(@$"\,?""?{name}""?([^,()]+)[,()]");
                        Regex unique = new(@$"unique\([^(]*({name})[^)]*\)");
                        Regex primary = new(@$"primary key\([^(]*({name})[^)]*\)");
                        Regex foreign = new(@$"foreign key\([^(]*({name})[^)]*\)");
                        Regex autoincrement = new(@$"primary key\([^(]*({name} autoincrement)[^)]*\)");

                        var nn = inline.Match(sql).Value.Contains("not null");
                        var pk = inline.Match(sql).Value.Contains("primary key") || primary.IsMatch(sql);
                        var ai = inline.Match(sql).Value.Contains("autoincrement") || autoincrement.IsMatch(sql);
                        var u = inline.Match(sql).Value.Contains("unique") || unique.IsMatch(sql);
                        var fk = inline.Match(sql).Value.Contains("unique") || foreign.IsMatch(sql);
                        string _default = Regex.Match(sql, $@"\,?""?{name}""?[^,()]+default (.+)").Groups[1].Value;

                        collumns.Add(new CollumnInfo(name, type, nn, pk, ai, u, fk, _default));
                    }

                    tables.Add(reader.GetString(0), collumns);
                }

                reader.Close();

                return tables;
            }
        }

        public DataBaseClient(string path) => connectionStr = $"Data Source={path}";

        public int ExecuteNonQuery(string query)
        {
            using var connection = new SqliteConnection(connectionStr);
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = query;
            return command.ExecuteNonQuery();
        }

        public SqliteDataReader ExecuteReader(string query)
        {
            var connection = new SqliteConnection(connectionStr);
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = query;
            return command.ExecuteReader();
        }

        public object ExecuteScalar(string query)
        {
            using var connection = new SqliteConnection(connectionStr);
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = query;
            return command.ExecuteScalar();
        }

        public SqliteDataReader GetTable(string name) => ExecuteReader($"SELECT * FROM {name}");

        public void Dispose() { }
    }
}
