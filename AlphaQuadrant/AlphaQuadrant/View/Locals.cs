using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaQuadrant
{
    class Locals
    {
        private Dictionary<string, string> ru;
        private Dictionary<string, string> eng;
        private string local;

        public Dictionary<string, string> Strings
        {
            get { return local == "eng" ? eng : ru; }
        }

        public Locals(string local = "eng")
        {
            ru = new Dictionary<string, string>();
            eng = new Dictionary<string, string>();
            this.local = local;
            FillEng();
            FillRu();
        }

        private void FillEng()
        {
            //Choose Race Screen Strings
            eng.Add("Damage", "Damage");
            eng.Add("Defence", "Defence");
            eng.Add("Speed", "Speed");
            eng.Add("Science", "Science");
            eng.Add("Product", "Product");
            eng.Add("Points", "Points");
            eng.Add("RaceName", "Race name");
            eng.Add("Start", "Start");
            eng.Add("Save", "Save");
            eng.Add("Load", "Load");
            eng.Add("Continue", "Continue");
            eng.Add("Exit", "Exit");
            eng.Add("Options", "Options");
            eng.Add("Colonize", "Colonize");
            eng.Add("Terraform", "Terraform");
            eng.Add("CreateShip", "Create Ship");
            eng.Add("StationBuilder", "Create Builder");
            //Planet Info Strings
        }

        private void FillRu()
        {
            //Choose Race Screen Strings
            ru.Add("Damage", "Урон");
            ru.Add("Defence", "Защита");
            ru.Add("Speed", "Скорость");
            ru.Add("Science", "Наука");
            ru.Add("Product", "Производство");
            ru.Add("Points", "Очки");
            ru.Add("RaceName", "Имя расы");
            ru.Add("Start", "Начать");
            ru.Add("Save", "Сохранить");
            ru.Add("Load", "Загрузить");
            ru.Add("Continue", "Продолжить");
            ru.Add("Exit", "Выход");
            ru.Add("Options", "Настройки");
            ru.Add("Colonize", "Колонизация");
            ru.Add("Terraform", "Терраформирование");
            ru.Add("CreateShip", "Создать Корабль");
            ru.Add("StationBuilder", "Создать Строителя");
            //Planet Info Strings
        }
    }
}
