#nullable disable
using FastTool.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace DBRandomizer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DataBaseClient client;
        private string fileName;
        private ObservableCollection<TableViewModel> tables = new();

        public string FileName => fileName;

        public ObservableCollection<TableViewModel> Tables => tables;

        public ICommand OpenDataBase => new RelayCommand(OpenDataBaseExecute);

        private void OpenDataBaseExecute(object obj)
        {
            var openFile = new OpenFileDialog();

            openFile.Filter = "Database files (*.db)|*.db|All files(*.*)|*.*";

            if (openFile.ShowDialog() == DialogResult.Cancel)
            {
                client = new DataBaseClient(fileName = "C:\\Users\\Roman\\Documents\\Projects\\DBRandomizer\\DBRandomizer.Tests\\bin\\Debug\\netcoreapp3.1\\test.db");
                OnPropertyChanged(nameof(FileName));

                Refresh?.Execute(obj);
                return;
            }

            client = new DataBaseClient(fileName = openFile.FileName);

            OnPropertyChanged(nameof(FileName));

            Refresh?.Execute(obj);
        }

        public ICommand Refresh => new RelayCommand(RefreshExecute);

        private void RefreshExecute(object obj)
        {
            tables.Clear();
            foreach (var table in client.DBInfo)
            {
                var dataTable = new DataTable();
                foreach (var c in table.Value)
                {
                    var collumn = new DataColumn(c.Name, c.Type);

                    collumn.Unique = c.U;
                    collumn.AllowDBNull = !c.NN;
                    collumn.AutoIncrement = c.AI;

                    dataTable.Columns.Add(collumn);
                }

                dataTable.PrimaryKey = table.Value.Where(c => c.PK).Select(c => dataTable.Columns[c.Name]).ToArray();

                var reader = client.ExecuteReader($"SELECT * FROM {table.Key}");

                foreach (DbDataRecord row in reader)
                {
                    dataTable.Rows.Add(Enumerable.Range(0, row.FieldCount).Select(row.GetValue).ToArray());
                }

                reader.Close();

                tables.Add(new TableViewModel(table.Key, table.Value, dataTable));
            }

            OnPropertyChanged(nameof(Tables));
        }

        #region PropertyChanged
#nullable disable
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
#nullable enable
        #endregion
    }
}
