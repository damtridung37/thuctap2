using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum StatType
{
    None = 0,
    Speed = 1,
    Health = 2,
    Armor = 3,
    Damage = 4,
    AttackSpeed = 5,
    CritChance = 6,
    CritDamage = 7,
    HealthRegen = 8
}
