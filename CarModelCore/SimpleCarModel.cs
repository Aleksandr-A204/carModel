using CarModelCore.Humans;
using CarModelCore.Parts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarModelCore
{
    // Интерфейсы. Они дают контракт — набор методов, которые должны реализовать классы.
    // Это помогает с полиморфизмом и разделением обязанностей (SOLID: Interface Segregation).

    // Интерфейс для запуска/остановки
    public interface IStartable
    {
        void Start();
        void Stop();
    }

    // Интерфейс для зарядки (электромобили)
    public interface IChargeable
    {
        void Charge(double kWh);
    }

    // Интерфейс для техобслуживания
    public interface IMaintainable
    {
        void PerformMaintenance();
    }

    // Интерфейс для заправки
    public interface IFuelable
    {
        FuelType FuelType { get; }
        void Refuel(double liters);
    }

    public enum FuelType
    {
        Petrol,
        Diesel
    }

    // Тип трансмиссии как enum
    public enum TransmissionType
    {
        Manual,
        Automatic,
        CVT
    }

    // Абстрактный базовый класс Vehicle демонстрирует наследование и инкапсуляцию.
    public abstract class Vehicle : IStartable, IMaintainable
    {
        // readonly уникальный идентификатор автомобиля
        private readonly Guid _id = Guid.NewGuid();

        // Пример private поля с публичным свойством (инкапсуляция)
        private double _speed;
        public double Speed
        {
            get => _speed;
            protected set => _speed = Math.Max(0, value); // защита от отрицательной скорости
        }

        // Объектные свойства разных типов: Engine, Driver, List<Tire>, Transmission
        public Engine Engine { get; private set; }
        public Driver Owner { get; private set; }
        public List<Tire> Tires { get; private set; }
        public TransmissionType Transmission { get; private set; }

        // Статическое и readonly поле
        public static readonly double MaxAllowedSpeed = 300.0; // общая константа для всех автомобилей

        // Конструктор отвечает за создание объекта (SRP)
        protected Vehicle(Engine engine, Driver owner, List<Tire> tires, TransmissionType transmission)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            Tires = tires ?? throw new ArgumentNullException(nameof(tires));
            Transmission = transmission;
            Speed = 0;
        }
        public Guid Id => _id; // публичное свойство только для чтения

        // Методы (минимум 3)
        public virtual void Start()
            => Console.WriteLine($"[{GetType().Name}] Запуск двигателя {Engine} для владельца {Owner}.");

        public virtual void Stop()
        {
            Speed = 0;
            Console.WriteLine($"[{GetType().Name}] Остановка автомобиля.");
        }

        // Ускорение - виртуальный метод, который может быть переопределён (полиморфизм)
        public virtual void Accelerate(double delta)
        {
            if (delta <= 0)
            {
                Console.WriteLine("Значение ускорения должно быть положительным.");
                return;
            }

            Speed = Math.Min(Speed + delta, MaxAllowedSpeed);
            Console.WriteLine($"[{GetType().Name}] Ускорение на {delta} -> текущая скорость {Speed} км/ч.");
        }

        // Метод техобслуживания (реализация IMaintainable)
        // Вынесена базовая логика в защищённый метод CoreMaintenance,
        // чтобы производные классы могли вызывать только общую часть, не добавляя лишних сообщений.
        public virtual void PerformMaintenance()
            => CoreMaintenance();

        protected void CoreMaintenance()
            => Console.WriteLine($"[{GetType().Name}] Базовое техобслуживание: проверка двигателя и давления в шинах.");

        public override string ToString() 
            => $"{GetType().Name} {Id} Владелец:{Owner} Скорость:{Speed} hp:{Engine.HorsePower}";
    }

    // Абстрактный класс для машин с топливом
    public abstract class FuelVehicle : Vehicle, IFuelable
    {
        protected double _fuelLevel;

        public abstract FuelType FuelType { get; }

        protected FuelVehicle(Engine engine, Driver owner, List<Tire> tires, TransmissionType transmission, double initialFuel = 0)
            : base(engine, owner, tires, transmission)
        {
            _fuelLevel = Math.Max(0, initialFuel);
        }

        public virtual void Refuel(double liters)
        {
            if (liters <= 0)
                throw new ArgumentException("liters must be positive", nameof(liters));

            _fuelLevel += liters;
            Console.WriteLine($"[{GetType().Name}] Заправлено {liters} л. Текущий уровень топлива: {_fuelLevel} л.");
        }

        public override void PerformMaintenance()
        {
            CoreMaintenance();
            Console.WriteLine($"[{GetType().Name}] Проверка топливной системы и фильтров.");
        }
    }

    // Абстрактный класс для электромобилей
    public abstract class ElectricVehicle : Vehicle, IChargeable
    {
        protected double _batteryKWh;
        protected readonly double _batteryCapacity;

        protected ElectricVehicle(Engine engine, Driver owner, List<Tire> tires, TransmissionType transmission, double batteryCapacity, double initialCharge = 0)
            : base(engine, owner, tires, transmission)
        {
            _batteryCapacity = Math.Max(0, batteryCapacity);
            _batteryKWh = Math.Max(0, Math.Min(initialCharge, _batteryCapacity));
        }

        public void Charge(double kWh)
        {
            if (kWh <= 0) 
                throw new ArgumentException("kWh must be positive", nameof(kWh));

            _batteryKWh = Math.Min(_batteryKWh + kWh, _batteryCapacity);
            Console.WriteLine($"[{GetType().Name}] Заряжено {kWh} kWh. Текущий заряд: {_batteryKWh}/{_batteryCapacity} kWh.");
        }

        public override void PerformMaintenance()
        {
            CoreMaintenance();
            Console.WriteLine($"[{GetType().Name}] Проверка аккумулятора и электроники.");
        }
    }

    // Класс Car — конкретная реализация легкового автомобиля
    public class Car : FuelVehicle
    {
        public override FuelType FuelType => FuelType.Petrol;

        // Конструктор использует базовый конструктор (DRY)
        public Car(Engine engine, Driver owner, List<Tire> tires, TransmissionType transmission, double initialFuel = 0)
            : base(engine, owner, tires, transmission, initialFuel)
        {
        }

        // Переопределение Start (полиморфизм)
        public override void Start()
        {
            base.Start(); // повторное использование логики из базового класса (DRY)
            Console.WriteLine("[Car] Проверяем ремни безопасности, зеркала.");
        }

        // Car добавляет свою часть техобслуживания (замена масла). Базовую часть берём через CoreMaintenance().
        public override void PerformMaintenance()
        {
            base.PerformMaintenance();
            Console.WriteLine("[Car] Замена масла и фильтров при необходимости.");
        }
    }

    // Электромобиль наследует ElectricCar — пример наследования и полиморфизма
    public class ElectricCar : ElectricVehicle
    {
        public ElectricCar(Engine engine, Driver owner, List<Tire> tires, TransmissionType transmission, double batteryCapacity, double initialCharge = 0)
            : base(engine, owner, tires, transmission, batteryCapacity, initialCharge)
        {
        }

        public override void Start()
            => Console.WriteLine($"[ElectricCar] Тихий запуск электродвигателя. Заряд: {_batteryKWh}/{_batteryCapacity} kWh.");

        // Вызываем только CoreMaintenance (общая часть) и добавляем свою проверку аккумулятора.
        public override void PerformMaintenance()
        {
            base.CoreMaintenance();
            Console.WriteLine("[ElectricCar] Проверка аккумулятора и электроники.");
        }
    }

    // Класс Truck — конкретная реализация грузового автомобиля
    public class Truck : FuelVehicle
    {
        private double _cargoWeight;
        public double CargoWeight
        {
            get => _cargoWeight;
            private set => _cargoWeight = Math.Max(0, value);
        }

        public override FuelType FuelType => FuelType.Diesel;

        // Конструктор использует базовый конструктор (DRY)
        public Truck(Engine engine, Driver owner, List<Tire> tires, TransmissionType transmission, double initialFuel = 0, double cargoWeight = 0)
            : base(engine, owner, tires, transmission, initialFuel)
        {
            _cargoWeight = cargoWeight;
        }

        // Переопределение Start (полиморфизм)
        public override void Start()
        {
            base.Start(); // повторное использование логики из базового класса (DRY)
            Console.WriteLine("[Truck] Проверяем ремни безопасности, зеркала, давление в шинах");
            Console.WriteLine($"[Truck] Вес груза: {CargoWeight} т.");
        }

        // Truck добавляет свою часть техобслуживания (замена масла). Базовую часть берём через CoreMaintenance().
        public override void PerformMaintenance()
        {
            base.PerformMaintenance();
            Console.WriteLine("[Truck] Замена масла и фильтров при необходимости.");
        }
    }

    // Электро грузовый автомобиль наследует Truck — пример наследования и полиморфизма
    public class ElectricTruck : ElectricVehicle
    {
        public double CargoWeight { get; private set; }

        public ElectricTruck(Engine engine, Driver owner, List<Tire> tires, TransmissionType transmission, double batteryCapacity, double initialCharge = 0, double cargoWeight = 0)
            : base(engine, owner, tires, transmission, batteryCapacity, initialCharge)
        {
            CargoWeight = Math.Max(0, cargoWeight);
        }

        public override void Start()
            => Console.WriteLine($"[ElectricTruck] Тихий запуск электродвигателя. Заряд: {_batteryKWh}/{_batteryCapacity} kWh.");

        // Вызываем только CoreMaintenance (общая часть) и добавляем свою проверку аккумулятора.
        public override void PerformMaintenance()
        {
            base.PerformMaintenance();
            Console.WriteLine("[ElectricTruck] Проверка аккумулятора и электроники.");
        }
    }
}
