using UnityEngine;
using System.Collections;

public class ManagerStore{

    /// <summary>
    /// 游戏管理器对象
    /// </summary>
    private static GameObject _manager = null;
    public static GameObject manager
    {
        get
        {
            if (_manager == null)
                _manager = GameObject.FindWithTag("GameManager");
            return _manager;
        }
    }

    /// <summary>
    /// 游戏管理器
    /// </summary>
    private static GameManager _gameManager = null;
    public static GameManager gameManager
    {
        get
        {
            if (_gameManager == null)
                _gameManager = manager.GetComponent<GameManager>();
            return _gameManager;
        }
    }

    /// <summary>
    /// UI管理器
    /// </summary>
    private static UIManager _panelManager = null;
    public static UIManager panelManager
    {
        get
        {
            if (_panelManager == null)
                _panelManager = manager.GetComponent<UIManager>();
            return _panelManager;
        }
    }

    /// <summary>
    /// 资源管理器
    /// </summary>
    private static AssetBundleManager _resourceManager = null;
    public static AssetBundleManager resourceManager
    {
        get
        {
            if (_resourceManager == null)
                _resourceManager = manager.GetComponent<AssetBundleManager>();
            return _resourceManager;
        }
    }

    /// <summary>
    /// 计时器管理器
    /// </summary>
    private static TimerManager _timerManager = null;
    public static TimerManager timerManager
    {
        get
        {
            if (_timerManager == null)
                _timerManager = manager.GetComponent<TimerManager>();
            return _timerManager;
        }
    }

    /// 声音管理器
    /// </summary>
    private static SoundManager _soundManager = null;
    public static SoundManager soundManager
    {
        get
        {
            if (_soundManager == null)
                _soundManager = manager.GetComponent<SoundManager>();
            return _soundManager;
        }
    }

    /// <summary>
    /// 网络管理器
    /// </summary>
    private static NetworkManager _networkManager = null;
    public static NetworkManager networkManager
    {
        get
        {
            if (_networkManager == null)
                _networkManager = manager.GetComponent<NetworkManager>();
            return _networkManager;
        }
    }
}
