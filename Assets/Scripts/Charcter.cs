using UnityEngine;

[System.Serializable]
public class Character
{
    public string Name;
    public int HP;
    public int Attack;
    public int Defense;
    public int Speed;

    public bool IsAlive()
    {
        return HP > 0;
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP < 0) HP = 0;
    }
}
