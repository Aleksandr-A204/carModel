using System;
using System.Collections.Generic;
using System.Text;

namespace CarModelCore.Humans
{
    // Владелец / водитель
    public class Driver
    {
        public string Name { get; private set; }
        public int ExperienceYears { get; private set; }

        public Driver(string name, int experience)
        {
            Name = name;
            ExperienceYears = experience;
        }

        public override string ToString() => $"{Name} ({ExperienceYears} yrs)";
    }
}
