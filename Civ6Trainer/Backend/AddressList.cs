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

            UInt64 playerList = mem.ReadUInt64(unchecked((IntPtr)(TrainerFacade.Instance.GameContext.PlayerAddress)));
            
            // 玩家0
            uint playerIndex = 0;
            ResourcePageModel player0Model = new ResourcePageModel()
            {
                Caption = "玩家0",
            };
            UInt64 playerBase = mem.ReadUInt64(unchecked((IntPtr)(playerList + 8 * playerIndex + 0x210)));
            UInt64 playerTreasury = unchecked(playerBase + 0x7D60);
            player0Model.AddressDict.Add("Gold", new Int64Scale256AddressInfo()
            {
                Address = unchecked((IntPtr)(playerTreasury + 0xA0)),
            });
            UInt64 playerReligion = unchecked(playerBase + 0x6A88);
            player0Model.AddressDict.Add("Faith", new Int64Scale256AddressInfo()
            {
                Address = unchecked((IntPtr)(playerReligion + 0xA8)),
            });

            UInt64 x1 = mem.ReadUInt64(unchecked((IntPtr)(TrainerFacade.Instance.GameContext.ModuleAddress + 0x7185D8)));
            UInt64 x2 = mem.ReadUInt64(unchecked((IntPtr)(x1 + 8 * playerIndex + 0x210)));
            UInt64 x3 = unchecked(x2 + 0x6A88);
            player0Model.AddressDict.Add("XX1", new Int64Scale256AddressInfo()
            {
                Address = unchecked((IntPtr)(x3 + 0xA8)),
            });

            MenuModel.Instance.PlayerList.Add(new MenuModel.MenuItemModel()
            {
                Category = MenuModel.MenuCategory.Resource,
                IsMarked = false,
                ContentText = player0Model.Caption,
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
