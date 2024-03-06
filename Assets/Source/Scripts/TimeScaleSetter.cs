using UnityEngine;

public class TimeScaleSetter : MonoBehaviour
{
    private bool _isPause;

    private ITimeControllable _currentController = null;

    public void SetGamePause(bool isPause, ITimeControllable controller)
    {
        if (_currentController == null)
            _currentController = controller;

        if (controller != _currentController)
            return;

        Time.timeScale = isPause ? 0 : 1;

        if (_isPause == true && isPause == false)
            _currentController = null;

        _isPause = isPause;
    }
}