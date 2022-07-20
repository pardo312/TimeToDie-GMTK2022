using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeToDie.DataManagerModule
{
    public class DataManager : MonoBehaviour
    {
        #region ----Singleton----
        public static DataManager instance;
        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
                return;
            }

            Destroy(this);
        }
        #endregion ----Singleton----

        #region ----Data----
        public List<EnemyInitData> enemiesData = new List<EnemyInitData>();
        public int currentLevel = 0;
        #endregion ----Data----

    }

    [System.Serializable]
    public class EnemyInitData
    {
        public int enemyType;
        public int numberOfEnemies;
    }
}
