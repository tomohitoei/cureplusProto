using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLRemoting
{
    public class GameEngine
    {
        static GameEngine _instance;
        public static GameEngine Instance()
        {
            if (null == _instance) _instance = new GameEngine();
            return _instance;
        }

        // hide constructor
        private GameEngine()
        {
        }

        public GameSettings GameSettings = new GameSettings();
        public GameContext GameContext = new GameContext();

        private float _lastProgressedTime = 0.0f;

        private float _threadProcessTime = 0.0f;
        private float _mailProcessTime = 0.0f;

        public void Progress(float delta)
        {
            _lastProgressedTime = GameContext.Time;
            GameContext.Time += (float)(delta / GameSettings.CountUnit * 1000 * GameContext.TimeScale);

            // メインループで必要になりそうな処理
            // 以下の処理を現在のゲーム時間や各パラメータを気順位行う
            // ・起動するスレッドの選出（100カウントごと
            // ・割り込みスレッドの選出（100カウントごと
            if (_threadProcessTime <= GameContext.Time)
            {
                ThreadProcess();
                _threadProcessTime += GameSettings.ThreadProcessInterval;
            }
            // ・送信するメールの選出（1カウントごと
            // ・発送待ちメールの管理 （1カウントごと
            if (_mailProcessTime <= GameContext.Time)
            {
                MailProcess();
                _mailProcessTime += GameSettings.MailProcessInterval;
            }
        }

        private int _threadCount = 0;
        public int ThreadCount()
        {
            return _threadCount;
        }
        private void ThreadProcess()
        {
            _threadCount++;
        }

        private int _mailCount = 0;
        public int MailCount()
        {
            return _mailCount;
        }
        private void MailProcess()
        {
            _mailCount++;
        }

        public int GameYear()
        {
            return (int)Math.Floor(GameContext.Time / GameSettings.CountsInDay / 30 / 12) + 1;
        }
        public int GameMonth()
        {
            return (int)Math.Floor(GameContext.Time / GameSettings.CountsInDay / 30) % 12 + 1;
        }
        public int GameDay()
        {
            return (int)Math.Floor(GameContext.Time / GameSettings.CountsInDay) % 30 + 1;
        }
        public int GameTimeCount()
        {
            return (int)Math.Floor(GameContext.Time);
        }
    }
}
