using Quantum;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Text _healthText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private string _menuSceneName = "Menu";
    [SerializeField] private EntityView _entityView;

    private void Start()
    {
        QuantumEvent.Subscribe<EventHealthChanged>(this, OnEventHealthChanged);
        QuantumEvent.Subscribe<EventDeath>(this, OnDeath);
    }

    public void SetEntityView(EntityView entityView)
    {
        _entityView = entityView;
        if (QuantumRunner.Default.Game.Frames.Verified.TryGet(_entityView.EntityRef, out Health health))
        {
            SetHealthText(health.CurrentHealth);
        }
    }

    public void RestartLevel()
    {
        QuantumRunner.ShutdownAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenMenuScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_menuSceneName);
    }

    private void OnEventHealthChanged(EventHealthChanged eventHealthChanged)
    {
        if (eventHealthChanged.Entity == _entityView.EntityRef)
        {
            SetHealthText(eventHealthChanged.CurrentHeath);
        }
    }

    private void SetHealthText(int currentHealth)
    {
        _healthText.text = "HP: " + currentHealth.ToString();
    }

    private void OnDeath(EventDeath eventDeath)
    {
        if (QuantumRunner.Default.Game.PlayerIsLocal(eventDeath.Player))
        {
            _gameOverPanel.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        QuantumEvent.UnsubscribeListener<EventHealthChanged>(this);
        QuantumEvent.UnsubscribeListener<EventDeath>(this);
    }
}
