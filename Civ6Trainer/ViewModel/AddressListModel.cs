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
    public class AddressListModel : INotifyPropertyChanged
    {
        public ObservableDictionary<string, AddressListModelItem> AddressList { get; private set; }
        
        #region 表项

        public class AddressListModelItem : INotifyPropertyChanged
        {
            public string Name { get; set; }
            public IAddressInfo Value { get; set; }

            private string _valueText;
            public string ValueText { get { return onRead(ref _valueText); } set { onChange(ref _valueText, value); } }

            #region 修改字段时触发事件

            public event PropertyChangedEventHandler PropertyChanged;
            private string onRead(ref string storage, [CallerMemberName] string propertyName = null)
            {
                string result = Value.GetValue();
                _valueText = result;
                return _valueText;
            }

            private bool onChange(ref string storage, string value)
            {
                if (object.Equals(_valueText, value))
                {
                    return false;
                }
                _valueText = value;
                Value.SetValue(value);
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(Name));
                }
                return true;
            }

            #endregion
        }

        #endregion

        #region 修改列表

        public AddressListModel()
        {
            AddressList = new ObservableDictionary<string, AddressListModelItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Add(string name, IAddressInfo value)
        {
            AddressList.Add(name, new AddressListModelItem()
            {
                Name = name,
                Value = value,
            });
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("AddressList"));
            }
        }

        #endregion
    }
}
