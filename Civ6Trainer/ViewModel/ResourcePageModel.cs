using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using tctianchi.Civ6Trainer.Backend;

namespace tctianchi.Civ6Trainer.ViewModel
{
    public class ResourcePageModel : INotifyPropertyChanged
    {
        // 修改列表
        internal readonly Dictionary<string, IAddressInfo> AddressDict = new Dictionary<string, IAddressInfo>();

        // 页面标题
        private string _caption;
        public string Caption { get { return onRead(ref _caption); } set { onChange(ref _caption, value); } }

        // 金币
        private string _gold;
        public string Gold { get { return onRead(ref _gold); } set { onChange(ref _gold, value); } }

        // 信仰值
        private string _faith;
        public string Faith { get { return onRead(ref _faith); } set { onChange(ref _faith, value); } }

        // 测试
        private string _xx1;
        public string XX1 { get { return onRead(ref _xx1); } set { onChange(ref _xx1, value); } }

        #region 修改字段时触发事件

        public event PropertyChangedEventHandler PropertyChanged;

        private string onRead(ref string storage, [CallerMemberName] string propertyName = null)
        {
            if (AddressDict.ContainsKey(propertyName))
            {
                string result = AddressDict[propertyName].GetValue();
                storage = result;
            }
            return storage;
        }

        private bool onChange(ref string storage, string value, [CallerMemberName] string propertyName=null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            if (AddressDict.ContainsKey(propertyName))
            {
                AddressDict[propertyName].SetValue(value);
            }
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            return true;
        }

        #endregion
    }
}
