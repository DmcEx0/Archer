using Archer.Data;
using Archer.Utils;
using Archer.Yandex;
using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.Tutor
{
    public class TutorialView : MonoBehaviour, ITimeControllable
    {
        private const int MinCurrentNumber = 0;

        private const string TranslationName = "Tutorial1";

        private const string RuWordForPhone = "ЭКРАН";
        private const string RuWordForPC = "ЛКМ";

        private const string EngWordForPhone = "SCREEN";
        private const string EngWordForPC = "Left-Click";

        private const string TurWordForPhone = "EKRAN";
        private const string TurWordForPC = "Sol fare düğmesi";

        [SerializeField] private TimeScaleSetter _timeScaleSetter;

        [Space] [SerializeField] private Button _nextTutorialButton;
        [SerializeField] private Button _previousTutorialButton;
        [SerializeField] private Button _readyButton;
        [SerializeField] private Button _settingButton;

        [SerializeField] private Image[] _tutorials;

        private Image _currentTutorial;
        private int _currentImageNumber = 0;

        private void OnEnable()
        {
            _nextTutorialButton.onClick.AddListener(OnShowNextTutorial);
            _previousTutorialButton.onClick.AddListener(OnShowPreviousTutorial);
            _readyButton.onClick.AddListener(OnCloseWindow);

            LeanLocalization.OnLocalizationChanged += OnChangeWord;
        }

        private void OnDisable()
        {
            _nextTutorialButton.onClick.RemoveListener(OnShowNextTutorial);
            _previousTutorialButton.onClick.RemoveListener(OnShowPreviousTutorial);
            _readyButton.onClick.RemoveListener(OnCloseWindow);

            LeanLocalization.OnLocalizationChanged -= OnChangeWord;
        }

        private void Awake()
        {
            SetTimeScale(true);
        }

        private void Start()
        {
            OnChangeWord();

            _readyButton.gameObject.SetActive(false);
            _settingButton.gameObject.SetActive(false);

            foreach (var tutorial in _tutorials)
            {
                tutorial.gameObject.SetActive(false);
            }

            _currentTutorial = _tutorials[_currentImageNumber];
            _currentTutorial.gameObject.SetActive(true);

            ActivateButton(_currentImageNumber);
        }

        private void OnChangeWord()
        {
            string output;

            LeanLocalization.SetCurrentLanguageAll(PlayerData.Instance.CurrentLanguage);

            output = string.Format(LeanLocalization.GetTranslationText(TranslationName),
                IdentifyWord(Application.isMobilePlatform));

            LeanLocalization.GetTranslation(TranslationName).Data = output;
        }

        private string IdentifyWord(bool isMobilePlatform)
        {
            string word;

            if (isMobilePlatform)
            {
                word = PlayerData.Instance.CurrentLanguage switch
                {
                    Language.ENG => EngWordForPhone,
                    Language.RUS => RuWordForPhone,
                    Language.TUR => TurWordForPhone,
                    _ => EngWordForPhone
                };

                return word;
            }
            else
            {
                word = PlayerData.Instance.CurrentLanguage switch
                {
                    Language.ENG => EngWordForPC,
                    Language.RUS => RuWordForPC,
                    Language.TUR => TurWordForPC,
                    _ => EngWordForPhone
                };

                return word;
            }
        }

        private void OnCloseWindow()
        {
            SetTimeScale(false);
            gameObject.SetActive(false);
            _settingButton.gameObject.SetActive(true);
        }

        private void OnShowNextTutorial()
        {
            int positiveIndex = 1;
            EnableTutorial(positiveIndex);
        }

        private void OnShowPreviousTutorial()
        {
            int negativeIndex = -1;
            EnableTutorial(negativeIndex);
        }

        private void EnableTutorial(int index)
        {
            _currentImageNumber = Mathf.Clamp(_currentImageNumber + index, MinCurrentNumber, _tutorials.Length - 1);

            ActivateButton(_currentImageNumber);

            _currentTutorial.gameObject.SetActive(false);
            _currentTutorial = _tutorials[_currentImageNumber];
            _currentTutorial.gameObject.SetActive(true);
        }

        private void ActivateButton(int currentNumber)
        {
            _readyButton.gameObject.SetActive(false);
            _previousTutorialButton.gameObject.SetActive(true);
            _nextTutorialButton.gameObject.SetActive(true);

            if (currentNumber == MinCurrentNumber)
            {
                _previousTutorialButton.gameObject.SetActive(false);
            }

            if (currentNumber == _tutorials.Length - 1)
            {
                _nextTutorialButton.gameObject.SetActive(false);
                _readyButton.gameObject.SetActive(true);
            }
        }

        private void SetTimeScale(bool isPause)
        {
            _timeScaleSetter.SetGamePause(isPause, this);
        }
    }
}