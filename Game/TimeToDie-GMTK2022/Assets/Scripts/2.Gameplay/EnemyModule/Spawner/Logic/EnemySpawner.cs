using Jiufen.Audio;
using System.Collections;
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
            if (!(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "boss"))
            {
                StartCoroutine(SpawnEnemies());
            }
        }

        public void SpawnHandler()
        {
            GameObject enemy = Instantiate(listOfEnemiesPrefabs[Random.Range(0,5)],
                                                   spawnPoints[Random.Range(0, spawnPoints.Count)].position,
                                                   Quaternion.identity);
        }

        public IEnumerator SpawnEnemies()
        {
            foreach (var enemyData in DataManager.instance.enemiesData)
            {
                for (int i = 0; i < enemyData.numberOfEnemies; i++)
                {
                    GameObject enemy = Instantiate(listOfEnemiesPrefabs[enemyData.enemyType],
                                                   spawnPoints[Random.Range(0, spawnPoints.Count)].position,
                                                   Quaternion.identity);

                    if (i % 4 == 0)
                        yield return new WaitForSeconds(Random.Range(30, 60));
                }
            }
        }

    }
}
