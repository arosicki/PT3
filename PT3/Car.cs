using System;
namespace PT3
{
    internal class Car(string model, Engine engine, int year)
    {
        public Engine engine = engine;
        public string model = model;
        public int year = year;
    }

}
