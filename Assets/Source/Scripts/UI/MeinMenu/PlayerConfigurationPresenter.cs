using UnityEngine;

public class PlayerConfigurationPresenter : MonoBehaviour
{
    [SerializeField] private MeinMenuView _meinMenuView;

    [Space]
    [SerializeField] private PlayerPresenter _playerPresenter;

    private WeaponDataSO _currentWeaponData;
    private ArrowDataSO _currentArrowData;

    private WeaponPresenter _currentWeaponPresenter;
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

    private Presenter CreatePresenter(EquipmentDataSO equipmentData, Transform spawnPoint)
    {
        return Instantiate(equipmentData.Presenter, spawnPoint.position, spawnPoint.rotation);
    }

    private void OnWeaponPresenterChanged(WeaponDataSO weaponData)
    {
        if(_currentWeaponPresenter != null)
            Destroy(_currentWeaponPresenter.gameObject);

        _currentWeaponData = weaponData;
        _currentWeaponPresenter = CreatePresenter(_currentWeaponData, _playerPresenter.SpawnPoint) as WeaponPresenter;
    }

    private void OnArrowPresenterChanged(ArrowDataSO arrowData)
    {
        if(_currentArrowPresenter != null)
            Destroy(_currentArrowPresenter.gameObject);

        _currentArrowData = arrowData;
        _currentArrowPresenter = CreatePresenter(_currentArrowData, _currentWeaponPresenter.SpawnPoint) as ArrowPresenter;
    }
}
