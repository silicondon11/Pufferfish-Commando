using UnityEngine;
using System.Collections.Generic;

public class DataMove : MonoBehaviour
{
    public static DataMove Instance;

    public ShellType CurrentShellType;
    public bool ShellFlag;
    public List<string> CurrentPentTypes;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
