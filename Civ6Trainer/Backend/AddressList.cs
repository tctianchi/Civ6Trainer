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

    public class UInt32Scale256AddressInfo : IAddressInfo
    {
        public IntPtr Address { get; set; }

        public string GetValue()
        {
            double result = TrainerFacade.Instance.GameMem.ReadUInt32(Address);
            result /= 256.0;
            return result.ToString();
        }

        public void SetValue(string newValue)
        {
            double result;
            if (double.TryParse(newValue, out result))
            {
                result *= 256.0;
                TrainerFacade.Instance.GameMem.WriteUInt32(Address, (UInt32)result);
            }
        }
    }

    public class UInt32AddressInfo : IAddressInfo
    {
        public IntPtr Address { get; set; }

        public string GetValue()
        {
            UInt32 result = TrainerFacade.Instance.GameMem.ReadUInt32(Address);
            return result.ToString();
        }

        public void SetValue(string newValue)
        {
            UInt32 result;
            if (UInt32.TryParse(newValue, out result))
            {
                TrainerFacade.Instance.GameMem.WriteUInt32(Address, result);
            }
        }
    }

    public class UInt16AddressInfo : IAddressInfo
    {
        public IntPtr Address { get; set; }

        public string GetValue()
        {
            UInt16 result = TrainerFacade.Instance.GameMem.ReadUInt16(Address);
            return result.ToString();
        }

        public void SetValue(string newValue)
        {
            UInt16 result;
            if (UInt16.TryParse(newValue, out result))
            {
                TrainerFacade.Instance.GameMem.WriteUInt16(Address, result);
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
    public class AddressList
    {
        #region Singleton

        // singleton instance
        private static AddressList _instance = new AddressList();

        // get singleton instance
        public static AddressList Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        public void All()
        {
            var mem = TrainerFacade.Instance.GameMem;
            UInt64 constTextList = mem.ReadUInt64(unchecked((IntPtr)(TrainerFacade.Instance.GameContext.ConstTextAddress)));
            UInt64 playerList = mem.ReadUInt64(unchecked((IntPtr)(TrainerFacade.Instance.GameContext.PlayerAddress)));
            uint playerIndex = 0;
            UInt64 playerBase = mem.ReadUInt64(unchecked((IntPtr)(playerList + 8 * playerIndex + 0x210)));

            #region 玩家资源

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
                player0Model.Add($"Resource{resourceIndex:X02}", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(resourceItem + 0x4 * 0)),
                });
            }
            UInt64 playerEras = unchecked(playerBase + 0x71D8);
            player0Model.Add("Era", new UInt32AddressInfo()
            {
                Address = unchecked((IntPtr)(playerEras + 0xA0)),
            });
            UInt64 playerTrade = unchecked(playerBase + 0xA28);
            player0Model.Add("RouteCapacity", new UInt32AddressInfo()
            {
                Address = unchecked((IntPtr)(playerTrade + 0x98)),
            });

            #endregion

            #region 城市

            // 先遍历
            List<UInt64> cityList = new List<UInt64>();
            UInt64 playerCities = unchecked(playerBase + 0x1738);
            UInt64 nextCity = mem.ReadUInt64(unchecked((IntPtr)(playerCities + 0xB8)));
            while (nextCity != 0)
            {
                UInt64 city = mem.ReadUInt64(unchecked((IntPtr)(nextCity + 0)));
                cityList.Add(city);
                nextCity = mem.ReadUInt64(unchecked((IntPtr)(nextCity + 0x10)));
            }
            foreach (var city in cityList)
            {
                // 名称
                UInt64 cityNamePointer = mem.ReadUInt64(unchecked((IntPtr)(city + 0x718)));
                string cityName = mem.ReadCString(unchecked((IntPtr)(cityNamePointer + 0)));
                cityName = GameTranslation.Instance.GetNameFromKey(cityName);

                // 模型
                AddressListModel cityModel = new AddressListModel()
                {
                    Caption = cityName,
                };
                MenuModel.Instance.CityList.Add(new MenuModel.MenuItemModel()
                {
                    Category = MenuModel.MenuCategory.City,
                    IsMarked = false,
                    ContentText = cityModel.Caption,
                    BubbleText = "",
                    PageModel = cityModel,
                });

                // 属性
                cityModel.Add("Population", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(city + 0x1E8)),
                });

                // 增长
                UInt64 growth = unchecked(city + 0x970 + 0x20);
                cityModel.Add("HousingFromCivics", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(growth + 0x30)),
                });
                cityModel.Add("HousingFromGreatPeople", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(growth + 0x34)),
                });
                cityModel.Add("HousingFromStartingEra", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(growth + 0x38)),
                });
                cityModel.Add("AmenitiesFromEntertainment", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(growth + 0x3C)),
                });
                cityModel.Add("AmenitiesFromCivics", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(growth + 0x40)),
                });
                cityModel.Add("AmenitiesFromGreatPeople", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(growth + 0x44)),
                });
                cityModel.Add("AmenitiesFromReligion", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(growth + 0x48)),
                });
                cityModel.Add("AmenitiesFromStartingEra", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(growth + 0x50)),
                });
                UInt64 production = mem.ReadUInt64(unchecked((IntPtr)(growth + 0)));
                cityModel.Add("YIELD_FROM_GAMEEFFECTS_0", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 0 + 0x500)),
                });
                cityModel.Add("YIELD_FROM_GAMEEFFECTS_1", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 1 + 0x500)),
                });
                cityModel.Add("YIELD_FROM_GAMEEFFECTS_2", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 2 + 0x500)),
                });
                cityModel.Add("YIELD_FROM_GAMEEFFECTS_3", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 3 + 0x500)),
                });
                cityModel.Add("YIELD_FROM_GAMEEFFECTS_4", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 4 + 0x500)),
                });
                cityModel.Add("YIELD_FROM_GAMEEFFECTS_5", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 5 + 0x500)),
                });
                cityModel.Add("YIELD_FROM_MODIFIER_GAMEEFFECTS_0", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 0 + 0x450)),
                });
                cityModel.Add("YIELD_FROM_MODIFIER_GAMEEFFECTS_1", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 1 + 0x450)),
                });
                cityModel.Add("YIELD_FROM_MODIFIER_GAMEEFFECTS_2", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 2 + 0x450)),
                });
                cityModel.Add("YIELD_FROM_MODIFIER_GAMEEFFECTS_3", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 3 + 0x450)),
                });
                cityModel.Add("YIELD_FROM_MODIFIER_GAMEEFFECTS_4", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 4 + 0x450)),
                });
                cityModel.Add("YIELD_FROM_MODIFIER_GAMEEFFECTS_5", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 4 * 5 + 0x450)),
                });
                cityModel.Add("YIELD_FROM_BUILDING_PRODUCTION_BONUS", new UInt32AddressInfo()
                {
                    Address = unchecked((IntPtr)(production + 0x3E0)),
                });
            }

            #endregion

            #region 部队

            // 遍历
            List<UInt64> unitList = new List<UInt64>();
            UInt64 playerUnits = unchecked(playerBase + 0x10A0);
            UInt64 nextUnit = mem.ReadUInt64(unchecked((IntPtr)(playerUnits + 0xB8)));
            while (nextUnit != 0)
            {
                UInt64 unit = mem.ReadUInt64(unchecked((IntPtr)(nextUnit + 0)));
                unitList.Add(unit);
                nextUnit = mem.ReadUInt64(unchecked((IntPtr)(nextUnit + 0x10)));
            }
            UInt64 unitNameConstText1 = mem.ReadUInt64(unchecked((IntPtr)(constTextList + 0x20)));
            UInt64 unitNameConstText2 = mem.ReadUInt64(unchecked((IntPtr)(unitNameConstText1 + 0xDE0)));
            foreach (var unit in unitList)
            {
                // 名称
                // 应该先读伟人的名字，但我懒所以跳过。这里一律显示unit type的名字
                UInt32 unitType = mem.ReadUInt32(unchecked((IntPtr)(unit + 0xD0)));
                UInt64 unitTypeInfo = mem.ReadUInt64(unchecked((IntPtr)(unitNameConstText2 + unitType * 2 * 8)));
                UInt64 unitNamePointer = mem.ReadUInt64(unchecked((IntPtr)(unitTypeInfo + 0x88)));
                string unitName = mem.ReadCString(unchecked((IntPtr)(unitNamePointer)));
                unitName = GameTranslation.Instance.GetNameFromKey(unitName);

                // 模型
                AddressListModel unitModel = new AddressListModel()
                {
                    Caption = unitName,
                };
                MenuModel.Instance.ArmyList.Add(new MenuModel.MenuItemModel()
                {
                    Category = MenuModel.MenuCategory.Army,
                    IsMarked = false,
                    ContentText = unitModel.Caption,
                    BubbleText = "",
                    PageModel = unitModel,
                });

                // 其他
            }

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

            // 每回合科研虽说是在自然科学分类，但为了修改器的易用性所以移动到资源页
            player0Model.Add("SCIENCE_FROM_OTHER", new UInt32Scale256AddressInfo()
            {
                Address = unchecked((IntPtr)(playerTechs + 0x1A8)),
            });

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

            // 每回合文化虽说是在社会科学分类，但为了修改器的易用性所以移动到资源页
            player0Model.Add("CULTURE_FROM_OTHER", new UInt32Scale256AddressInfo()
            {
                Address = unchecked((IntPtr)(playerCulture + 0x6E8)),
            });

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

            foreach (var city in cityList)
            {

            }

            #endregion
        }
    }

}
