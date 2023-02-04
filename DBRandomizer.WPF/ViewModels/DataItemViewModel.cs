#nullable disable
using FastTool.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DBRandomizer.ViewModels
{
    public class DataItemViewModel : INotifyPropertyChanged
    {
        private string str = "";
        private Action<object> del;

        public string String
        {
            get => str;
            set
            {
                str = value;
                OnPropertyChanged();
            }
        }

        public DataItemViewModel(Action<object> del) => this.del = del;

        public ICommand DeleteString => new RelayCommand(del);

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
