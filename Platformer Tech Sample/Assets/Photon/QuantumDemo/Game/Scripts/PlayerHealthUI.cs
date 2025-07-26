using Quantum;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Text _healthText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private string _menuSceneName = "Menu";

    private void OnEnable()
    {
        QuantumEvent.Subscribe<EventHealthChanged>(this, OnEventHealthChanged);
        QuantumEvent.Subscribe<EventDeath>(this, OnDeath);
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
        _healthText.text = "HP: " + eventHealthChanged.CurrentHeath.ToString();
    }

    private void OnDeath(EventDeath eventDeath)
    {
        if (QuantumRunner.Default.Game.PlayerIsLocal(eventDeath.Player))
        {
            _gameOverPanel.SetActive(true);
        }
    }

    private void OnDisable()
    {
        QuantumEvent.UnsubscribeListener<EventHealthChanged>(this);
        QuantumEvent.UnsubscribeListener<EventDeath>(this);
    }
}
