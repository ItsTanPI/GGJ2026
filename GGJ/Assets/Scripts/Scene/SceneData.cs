using UnityEngine;

namespace Scene
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "Mugamoodi/SceneData")]
    public class SceneData : ScriptableObject
    {
        public string sceneName;
        public string displayName;
        public Sprite thumbnail;
    }
}
