using Jiufen.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TimeToDie
{
    public class MusicPartsRandomizer : MonoBehaviour
    {
        private List<GameplayMusicExtraEnum> currentPlayExtra = new List<GameplayMusicExtraEnum>();
        public bool playMusic = true;
        [Range(0,40)]public int minTimeBetweenExtras, maxTimeBetweenExtras;
        public void Start()
        {
            AudioManager.PlayAudio(nameof(GameplayMusicBaseEnum.GAMEPLAY_BASE_) + UnityEngine.Random.Range(1, 4), new AudioJobOptions()
            {
                loop = true
            });

            StartCoroutine(PlayNewMusicLayer());
        }

        public IEnumerator PlayNewMusicLayer()
        {
            while (playMusic)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeBetweenExtras, maxTimeBetweenExtras));

                int numberOfEnum = 0;
                GameplayMusicExtraEnum finalEnum;
                if (currentPlayExtra.Exists((x) => nameof(x).Contains("GAMEPLAY_MELODY") || nameof(x).Contains("GAMEPLAY_ORNAMENT"))
                    && !currentPlayExtra.Contains(GameplayMusicExtraEnum.GAMEPLAY_DRUM_1))
                    finalEnum = GameplayMusicExtraEnum.GAMEPLAY_DRUM_1;
                else
                {
                    numberOfEnum = UnityEngine.Random.Range(0, Enum.GetNames(typeof(GameplayMusicExtraEnum)).Length);
                    finalEnum = (GameplayMusicExtraEnum)numberOfEnum;
                }
                Debug.Log($"Playing: {finalEnum.ToString()}");
                currentPlayExtra.Add(finalEnum);
                AudioManager.PlayAudio(finalEnum.ToString());
                yield return new WaitForSeconds(8);
                Debug.Log($"Stopping: {finalEnum.ToString()}");
                AudioManager.StopAudio(finalEnum.ToString());
                currentPlayExtra.Remove(finalEnum);
            }
        }
    }
}
