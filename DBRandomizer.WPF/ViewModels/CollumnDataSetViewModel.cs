#nullable disable
using FastTool.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DBRandomizer.ViewModels
{
    public class CollumnDataSetViewModel : INotifyPropertyChanged
    {
        private CollumnInfo collumn;
        private ObservableCollection<DataItemViewModel> strings;

        public string Name => collumn.Name;
        public ObservableCollection<DataItemViewModel> Strings => strings;

        public CollumnDataSetViewModel(CollumnInfo collumn)
        {
            this.collumn = collumn;
            strings = new(new DataItemViewModel[] { new DataItemViewModel(DeleteStringExecute) });
        }

        public ICommand DeleteString => new RelayCommand(DeleteStringExecute);

        private void DeleteStringExecute(object obj)
        {
            strings.Remove((DataItemViewModel)obj);
        }

        public ICommand AddString => new RelayCommand(AddStringExecute);

        private void AddStringExecute(object obj)
        {
            strings.Add(new DataItemViewModel(DeleteStringExecute));
            OnPropertyChanged(nameof(Strings));
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
