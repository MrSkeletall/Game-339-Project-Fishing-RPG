using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    public string SceneName => sceneToLoad;
}