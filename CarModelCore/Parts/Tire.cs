using System;
using System.Collections.Generic;
using System.Text;

namespace CarModelCore.Parts
{
    // Класс шины
    public class Tire
    {
        public string Brand { get; private set; }
        public double Pressure { get; set; }

        public Tire(string brand, double pressure)
        {
            Brand = brand;
            Pressure = pressure;
        }
    }
}
