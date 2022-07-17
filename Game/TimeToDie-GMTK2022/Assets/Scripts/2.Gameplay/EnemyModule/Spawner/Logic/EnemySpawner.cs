using Jiufen.Audio;
using System.Collections.Generic;
using TimeToDie.DataManagerModule;
using TMPro;
using UnityEngine;

namespace TimeToDie.EnemyModule
{
    public class EnemySpawner : MonoBehaviour
    {
        public List<GameObject> listOfEnemiesPrefabs;
        public List<Transform> spawnPoints;

        //Should not be in this class but fuck it. No time
        public TMP_Text levelLabelText;

        public void Start()
        {
            levelLabelText.text = DataManager.instance?.currentLevel.ToString();
            SpawnEnemies();
        }

        public void SpawnEnemies()
        {
            DataManager.instance.enemiesData.ForEach(enemyData =>
            {
                for (int i = 0; i < enemyData.numberOfEnemies; i++)
                {
                    GameObject enemy = Instantiate(listOfEnemiesPrefabs[enemyData.enemyType],
                                                   spawnPoints[Random.Range(0, spawnPoints.Count)].position,
                                                   Quaternion.identity);

                    //enemy.transform.position += new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));
                }
            });
        }

    }
}
