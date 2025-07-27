using Quantum;
using UnityEngine;

public class PlayerHealthUIHandler : MonoBehaviour
{
    [SerializeField] private EntityView _entityView;
    [SerializeField] private PlayerHealthUI _playerHealthUI;

    public void OnEntityInstantiated()
    {
        QuantumGame game = QuantumRunner.Default.Game;
        Frame frame = game.Frames.Verified;

        if (frame.TryGet(_entityView.EntityRef, out PlayerLink playerLink))
        {
            if (game.PlayerIsLocal(playerLink.Player))
            {
                PlayerHealthUI playerHealthUI = Instantiate(_playerHealthUI);
                playerHealthUI.transform.SetParent(_entityView.transform);
                playerHealthUI.SetEntityView(_entityView);
            }
        }
    }
}
