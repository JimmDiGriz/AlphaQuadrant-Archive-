using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using icsimplelib;

namespace AlphaQuadrant
{
    public partial class AlphaQuadrant : Microsoft.Xna.Framework.Game
    {
        #region Some Helpers
        private Vector2 Shift(float x, float y)
        {
            return new Vector2(x * stepX, y * stepY);
        }

        private Vector2 Shift(Vector2 position)
        {
            return new Vector2(position.X * stepX, position.Y * stepY);
        }

        private Vector2 Shift(Vector2 position, Vector2 scale)
        {
            return new Vector2(position.X * stepX * scale.X, position.Y * stepY * scale.Y);
        }

        private Vector2 GalaxyShift(Vector2 position, Vector2 scale)
        {
            return new Vector2(position.X * 1 * scale.X, position.Y * 1 * scale.Y);
        }
        private Vector2 Resize(float startScale)
        {
            return new Vector2(startScale * scaleX, startScale * scaleY);
        }
        #endregion

        #region Elements Helper
        //TODO: Рефакторнуть некоторые методы ViewHelper'a и далее короче тоже, многое можно упростить сейчас.
        /// <summary>
        /// Фабричный метод для создания кнопки со строкой.
        /// </summary>
        /// <param name="type">Тип кнопки. Пока что вохможны: blue, grey, default.</param>
        /// <param name="x">Координата по горизонтали.</param>
        /// <param name="y">Координата по вертикали.</param>
        /// <param name="scale">Масштаб.</param>
        /// <param name="str">Строка. По умолчанию пустая, хоят в данном случе стоило бы и не пустую запилить.</param>
        /// <returns>Возвращает как ни странно кнопочку ебаную.</returns>
        private Button CreateStrButton(string type, float x, float y, float scale, string str="")
        {
            Button b = CreateButton("TestButtons/TestButton", "TestButtons/OnOverTestButton", x, y, scale, str);
            switch (type.ToLower())
            {
                case "default":
                   b = CreateButton("TestButtons/TestButton", "TestButtons/OnOverTestButton", x, y, scale, str);
                   break;
                case "blue": 
                    b = CreateButton("TestButtons/TestButtonBlue", "TestButtons/OnOverTestButtonBlue", x, y, scale, str);
                    break;
                case "grey": 
                    b = CreateButton("TestButtons/TestButtonGrey", "TestButtons/OnOverTestButtonGrey", x, y, scale, str);
                    break;
                default:
                    CreateButton("TestButtons/TestButton", "TestButtons/OnOverTestButton", x, y, scale, str);
                    break;
            }
            return b;
        }

        /// <summary>
        /// Создает няшнную кнопочку. Тащемта метод себя изживший, но пригождается пока еще.
        /// </summary>
        /// <param name="staticTexture">Текстура статичной кнопки.</param>
        /// <param name="overTexture">Текстура кнопки при наведении на нее курсора мыши.</param>
        /// <param name="x">Координата по горизонтали.</param>
        /// <param name="y">Координата по вертикали.</param>
        /// <param name="scale">Масштаб.</param>
        /// <param name="str">Строка, если требуется. По дефолту пустая.</param>
        /// <returns>Возвращает кнопку епта.</returns>
        private Button CreateButton(string staticTexture, string overTexture, float x, float y, float scale, string str = "")
        {
            //TODO Выяснить, что за пиздец со звуком. И с песней тоже.
            //todo test
            SoundEffect click;
            try
            {
                click = Content.Load<SoundEffect>("Sound/Click");
                SoundEffect.MasterVolume = 0.1f;
            }
            catch (NoAudioHardwareException ex)
            {
                click = null;
            }

            GameString gs = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"), str, Vector2.Zero, Color.Azure);
            return new Button(Content.Load<Texture2D>("UI/Buttons/"+staticTexture),
                            Content.Load<Texture2D>("UI/Buttons/"+overTexture),
                            Content.Load<Texture2D>("UI/Buttons/"+overTexture),
                            Shift(x, y), Resize(scale), click, gs);
        }

        /// <summary>
        /// Ололо, ебись он овсе конем, да это же ФАБРИЧНЫЙ МЕТОД! Быстренько собирает варианты текстбокса.
        /// </summary>
        /// <param name="variant">Необходимый вариант текстбокса. square или другой.</param>
        /// <param name="font">Шрифт.</param>
        /// <param name="x">Координата по горизонтали.</param>
        /// <param name="y">Координата по вертикали.</param>
        /// <param name="scale">Масштаб.</param>
        /// <returns>Возвращает, как ни странно, текстбокс.</returns>
        private TextBox CreateTextBox(string variant, string font, float x, float y, float scale)
        {
            if (variant.ToLower() == "square")
            {
                return new TextBox(Content.Load<Texture2D>("UI/TextBox/SquareTextBox"),
                                    Content.Load<Texture2D>("UI/TextBox/OnOverSquareTextBox"),
                                    Content.Load<Texture2D>("UI/TextBox/ActiveSquareTextBox"),
                                    Content.Load<SpriteFont>("Fonts/"+font),
                                    Shift(x, y), Resize(scale));
            }
            return new TextBox(Content.Load<Texture2D>("UI/TextBox/TextBox"),
                                    Content.Load<Texture2D>("UI/TextBox/OnOverTextBox"),
                                    Content.Load<Texture2D>("UI/TextBox/ActiveTextBox"),
                                    Content.Load<SpriteFont>("Fonts/"+font),
                                    Shift(x, y), Resize(scale));
        }

        /// <summary>
        /// Создаем ебучий бэкграунд.
        /// </summary>
        /// <param name="texture">Текстура.</param>
        /// <param name="scale">Масштаб.</param>
        /// <param name="alpha">Альфа.</param>
        /// <returns>Возвращает бэкграунд.</returns>
        private BackGround CreateBackground(string texture, float scale, float alpha, bool isResizing = true)
        {
            return isResizing == true ? new BackGround(Content.Load<Texture2D>(texture), Resize(scale), alpha) 
                : new BackGround(Content.Load<Texture2D>(texture), new Vector2(scale), alpha);
        }

        /// <summary>
        /// Создает иконку. Иконка - мелкая ебола на панели например. По сути бэкграунд с заданными координатами.
        /// </summary>
        /// <param name="texture">Текстура.</param>
        /// <param name="scale">Масштаб.</param>
        /// <param name="alpha">Прозрачность.</param>
        /// <param name="position">Позиция.</param>
        /// <returns>Возвращает бэкграунд, который в данном случае является иконкой.</returns>
        private BackGround CreateIcon(string texture, float scale, float alpha, Vector2 position)
        {
            BackGround back = CreateBackground(texture, scale, alpha);
            back.Position = Shift(position);
            return back;
        }

        /// <summary>
        /// Создает игровую строку.
        /// </summary>
        /// <param name="font">Название шрифта в ресурсах игры.</param>
        /// <param name="str">Содержимое.</param>
        /// <param name="x">Координата по горизонтали.</param>
        /// <param name="y">Координата по вертикали.</param>
        /// <param name="color">Цвет.</param>
        /// <returns>Возвращает игровую строку.</returns>
        private GameString CreateGameString(string font, string str, float x, float y, Color color)
        {
            return new GameString(Content.Load<SpriteFont>("Fonts/"+font), str, Shift(x, y), color);
        }

        /// <summary>
        /// Создает прогрессбар без рамки. На данный момент не пашет.
        /// </summary>
        /// <param name="texture">Текстура прогрессбара (не рамки).</param>
        /// <param name="x">Координата по горизонтали.</param>
        /// <param name="y">координата по вертикали.</param>
        /// <param name="scale">Масштабирование.</param>
        private ProgressBar CreateProgressBar(string texture, float x, float y, float scale)
        {
            return new ProgressBar(Content.Load<Texture2D>("UI/"+texture), Shift(x, y), Resize(scale));
        }

        /// <summary>
        /// Очевидно, что метод создает попап.
        /// </summary>
        /// <param name="back">Фон попапа.</param>
        /// <param name="scale">Масштабирование попапа.</param>
        /// <param name="x">Координата по горизонтали.</param>
        /// <param name="y">Координата по вертикали.</param>
        /// <param name="isVisible">Видимость попапа.</param>
        /// <param name="isDragable">Возможно ли перетаскивание попапа с помощью мыши.</param>
        /// <returns>Возвращает, как ни странно, попапчик.</returns>
        private Popup CreatePopup(BackGround back, float scale, float x, float y, bool isVisible = false, bool isDragable = false)
        {
            Vector2 position = Shift(x, y);
            Popup temp = new Popup(position, Resize(scale), back.Width, back.Height, isVisible, isDragable);
            temp.Objects.Add("Back", back);
            return temp;
        }

        /// <summary>
        /// Рефрешит ебаный попап с планетой, чтобы применить пиздатые изменения.
        /// </summary>
        /// <param name="popup">Попапчик сам.</param>
        /// <param name="sender">Планета.</param>
        private void RefreshPlanetInfoPopup(Planet sender)
        {
            HidePlanetInfoPopup();
            ShowPlanetInfoPopup(sender);
        }

        /// <summary>
        /// Метод для заполнения попапа состояния планеты.
        /// </summary>
        /// <param name="planet">Планета, на основе инфы которой и будет рисовать ебучее няшное состояние.</param>
        private void FillPlanetState(Planet planet)
        {
            string color = planet.CenterStar.StarColor;

            //Для удобства небольшого.
            float fertility = planet.Fertility;//Пригодится для определения всяких полей, это немного так то.
            float gravity = planet.Gravity;//не нужно
            float mass = planet.Mass;//не нужно
            //Ну с температурами все предельно ясно, по сути только ради этой хуйни все и затевается.
            int maxTemp = planet.MaxTemperature; 
            int minTemp = planet.MinTemperature;
            float radioactivity = planet.Radioactivity;//хз пригодится ли
            float stability = planet.Stability;//Вулканы всякие, молние и тд
            int terraform = planet.Terraform;//Нужно ли отрисовывать всякие постройки цивилизации.
            string path = "PlanetState/"+color+"/"+color;

            //Тоже для удобства
            /*BackGround frame = ((BackGround)((Popup)screens["SolarSystem"].Objects["MoreInfoPopup"]).Objects["PlanetStateFrame"]);
            float x = frame.X;
            float y = frame.Y;
            Vector2 position = frame.Position;*/

            Vector2 position = new Vector2(104, 84);
            Popup planetInfo = ((Popup)screens["SolarSystem"].Objects["MoreInfoPopup"]);

            try
            {
                planetInfo.Objects.Remove("PlanetStateLand");
                planetInfo.Objects.Remove("PlanetStateMountain");
                planetInfo.Objects.Remove("PlanetStateClouds");
                planetInfo.Objects.Remove("PlanetStateSky");
            }
            catch
            {
                Debug.Assert(false, "Something wrong with remove");
            }

            /*
             * ПОРЯДОК:
             * Небо
             * Облака
             * Горы и тд
             * Основная область
             */



            planetInfo.Objects.Add("PlanetStateSky", CreateIcon(path + "Sky", /*HERE*/0.7f, 255f, position));
            planetInfo.Objects.Add("PlanetStateClouds", CreateIcon(path + "Clouds", /*HERE*/0.7f, 255f, position));

            //Пока только по максимальной температуре работать.
            /*
             * Для себя:
             * -20 и ниже - лед.
             * меньше 0 - снег.
             * 0-25 поля
             * 25-35 - каменная пустыня
             * 35 - 70 - пустыня
             * 70 - 150 - лава
             * 150+ - огонь
             */
            string TempType = "";

            if (maxTemp <= 0)
            {
                TempType = "Snow";
            }
            else if (maxTemp > 0 && maxTemp <= 25)
            {
                TempType = "Field";
            }
            else if (maxTemp > 25 && maxTemp <= 35)
            {
                TempType = "RockyDesert";
            }
            else if (maxTemp > 35 && maxTemp <= 70)
            {
                TempType = "Sands";
            }
            else if (maxTemp > 70 && maxTemp <= 150)
            {
                TempType = "Sands";
            }
            else
            {
                TempType = "Sands";
            }

            /*
             * Стабильность:
             * Вызывает горы.
             * Если стабильность выше выше 30%, то делаем горы
             * Если выше 60%, то вулканы
             * Если выше даже 80% то к этому доблавляем молнии.
             * По хорошему еще бы придумать, что запиливать, когда стабильность ниже 30%
             * Алсо, стоит самому себе напомнить, что чем выше показатель стабильности, тем нестабильнее
             * И вообще эт у хуйню надо по-другому назвать.
             */

            string mountainsType = "";
            switch (TempType)
            { 
                case "RockyDesert" : case "Sands":
                    mountainsType = "DesertMountain";
                    break;
                case "Snow":
                    mountainsType = "SnowMountains";
                    break;
                default:
                    mountainsType = "StoneMountains";
                    break;
            }

            if (stability > 0.3f && stability <= 0.6f)
            {
                planetInfo.Objects.Add("PlanetStateMountain", CreateIcon(path + mountainsType, 0.7f, 255f, position));
            }
            else if (stability > 0.6f && stability <= 0.8f)
            {
                planetInfo.Objects.Add("PlanetStateMountain", CreateIcon(path + mountainsType, 0.7f, 255f, position));
            }
            else if (stability > 0.8f)
            {
                planetInfo.Objects.Add("PlanetStateMountain", CreateIcon(path + mountainsType, 0.7f, 255f, position));
            }

            planetInfo.Objects.Add("PlanetStateLand", CreateIcon(path + TempType, 0.7f, 255f, position));
        }

        /*private Texture2D CreateCircle(int radius)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            using (Texture2D texture = new Texture2D(GraphicsDevice, outerRadius, outerRadius))
            {

                Color[] data = new Color[outerRadius * outerRadius];

                // Colour the entire texture transparent first.
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = Color.Transparent;
                }

                // Work out the minimum step necessary using trigonometry + sine approximation.
                double angleStep = 1f / radius;

                for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
                {
                    // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                    int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                    int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                    data[y * outerRadius + x + 1] = Color.White;
                }

                texture.SetData(data);
                return texture;
            }
        }*/
        #endregion
    }
}