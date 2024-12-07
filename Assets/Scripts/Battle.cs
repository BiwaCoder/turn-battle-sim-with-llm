using System;
using System.Collections.Generic;
using System.Linq;

public class Battle
{
    private List<Character> characters;

    public Battle(List<Character> characters)
    {
        this.characters = characters;
    }

    // 素早さと乱数で順番を決める
    public List<Character> DetermineTurnOrder()
    {
        Random random = new Random();
        return characters
            .OrderByDescending(c => c.Speed + random.Next(0, 10)) // 素早さに小さな乱数を加える
            .ToList();
    }

    // ダメージ計算
    public int CalculateDamage(Character attacker, Character defender)
    {
        Random random = new Random();
        int baseDamage = attacker.Attack - defender.Defense;
        int randomModifier = random.Next(0, 5); // ダメージに小さな乱数を追加
        return Math.Max(baseDamage + randomModifier, 0); // ダメージが0未満にならないようにする
    }

    // バトル1ターンの実行
    public void ExecuteTurn(Character attacker, Character defender)
    {
        if (!attacker.IsAlive() || !defender.IsAlive()) return;

        int damage = CalculateDamage(attacker, defender);
        defender.TakeDamage(damage);
        Console.WriteLine($"{attacker.Name} attacks {defender.Name} for {damage} damage!");

        if (!defender.IsAlive())
        {
            Console.WriteLine($"{defender.Name} has been defeated!");
        }
    }
}
