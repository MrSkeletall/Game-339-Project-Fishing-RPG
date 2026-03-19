using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private SpriteRenderer playerSpriteRenderer;
    private SceneTransition _currentZone;

    void Awake()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Move(Vector2 direction)
    {
        Vector2 movement = direction * 5f * Time.deltaTime;
        playerSpriteRenderer.transform.Translate(movement.x, movement.y, 0f);
    }

    public void Interact()
    {
        if (_currentZone != null)
            SceneManager.LoadScene(_currentZone.SceneName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out SceneTransition zone))
            _currentZone = zone;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out SceneTransition zone) && zone == _currentZone)
            _currentZone = null;
    }
}