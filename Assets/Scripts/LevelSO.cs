using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level")]
public class LevelSO : ScriptableObject
{
    [SerializeField] private string sceneName;
    public string SceneName { get => sceneName; }

    [SerializeField] private int sceneBuildIndex;
    public int SceneBuildIndex { get => sceneBuildIndex; }

    #if UNITY_EDITOR
    private void Reset()
    {
        UpdateSceneParams();    
    }
    #endif

    [ContextMenu("Update Scene Params")]
    public void UpdateSceneParams()
    {
        sceneName = this.name;
        sceneBuildIndex = SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/Levels/{sceneName}.unity");
    }
}
