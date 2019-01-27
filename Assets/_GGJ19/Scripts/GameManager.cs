using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameObject levelPrefab;
    public LevelManager levelManager {
        get { return LevelManager.Instance; }
    }
    public ResourceManager resourceManager {
        get { return ResourceManager.Instance; }
    }
    private void Start() {
        Initialize();
    }
    ///////////////////////////////////////////////////////
    public void Initialize() {
        if (levelManager == null && levelPrefab != null) Instantiate(levelPrefab);
        levelManager.Initialize();
        if (resourceManager == null) new GameObject("Resource Manager").AddComponent<ResourceManager>();
        resourceManager.Initialize();
        CreateAmbiance();
    }
    public void Cleanup() {
        // When this breaks, instead of hoping cleanup works perfectly, just destroy and re-initialize
        if (levelManager != null) levelManager.Cleanup();
        if (resourceManager != null) resourceManager.Cleanup();
    }
    ///////////////////////////////////////////////////////
    private void CreateAmbiance() {
        Instantiate(Resources.Load<GameObject>("AudioAmbiance"));
    }
}
