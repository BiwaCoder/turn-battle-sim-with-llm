using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameCreateSystem : MonoBehaviour
{
    private string serverUrl = "http://localhost:5001";

    public Character Player { get; private set; }
    public Character Enemy { get; private set; }

    void Start()
    {
        StartCoroutine(InitializeGame());
    }

    // ゲームデータをサーバーから取得する
    private IEnumerator InitializeGame()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{serverUrl}/start"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.Log($"Server Response: {jsonResponse}");

                // JSONからプレイヤーと敵のデータをデシリアライズ
                GameData gameData = JsonUtility.FromJson<GameData>(jsonResponse);
                Player = gameData.Player;
                Enemy = gameData.Enemy;

                Debug.Log($"Player: {Player.Name}, HP: {Player.HP}, Attack: {Player.Attack}");
                Debug.Log($"Enemy: {Enemy.Name}, HP: {Enemy.HP}, Attack: {Enemy.Attack}");

                // 敵を攻撃
                StartCoroutine(GameLoop());
            }
            else
            {
                // エラーが発生した場合
                Debug.LogError($"Error: {www.error}");
            }
        }
    }

    // ゲームループ
    private IEnumerator GameLoop()
    {
        // プレイヤーと敵が生存している間、攻撃を繰り返す
        while (Enemy.HP > 0 && Player.HP > 0)
        {
            yield return new WaitForSeconds(1); // 1秒待つ（攻撃間隔）

            AttackEnemy();
        }

        // 結果の表示
        if (Enemy.HP <= 0)
        {
            Debug.Log("Enemy defeated! You win!");
        }
        else if (Player.HP <= 0)
        {
            Debug.Log("Player defeated! Game over.");
        }
    }

    // 敵を攻撃
    private void AttackEnemy()
    {
        // プレイヤーが生存していて、敵も生存している場合
        if (Player != null && Enemy != null && Player.HP > 0)
        {
            int damage = Player.Attack - Enemy.Defense;
            if (damage < 0) damage = 0; // ダメージが0未満にならないようにする

            Enemy.HP -= damage;

            Debug.Log($"{Player.Name} attacks {Enemy.Name} for {damage} damage!");
            Debug.Log($"{Enemy.Name} HP: {Enemy.HP}");

            if (Enemy.HP <= 0)
            {
                Enemy.HP = 0; // 敵のHPが負にならないように調整
                Debug.Log($"{Enemy.Name} has been defeated!");
            }
        }
    }
}

// サーバーから取得するゲームデータの構造
[System.Serializable]
public class GameData
{
    public Character Player;
    public Character Enemy;
}
