using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GamePersistingData : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

  public void Save() {
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(Application.persistentDataPath + "gamerInfo.dat");

    GameData data = new GameData();
    data.currentLevel = GameManager.instance.Level;
    data.nbLevelUnlocked = 0; //GameManager.instance.nbLevelUnlocked

    bf.Serialize(file, data);
    file.Close();
  }

  public void Load() {
    if(File.Exists(Application.persistentDataPath + "gamerInfo.dat")) {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Open(Application.persistentDataPath + "gamerInfo.dat", FileMode.Open);

      GameData data = (GameData) bf.Deserialize(file);
      file.Close();

      GameManager.instance.Level = data.currentLevel;
      // GameManger.instance.NbLevelUnlocked = data.nbLevelUnlocked;
    }
  }
}

[Serializable]
class GameData {
  public int currentLevel;
  public int nbLevelUnlocked;
}
