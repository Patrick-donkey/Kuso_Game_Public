using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    [SerializeField]
    bool battleArea;
    public static bool isbattle;
    public static bool freeze;

    private void Awake()
    {
        isbattle = battleArea;
    }
}
