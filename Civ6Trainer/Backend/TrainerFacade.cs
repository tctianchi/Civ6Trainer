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
            // 抛弃现有环境
            MenuModel.Instance.ClearAll();

            // 找到游戏
            try
            {
                _gameContext = GameContext.FindGameRunning("CivilizationVI", "GameCore_Base_FinalRelease.dll");
                if (_gameContext == null)
                {
                    // offline
                    MenuModel.Instance.ShowMessage(Properties.Resources.UITextGameIsNotRunning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MenuModel.Instance.ShowMessage(ex.Message);
                return;
            }

            // 生成修改列表
        }
    }
}
