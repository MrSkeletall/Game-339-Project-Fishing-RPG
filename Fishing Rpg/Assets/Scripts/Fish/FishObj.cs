using UnityEngine;

[CreateAssetMenu(fileName = "FishObj", menuName = "Scriptable Objects/FishObj")]
public class FishObj : ScriptableObject
{
    public string fishName;
    public int fishPrice;
    public float health;
    public float maxHealth;
    public float speed;
    public float strength;
    public float luck;
    public float fishSize = 1f;
    
    
    public Sprite fishSprite;

    public void setFishSize(Transform fish)
    {
        fish.localScale = new Vector3(fishSize, fishSize, fishSize);
    }
    

}
