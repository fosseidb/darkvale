public enum Allegiance
{
    Destruction,
    Order,
    DestructableObject
}

public interface IDamagable
{
    public void TakeDamage(float damage, PlayerProfile attacker);

    public Allegiance DamagableType { get;}
    float Health { get; set; }
}
