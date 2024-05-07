using System.Xml.Linq;

namespace PT3
{
    internal class CarXMLSerializer
    {
        public static string Serialize(List<Car> cars)
        {

            XElement root = new XElement("cars");

            root.Add(cars.Select(car =>
            {
                XElement carElement = new XElement("car");
                carElement.Add(new XElement("model", car.model));
                carElement.Add(new XElement("engine", new XElement("displacement", car.engine.displacement),
                    new XElement("horsePower", car.engine.horsePower),
                    new XAttribute("model", car.engine.model)));
                carElement.Add(new XElement("year", car.year));

                return carElement;
            }));




            return root.ToString();
        }


        public static List<Car> Deserialize(string xml)
        {
            throw new NotImplementedException();
        }
    }
}
