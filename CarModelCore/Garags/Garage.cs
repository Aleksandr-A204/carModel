using System;
using System.Collections.Generic;
using System.Text;

namespace CarModelCore.Garags
{
    // Пример применения шаблона (generic) — гараж для любого типа Vehicle
    public class Garage<TVehicle> where TVehicle : Vehicle
    {
        private readonly Guid _id = Guid.NewGuid();

        private readonly Dictionary<Guid, TVehicle> _storage = new Dictionary<Guid, TVehicle>();

        private Guid Id { get { return _id; } }
        public void Add(TVehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            _storage[vehicle.Id] = vehicle;
            Console.WriteLine($"[{GetType().Name}] с ID {Id}. Добавлен {vehicle.GetType().Name} с Id={vehicle.Id}");
        }

        public TVehicle? Get(Guid id)
            => _storage.TryGetValue(id, out var v) ? v : null;

        public bool Remove(Guid id)
            => _storage.Remove(id);

        public IEnumerable<TVehicle> GetAll() => _storage.Values;
    }
}
