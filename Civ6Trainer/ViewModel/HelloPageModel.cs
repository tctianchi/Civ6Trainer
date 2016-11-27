using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        public string PromptText { get { return _promptText; } set { _promptText = value; onChange("PromptText"); } }

        #region 修改字段时触发事件

        public event PropertyChangedEventHandler PropertyChanged;
        private void onChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
