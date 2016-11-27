using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace tctianchi.Civ6Trainer.ViewModel
{
    public class HelloPageModel : INotifyPropertyChanged
    {
        // singleton instance
        private static HelloPageModel _instance = new HelloPageModel();

        // get singleton instance
        public static HelloPageModel Instance
        {
            get
            {
                return _instance;
            }
        }

        private HelloPageModel()
        {
            
        }

        // 蓝色提示
        private string _promptText;
        public string PromptText { get { return _promptText; } set { onChange(ref _promptText, value); } }

        #region 修改字段时触发事件

        public event PropertyChangedEventHandler PropertyChanged;

        private bool onChange<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            return true;
        }

        #endregion
    }
}
