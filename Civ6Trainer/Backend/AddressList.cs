using System;
using System.Collections.Generic;
using tctianchi.Civ6Trainer.ViewModel;

namespace tctianchi.Civ6Trainer.Backend
{
    #region int/float/string的内存读取

    public interface IAddressInfo
    {
        IntPtr Address { get; set; }
        string GetValue();
        void SetValue(string newValue);
    }

    public class Int64Scale256AddressInfo : IAddressInfo
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
    
    public class Int32AddressInfo : IAddressInfo
    {
        public IntPtr Address { get; set; }

        public string GetValue()
        {
            Int32 result = TrainerFacade.Instance.GameMem.ReadInt32(Address);
            return result.ToString();
        }

        public void SetValue(string newValue)
        {
            Int32 result;
            if (Int32.TryParse(newValue, out result))
            {
                TrainerFacade.Instance.GameMem.WriteInt32(Address, result);
            }
        }
    }

    public class ByteAddressInfo : IAddressInfo
    {
        public IntPtr Address { get; set; }

        public string GetValue()
        {
            int bytesRead;
            byte[] buffer = TrainerFacade.Instance.GameMem.ReadBytes(Address, sizeof(byte), out bytesRead);
            if (buffer.Length == 1)
            {
                return buffer[0].ToString();
            }
            return "";
        }

        public void SetValue(string newValue)
        {
            byte result;
            if (byte.TryParse(newValue, out result))
            {
                byte[] buffer = new byte[1] { result };
                int bytesWriten;
                TrainerFacade.Instance.GameMem.WriteBytes(
                    buffer,
                    Address, sizeof(byte), out bytesWriten);
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
            AddressListModel player0Model = new AddressListModel()
            {
                Caption = "玩家0",
            };
            MenuModel.Instance.PlayerList.Add(new MenuModel.MenuItemModel()
            {
                Category = MenuModel.MenuCategory.Resource,
                IsMarked = false,
                ContentText = player0Model.Caption,
                BubbleText = "",
                PageModel = player0Model,
            });
            UInt64 playerBase = mem.ReadUInt64(unchecked((IntPtr)(playerList + 8 * playerIndex + 0x210)));
            UInt64 playerTreasury = unchecked(playerBase + 0x7D60);
            player0Model.Add("Gold", new Int64Scale256AddressInfo()
            {
                Address = unchecked((IntPtr)(playerTreasury + 0xA0)),
            });
            UInt64 playerReligion = unchecked(playerBase + 0x6A88);
            player0Model.Add("Faith", new Int64Scale256AddressInfo()
            {
                Address = unchecked((IntPtr)(playerReligion + 0xA8)),
            });
            UInt64 playerInfluence = unchecked(playerBase + 0x7520);
            player0Model.Add("Influence", new Int64Scale256AddressInfo()
            {
                Address = unchecked((IntPtr)(playerInfluence + 0xD0)),
            });
            UInt64 playerResources = unchecked(playerBase + 0x51C8);
            UInt64 playerResourceList = mem.ReadUInt64(unchecked((IntPtr)(playerResources + 0x198)));
            for (UInt64 resourceIndex = 0; resourceIndex <= 0x30; resourceIndex++)
            {
                UInt64 resourceItem = mem.ReadUInt64(unchecked((IntPtr)(playerResourceList + 0x18 * resourceIndex)));
                player0Model.Add($"Resource{resourceIndex:X02}", new Int32AddressInfo()
                {
                    Address = unchecked((IntPtr)(resourceItem + 0x4 * 0)),
                });
            }
            UInt64 playerEras = unchecked(playerBase + 0x71D8);
            player0Model.Add("Era", new Int32AddressInfo()
            {
                Address = unchecked((IntPtr)(playerEras + 0xA0)),
            });
            UInt64 playerTrade = unchecked(playerBase + 0xA28);
            player0Model.Add("RouteCapacity", new Int32AddressInfo()
            {
                Address = unchecked((IntPtr)(playerTrade + 0x98)),
            });




            #endregion

            #region 城市
            #endregion

            #region 部队
            #endregion

            #region 研究

            AddressListModel techModel = new AddressListModel()
            {
                Caption = "自然科学",
            };
            MenuModel.Instance.ResearchList.Add(new MenuModel.MenuItemModel()
            {
                Category = MenuModel.MenuCategory.Tech,
                IsMarked = false,
                ContentText = techModel.Caption,
                BubbleText = "",
                PageModel = techModel,
            });
            UInt64 playerTechs = unchecked(playerBase + 0x66E8);
            UInt64 techList = mem.ReadUInt64(unchecked((IntPtr)(playerTechs + 0x1E0)));
            for (UInt64 i = 0; i <= 0x43; i++)
            {
                techModel.Add($"Tech{i:X02}", new ByteAddressInfo()
                {
                    Address = unchecked((IntPtr)(techList + i)),
                });
            }

            AddressListModel civicModel = new AddressListModel()
            {
                Caption = "社会科学",
            };
            MenuModel.Instance.ResearchList.Add(new MenuModel.MenuItemModel()
            {
                Category = MenuModel.MenuCategory.Civic,
                IsMarked = false,
                ContentText = civicModel.Caption,
                BubbleText = "",
                PageModel = civicModel,
            });
            UInt64 playerCulture = unchecked(playerBase + 0x4338);
            UInt64 civicList = mem.ReadUInt64(unchecked((IntPtr)(playerCulture + 0x4D8)));
            for (UInt64 i = 0; i <= 0x31; i++)
            {
                civicModel.Add($"Civic{i:X02}", new ByteAddressInfo()
                {
                    Address = unchecked((IntPtr)(civicList + i)),
                });
            }

            #endregion

            #region 测试

            AddressListModel debug1Model = new AddressListModel()
            {
                Caption = "Debug1",
            };
            MenuModel.Instance.DebugList.Add(new MenuModel.MenuItemModel()
            {
                Category = MenuModel.MenuCategory.Debug1,
                IsMarked = false,
                ContentText = debug1Model.Caption,
                BubbleText = "",
                PageModel = debug1Model,
            });
            

            #endregion
        }
    }

}
