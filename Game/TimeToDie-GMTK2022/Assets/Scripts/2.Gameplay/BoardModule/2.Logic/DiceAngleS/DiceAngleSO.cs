using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeToDie
{
    [CreateAssetMenu(fileName = "DiceAngles", menuName = "TiemToDie/Board/DiceAngles", order = 1)]
    public class DiceAngleSO : ScriptableObject
    {
        public List<float> diceAngles = new List<float>();
    }
}