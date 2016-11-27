using System;
using System.Collections.Generic;
using tctianchi.Civ6Trainer.ViewModel;

namespace tctianchi.Civ6Trainer.Backend
{
    #region int/float/string的内存读取

    internal interface IAddressInfo
    {
        IntPtr Address { get; set; }
        string GetValue();
        void SetValue(string newValue);
    }

    internal class Int64Scale256AddressInfo : IAddressInfo
    {
        public IntPtr Address { get; set; }

        public string GetValue()
        {
            double result = TrainerFacade.Instance.GameMem.ReadInt64(Address);
            result /= 256.0;
            return result.ToString();
        }

        public void SetValue(string newValue)
        {
            double result;
            if (double.TryParse(newValue, out result))
            {
                result *= 256.0;
                TrainerFacade.Instance.GameMem.WriteInt64(Address, (Int64)result);
            }
        }
    }

    #endregion
    
    // 所有可以修改的项目
    public static class AddressList
    {
        public static void All()
        {
            var mem = TrainerFacade.Instance.GameMem;

            #region 玩家资源

            UInt64 playersBase = mem.ReadUInt64(unchecked((IntPtr)(TrainerFacade.Instance.GameContext.PlayerAddress)));
            UInt64 playerList = mem.ReadUInt64(unchecked((IntPtr)(playersBase + 0x3A8)));

            // 玩家0
            uint playerIndex = 0;
            ResourcePageModel player0Model = new ResourcePageModel();
            UInt64 playerBase = mem.ReadUInt64(unchecked((IntPtr)(playerList + 8 * playerIndex + 0x210)));
            UInt64 playerResource = mem.ReadUInt64(unchecked((IntPtr)(playerBase + 0x7D60)));
            player0Model.AddressDict.Add("Gold", new Int64Scale256AddressInfo()
            {
                Address = unchecked((IntPtr)(playerResource + 0xA0)),
            });
            MenuModel.Instance.PlayerList.Add(new MenuModel.MenuItemModel()
            {
                Category = MenuModel.MenuCategory.Resource,
                IsMarked = false,
                ContentText = "玩家0",
                BubbleText = "",
                PageModel = player0Model,
            });

            #endregion

            #region 城市
            #endregion

            #region 部队
            #endregion

            #region 研究
            #endregion

            #region 测试
            #endregion
        }
    }

}
