using System;
using System.Collections.Generic;
using tctianchi.Civ6Trainer.ViewModel;

namespace tctianchi.Civ6Trainer.Backend
{
    // 修改器backend逻辑总入口
    class TrainerFacade
    {
        #region Singleton

        // singleton instance
        private static TrainerFacade _instance = new TrainerFacade();

        // get singleton instance
        public static TrainerFacade Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        private GameContext _gameContext;
        public GameContext GameContext { get { return _gameContext; } }
        private WindowsApi.ProcessMemory _gameMem;
        public WindowsApi.ProcessMemory GameMem { get { return _gameMem; } }

        // 返回false如果失败的话
        public bool Init()
        {
            try
            {
                System.Diagnostics.Process.EnterDebugMode();
            }
            catch
            {
                MenuModel.Instance.ShowMessage(Properties.Resources.UITextEnterDebugFailed);
                return false;
            }

            return true;
        }

        // 刷新逻辑
        public void Refresh()
        {
            try
            {
                // 抛弃现有环境
                MenuModel.Instance.ClearAll();

                // 找到游戏
                _gameContext = GameContext.FindGameRunning("CivilizationVI", "GameCore_Base_FinalRelease.dll");
                if (_gameContext == null)
                {
                    // offline
                    MenuModel.Instance.ShowMessage(Properties.Resources.UITextGameIsNotRunning);
                    return;
                }

                // 打开句柄
                if (_gameMem != null)
                {
                    _gameMem.Dispose();
                }
                _gameMem = new WindowsApi.ProcessMemory(_gameContext.ProcessId);

                // 生成修改列表
                AddressList.All();

                // 回到开始页
                MenuModel.Instance.ShowMessage(Properties.Resources.UITextPleaseSelectAMenuItem);
            }
            catch (Exception ex)
            {
                MenuModel.Instance.ShowMessage(ex.Message);
                return;
            }
        }
    }
}
