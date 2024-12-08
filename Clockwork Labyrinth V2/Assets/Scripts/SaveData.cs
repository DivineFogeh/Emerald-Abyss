using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SaveData
{
    public static SaveData Instance;

    //map stuff
    public HashSet<string> sceneNames;

    //bench stuff
    public string benchSceneName;
    public Vector2 benchPos;

    //player stuff
    public int playerHealth;
    public float playerMana;
    public Vector2 playerPosition;
    public string lastScene;

    public bool THKDefeated;


    public void Initialize()
    {
        if (!File.Exists(Application.persistentDataPath + "/save.bench.data")) //if file doesn't exist, create the file
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.bench.data"));
        }
        if (!File.Exists(Application.persistentDataPath + "/save.player.data")) //if file doesn't exist, create the file
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.player.data"));
        }

        if (sceneNames == null)
        {
            sceneNames = new HashSet<string>();
        }
    }

    #region Bench Stuff
    public void SaveBench()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.bench.data")))
        {
            writer.Write(benchSceneName);
            writer.Write(benchPos.x);
            writer.Write(benchPos.y);
        }
    }

    public void LoadBench()
    {
        string savePath = Application.persistentDataPath + "/save.bench.data";
        if (File.Exists(savePath) && new FileInfo(savePath).Length > 0)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.bench.data")))
            {
                benchSceneName = reader.ReadString();
                benchPos.x = reader.ReadSingle();
                benchPos.y = reader.ReadSingle();
            }
        }
        else
        {
            Debug.Log("Bench doesn't exist");
        }
    }
    #endregion

    #region Player stuff
    public void SavePlayerData()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.player.data")))
        {
            playerHealth = PlayerController.Instance.Health;
            writer.Write(playerHealth);
          

            playerMana = PlayerController.Instance.Mana;
            writer.Write(playerMana);
            
        

            playerPosition = PlayerController.Instance.transform.position;
            writer.Write(playerPosition.x);
            writer.Write(playerPosition.y);

            lastScene = SceneManager.GetActiveScene().name;
            writer.Write(lastScene);
        }
        Debug.Log("Saved player data");
    }

    public void LoadPlayerData()
    {
        string savePath = Application.persistentDataPath + "/save.player.data";
        if (File.Exists(savePath) && new FileInfo(savePath).Length > 0)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.player.data")))
            {
                playerHealth = reader.ReadInt32();
                playerMana = reader.ReadSingle();

                playerPosition.x = reader.ReadSingle();
                playerPosition.y = reader.ReadSingle();

                lastScene = reader.ReadString();

                SceneManager.LoadScene(lastScene);
                PlayerController.Instance.transform.position = playerPosition;
                PlayerController.Instance.Health = playerHealth;
                PlayerController.Instance.Mana = playerMana;
                
            
            }
            Debug.Log("Loaded player data");
        }
        else
        {
            Debug.Log("File doesn't exist");
            PlayerController.Instance.Health = PlayerController.Instance.maxHealth;
            PlayerController.Instance.Mana = 0.5f;

          
        }
    }
    #endregion

    public void SaveBossData()
    {
        if (!File.Exists(Application.persistentDataPath + "/save.boss.data")) //if file doesnt exist, well create the file
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.boss.data"));
        }

        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.boss.data")))
        {
            THKDefeated = GameManager.Instance.THKDefeated;
            writer.Write(THKDefeated);
        }
        
    }

    public void LoadBossData()
    {
        if (File.Exists(Application.persistentDataPath + "/save.boss.data"))
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.boss.data")))
            {
                THKDefeated = reader.ReadBoolean();

                GameManager.Instance.THKDefeated = THKDefeated;
            }
        }
        else
        {
            Debug.Log("Boss doesnt exist");
        }
    }
}
