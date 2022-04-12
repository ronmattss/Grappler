using Cinemachine;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Levels", order = 0)]
    public class LevelData : ScriptableObject
    {
        public GameObject levelPrefab;
        public bool followCamera = false;

        // level Details???

    }
}