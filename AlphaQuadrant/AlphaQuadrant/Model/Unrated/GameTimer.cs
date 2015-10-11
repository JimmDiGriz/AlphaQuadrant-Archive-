using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using icsimplelib;
using System.Timers;
using Microsoft.Xna.Framework.Graphics;

namespace AlphaQuadrant
{
    /// <summary>
    /// Принимает ссылку на прогрессбар необходимый, шаг за секунду (в прцоентах от длины прогрессбара)
    /// И колбек, который ебашится по завершении.
    /// Вроде должно проканать, но хуй его знает.
    /// </summary>
    [Serializable]
    public class GameTimer
    {
        #region Fields
        private ProgressBar progressbar;
        private Timer timer;
        private Action callback;
        private int step;
        private double interval;
        private bool changeProgress;
        private int width;
        public Button SlotBtn {get;set;}
        #endregion

        #region Properties
        public bool ChangeProgress
        {
            get { return changeProgress; }
            set
            {
                changeProgress = value;
                progressbar.PWidth = 0;
            }
        }

        public ProgressBar PB
        {
            get { return progressbar; }
        }
        #endregion

        #region Events
        public delegate void OnEndHadler();
        public event OnEndHadler OnEnd;
        #endregion

        #region Constructors
        public GameTimer(ProgressBar progressbar, float percent, Action callback, Button slotBtn, double interval = 1000d)
        {
            this.progressbar = progressbar;
            this.callback = callback;
            try
            {
                step = Convert.ToInt32(percent * progressbar.TextureWidth);
            }
            catch 
            {
                //just for lulz.
                try
                {
                    step = Convert.ToInt32(0.01f * progressbar.TextureWidth);
                }
                catch
                {
                    step = 1;
                }
            }
            this.interval = interval;
            changeProgress = false;
            width = 0;
            //isBeingStarted = false;
            timer = new Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(Tick);
            SlotBtn = slotBtn;
        }
        #endregion

        #region Else
        public void Start()
        {
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Enabled = false;
        }

        private void Tick(object source, ElapsedEventArgs e)
        {
            /*
             * Change progressbar width only when flag is true.
             * Queue manage timers flags.
             * But false flag doesnot meaning that timer have no progress.
             * Temporary width will storage in a width value, which will be calculated.
             * All this shit provide me to have no false progressbars on the planet windows.
             */
            if (width + step <= progressbar.TextureWidth)
            {
                width += step;
            }
            else
            {
                width = Convert.ToInt32(progressbar.TextureWidth);
            }
            if (changeProgress)
            {
                progressbar.PWidth = width;
            }
            if (width == progressbar.TextureWidth)
            {
                timer.Enabled = false;
                if (callback != null)
                {
                    callback();
                }
                if (OnEnd != null)
                {
                    OnEnd();
                }
            }
        }
        #endregion
    }
}
