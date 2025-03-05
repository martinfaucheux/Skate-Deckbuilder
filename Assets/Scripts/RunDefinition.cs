using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunDefinition", menuName = "RunDefinition", order = 0)]
public class RunDefinition : ScriptableObject
{
    [System.Serializable]
    public class Round
    {
        public int handsPerRound;
        public int cardCountOnBoard;
        public int scoreGoal;
    }

    public List<Round> rounds;
}
