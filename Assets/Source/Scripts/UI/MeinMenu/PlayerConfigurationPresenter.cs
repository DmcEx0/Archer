using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerConfigurationPresenter : MonoBehaviour
{
    [SerializeField] private MeinMenuView _meinMenuView;

    [Space]
    [SerializeField] private PlayerPresenter _playerPresenter;
    [SerializeField] private Transform _rightHand;

    private AnimationController _animationRig;

    private WeaponDataSO _currentWeaponData;
    private ArrowDataSO _currentArrowData;

    private WeaponPresenter _currentWeaponTemplate;
    private ArrowPresenter _currentArrowPresenter;

    private void OnEnable()
    {
        _meinMenuView.WeaponChanged += OnWeaponPresenterChanged;
        _meinMenuView.ArrowChanged += OnArrowPresenterChanged;
    }

    private void OnDisable()
    {
        _meinMenuView.WeaponChanged -= OnWeaponPresenterChanged;
        _meinMenuView.ArrowChanged -= OnArrowPresenterChanged;
    }

    private void Awake()
    {
        _animationRig = _playerPresenter.GetComponent<AnimationController>();
    }

    private void Update()
    {
        if (_currentWeaponTemplate != null)
        {
            _animationRig.SetTargetsForHands(_currentWeaponTemplate.RightHandTarget, _currentWeaponTemplate.LeftHandTarget, _currentWeaponTemplate.ChestTarget);
        }
    }

    private Presenter CreatePresenter(EquipmentDataSO equipmentData, Transform spawnPoint)
    {
        return Instantiate(equipmentData.Presenter, spawnPoint.position, spawnPoint.rotation);
    }

    private void OnWeaponPresenterChanged(WeaponDataSO weaponData)
    {
        if(_currentWeaponTemplate != null)
            Destroy(_currentWeaponTemplate.gameObject);

        _currentWeaponData = weaponData;
        _currentWeaponTemplate = CreatePresenter(_currentWeaponData, _rightHand) as WeaponPresenter;
        _currentWeaponTemplate.transform.parent = _rightHand;
    }

    private void OnArrowPresenterChanged(ArrowDataSO arrowData)
    {
        if(_currentArrowPresenter != null)
            Destroy(_currentArrowPresenter.gameObject);

        _currentArrowData = arrowData;
        _currentArrowPresenter = CreatePresenter(_currentArrowData, _currentWeaponTemplate.ArrowSlot) as ArrowPresenter;
        _currentArrowPresenter.transform.SetParent(_currentWeaponTemplate.ArrowSlot);
    }
}
