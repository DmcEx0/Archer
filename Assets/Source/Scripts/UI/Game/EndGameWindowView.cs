using IJunior.TypedScenes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameWindowView : MonoBehaviour
{
    [SerializeField] private Button _meinMenuButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private TMP_Text _goldText;

    [SerializeField] private GameObject _victoryImage;
    [SerializeField] private GameObject _defeatImage;

    private void OnEnable()
    {
        _meinMenuButton.onClick.AddListener(LoadMenuScene);
        _nextLevelButton.onClick.AddListener(LoadNextLevel);
    }

    private void OnDisable()
    {
        _meinMenuButton.onClick.RemoveListener(LoadMenuScene);
        _nextLevelButton.onClick.RemoveListener(LoadNextLevel);
    }

    public void SetActiveNextLevelButton(bool isGoToNextLevel)
    {
        _nextLevelButton.gameObject.SetActive(isGoToNextLevel);

        if (isGoToNextLevel)
            _victoryImage.SetActive(true);
        else
            _defeatImage.SetActive(true);
    }

    public void SetAmountCoins(int amountCoins)
    {
        _goldText.text = $"+{amountCoins}";
    }

    private void LoadNextLevel()
    {
        LevelManager.LoadNextLevel();
    }

    private void LoadMenuScene()
    {
        Menu.Load();
    }
}
