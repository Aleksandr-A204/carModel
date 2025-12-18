using CarModelCore;
using CarModelCore.Garags;
using CarModelCore.Humans;
using CarModelCore.Parts;

var tires = new List<Tire>
            {
                new Tire("Michelin", 2.1),
                new Tire("Michelin", 2.1),
                new Tire("Michelin", 2.1),
                new Tire("Michelin", 2.1),
            };

var car = new Car(new Engine("ВАЗ-11182", 83), new Driver("Александр Пономарев", 3), tires, TransmissionType.Manual, initialFuel: 10);
car.Start();
car.Accelerate(50);
car.Refuel(30);
car.PerformMaintenance();
car.Stop();

Console.WriteLine();

var car2 = new Car(new Engine("Шкода Октавия", 150), new Driver("Александр Шипов", 5), tires, TransmissionType.Automatic, initialFuel: 15);
car2.Start();
car2.Accelerate(80);
car2.Refuel(30);

Console.WriteLine();

var eCar = new ElectricCar(new Engine("E-Motor", 200), new Driver("Александр Александров", 12), tires, TransmissionType.Automatic, batteryCapacity: 85, initialCharge: 40);
eCar.Start();
eCar.Accelerate(60);
eCar.Charge(15);
eCar.PerformMaintenance();

Console.WriteLine();

tires = new List<Tire>
            {
                new Tire("Michelin", 8.0),
                new Tire("Michelin", 8.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0)
            };

var truck = new Truck(new Engine("KAMAZ-5490 ", 300), new Driver("Александр Абдулаев", 20), tires, TransmissionType.Automatic, initialFuel: 100, cargoWeight: 10);
truck.Start();
truck.Accelerate(30);
truck.Refuel(100);
truck.PerformMaintenance();

Console.WriteLine();

tires = new List<Tire>
            {
                new Tire("Michelin", 8.0),
                new Tire("Michelin", 8.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0),
                new Tire("Michelin", 6.0)
            };

var eTruck = new ElectricTruck(new Engine("Actros", 350), new Driver("Александр Бартенев", 12), tires, TransmissionType.Automatic, batteryCapacity: 90, initialCharge: 60);
eTruck.Start();
eTruck.Accelerate(40);
eTruck.Charge(15);

Console.WriteLine();

var garage = new Garage<Vehicle>();
garage.Add(car);
garage.Add(eCar);

Console.WriteLine("Содержимое гаража:");
foreach (var g in garage.GetAll())
    Console.WriteLine(g);

//garage.Remove((Guid)"");

Console.WriteLine();

var garage2 = new Garage<Vehicle>();
garage2.Add(car2);

Console.WriteLine("Содержимое гаража:");
foreach (var g in garage2.GetAll())
    Console.WriteLine(g);

Console.WriteLine();

var garage3 = new Garage<Vehicle>();
garage3.Add(truck);

Console.WriteLine("Содержимое гаража:");
foreach (var g in garage3.GetAll())
    Console.WriteLine(g);

Console.WriteLine();

var garage4 = new Garage<Vehicle>();
garage4.Add(eTruck);

Console.WriteLine("Содержимое гаража:");
foreach (var g in garage4.GetAll())
    Console.WriteLine(g);