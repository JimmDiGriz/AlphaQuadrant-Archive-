using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using icsimplelib;

namespace AlphaQuadrant
{
    [Serializable]
    public class ProductQuery : IDraw, IUpdateble
    {
        #region Fields
        private bool isVisible;
        #endregion

        #region Properties

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if (Queue.Count > 0)
                {
                    Queue.Peek().ChangeProgress = value;
                }
                isVisible = value;
            }
        }

        private Queue<GameTimer> Queue { get; set; }
        public List<Button> Buttons { get; private set; }
        #endregion

        #region Events

        private void OnTimerEnd()
        {
            Buttons.Remove(Buttons.Find(x => Object.ReferenceEquals(x, Queue.Peek().SlotBtn)));
            Queue.Dequeue();
            SetButtonsCoords();
            if (Queue.Count > 0)
            {
                Queue.Peek().ChangeProgress = isVisible;
                Queue.Peek().Start();
            }
        }

        #endregion

        #region Construct
        public ProductQuery()
        {
            Queue = new Queue<GameTimer>();
            isVisible = false;
            Buttons = new List<Button>();
        }
        #endregion

        #region Else
        public void Add(GameTimer gt)
        {
            if (Queue.Count == 5)
            {
                return;
            }
            gt.OnEnd += new GameTimer.OnEndHadler(OnTimerEnd);
            Queue.Enqueue(gt);
            Buttons.Add(gt.SlotBtn);
            //buttons coordinates
            SetButtonsCoords();
            //end coordinates
            if (Queue.Count == 1)
            {
                Queue.Peek().Start();
                Queue.Peek().ChangeProgress = IsVisible;
            }
        }

        public void SetButtonsCoords()
        {
            ProgressBar pb;
            /*try
            {
                pb = Queue.Peek().PB;
            }
            catch 
            {
                return;
            }*/
            if (Queue.Count > 0)
            {
                pb = Queue.Peek().PB;
            }
            else
            {
                return;
            }

            float x = pb.X;
            float y = pb.Y;

            for (int i = 0; i < Buttons.Count; i++)
            {
                if (Buttons[i] == null)
                {
                    continue;
                }
                if (i == 0)
                {
                    Buttons[i].X = x - Buttons[i].Width - 10;
                    Buttons[i].Y = y;
                }
                else
                {
                    Buttons[i].X = x + ((Buttons[i].Width+10) * (i - 1));
                    Buttons[i].Y = y + pb.Height + 10;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Button button in Buttons)
            {
                if (button != null)
                {
                    button.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        { 
            foreach (Button button in Buttons)
            {
                if (button != null)
                {
                    button.Draw(spriteBatch);
                }
            }
        }
        #endregion
    }
}
