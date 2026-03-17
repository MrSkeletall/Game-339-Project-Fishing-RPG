using System;

namespace Game339.Shared.Models
{
    public class Character
    {
        public ObservableValue<string> Name { get; } = new();
        public ObservableValue<int> Health { get; } = new();
        public ObservableValue<int> Damage { get; } = new();
        public ObservableValue<int> Armor { get; } = new();
    }
}