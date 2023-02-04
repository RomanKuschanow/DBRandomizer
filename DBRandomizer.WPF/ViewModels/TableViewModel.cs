using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DBRandomizer.ViewModels
{
    public class TableViewModel : INotifyPropertyChanged
    {
        private string name;
        private DataTable data;
        private ObservableCollection<CollumnDataSetViewModel> collumns = new();

        public string Name => name;
        public DataTable Data => data;
        public ObservableCollection<CollumnDataSetViewModel> Collumns => collumns;

        public TableViewModel(string name, List<CollumnInfo> collumns, DataTable data)
        {
            this.name = name;
            OnPropertyChanged(nameof(Name));
            this.data = data;
            OnPropertyChanged(nameof(Data));

            foreach (var collumn in collumns)
                this.collumns.Add(new CollumnDataSetViewModel(collumn));
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
