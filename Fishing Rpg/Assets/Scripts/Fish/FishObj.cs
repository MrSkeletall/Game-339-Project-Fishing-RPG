using UnityEngine;

[CreateAssetMenu(fileName = "FishObj", menuName = "Scriptable Objects/FishObj")]
public class FishObj : ScriptableObject
{
    public string fishName;
    public float health;
    public float maxHealth;
    public float speed;
    public float strength;
    public float luck;

}
