using PT3;
using System.Xml.Linq;
using System.Xml.XPath;

string FILE = "C:\\Users\\Adrian\\source\\repos\\PT3\\PT3\\cars.xml";
string LINQ_FILE = "C:\\Users\\Adrian\\source\\repos\\PT3\\PT3\\CarsFromLinq.xml";
string EMPTY_HTML = "C:\\Users\\Adrian\\source\\repos\\PT3\\PT3\\empty.html";
string HTML_FILE = "C:\\Users\\Adrian\\source\\repos\\PT3\\PT3\\index.html";

List<Car> myCars = [
new("E250", new Engine(1.8, 204, "CGI"), 2009),
 new("E350", new Engine(3.5, 292, "CGI"), 2009),
 new("A6", new Engine(2.5, 187, "FSI"), 2012),
 new("A6", new Engine(2.8, 220, "FSI"), 2012),
 new("A6", new Engine(3.0, 295, "TFSI"), 2012),
 new("A6", new Engine(2.0, 175, "TDI"), 2011),
 new("A6", new Engine(3.0, 309, "TDI"), 2011),
 new("S6", new Engine(4.0, 414, "TFSI"), 2012),
 new("S8", new Engine(4.0, 513, "TFSI"), 2012)
];


var anonymizedCars = myCars.Where(p => p.model == "A6").Select(p =>
new
{
    engineType = p.engine.model == "TDI" ? "diesel" : "petrol",
    hppl = p.engine.horsePower / p.engine.displacement
});

Console.WriteLine("=== 1a ===");
foreach (var item in anonymizedCars)
{
    Console.WriteLine($"{item.engineType}: {item.hppl}");
}

var averagePpplByType = anonymizedCars.GroupBy(p => p.engineType).Select(g =>
new
{
    engineType = g.Key,
    averageHppl = g.Average(p => p.hppl)
});


Console.WriteLine("=== 1b ===");
foreach (var item in averagePpplByType)
{
    Console.WriteLine($"{item.engineType}: {item.averageHppl}");
}


string xml = CarXMLSerializer.Serialize(myCars);


var File = new StreamWriter(FILE);
File.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
File.WriteLine(xml);
File.Close();


XElement rootNode = XElement.Load(FILE);

var avgHP = rootNode.XPathEvaluate("sum(//car/engine/horsePower) div count(//car/engine/horsePower)");

Console.WriteLine("=== 3a ===");
Console.WriteLine(avgHP);

IEnumerable<XElement> models = rootNode.XPathSelectElements("//car[not(model=preceding-sibling::car/model)]/model");

Console.WriteLine("=== 3b ===");
foreach (var item in models)
{
    Console.WriteLine(item.Value);
}

IEnumerable<XElement> nodes = from car in myCars
                              select new XElement("car", new XElement("model", car.model),
                              new XElement("engine",
                              new XElement("displacement", car.engine.displacement),
                              new XElement("horsePower", car.engine.horsePower),
                              new XAttribute("model", car.engine.model)),
                              new XElement("year", car.year));
XElement linqRoot = new XElement("cars", nodes);
linqRoot.Save(LINQ_FILE);


/*
 Korzystając z LINQ to XML wygenerować na podstawie kolekcji myCars dokument
XHTML zawierający tabelę, której wiersze reprezentują kolejne elementy kolekcji (1
pkt). Można ułatwić sobie zadanie poprzez załadowanie pustego dokumentu
XHTML (patrz załącznik nr 2) i dołączenie wygenerowanego elementu table
 
 */

XDocument xhtml = XDocument.Load(EMPTY_HTML);

var table = new XElement("table");
var header = new XElement("tr", new XElement("th", "Model"), new XElement("th", "Engine"),
    new XElement("th", "Year"));

table.Add(header);

foreach (var car in myCars)
{
    var row = new XElement("tr", new XElement("td", car.model),
               new XElement("td", $"{car.engine.model} {car.engine.horsePower}HP"),
                      new XElement("td", car.year));
    table.Add(row);
}

xhtml.Root.Add(table);

xhtml.Save(HTML_FILE);
