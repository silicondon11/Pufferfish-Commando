using UnityEngine;

[CreateAssetMenu(fileName = "PentInfo", menuName = "PentInfo", order = 51)]
public class PentInfo : ScriptableObject
{
    public string title;
    public Color color;
    public int[] range = new int[2]; 
    public int damage;
    public int damageRadius;
    public string projectile;
    public int projectileSpeed;
    public float reloadTime;
}
