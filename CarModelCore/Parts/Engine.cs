using System;
using System.Collections.Generic;
using System.Text;

namespace CarModelCore.Parts
{
    // Класс двигателя
    public class Engine
    {
        public string Model { get; private set; }
        public int HorsePower { get; private set; }

        public Engine(string model, int hp)
        {
            Model = model;
            HorsePower = hp;
        }

        public override string ToString() => $"{Model} ({HorsePower} hp)";
    }
}
