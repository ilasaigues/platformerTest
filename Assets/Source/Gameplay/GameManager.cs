using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private ScriptableTimeContext _playerTimeContext;

    [SerializeField]
    private ScriptableInt _scoreCount;

    // Start is called before the first frame update
    void Start()
    {
        _playerTimeContext.Value = 1;
        _scoreCount.Value = 0;
    }


}
