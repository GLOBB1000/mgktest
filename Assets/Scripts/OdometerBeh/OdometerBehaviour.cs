using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OdometerBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<OdometerCell> numbers;

    private void Start()
    {
        SetUpOdometer(0);
    }

    [Button]
    public void SetUpOdometer(int value)
    {
        numbers[0].SetCell(value % 10);
        numbers[1].SetCell(Mathf.Abs((value % 100) / 10));
        numbers[2].SetCell(Mathf.Abs((value % 1000) / 100));
        numbers[3].SetCell(Mathf.Abs((value % 10000) / 1000));
        numbers[4].SetCell(Mathf.Abs((value % 100000) / 10000));
        numbers[5].SetCell(Mathf.Abs((value % 1000000) / 100000));
        numbers[6].SetCell(Mathf.Abs((value % 10000000) / 1000000));
        numbers[7].SetCell(Mathf.Abs((value % 100000000) / 10000000));
    }
}
