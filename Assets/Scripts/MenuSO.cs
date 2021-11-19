using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName="NewMenu", menuName ="Game/Menu")]
public class MenuSO : ScriptableObject
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
        sceneBuildIndex = SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/Menus/{sceneName}.unity");
    }
}
