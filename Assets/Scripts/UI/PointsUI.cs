using StatePattern.Main;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using StatePattern.Events;
using System;

namespace StatePattern.UI
{
    public class PointsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentPontuationAmount;

        private void Start()
        {
            GameService.Instance.EventService.OnPointGained.AddListener(OnPointsGained);
        }

        private void OnPointsGained(int points)
        {
            currentPontuationAmount.text = points.ToString();   
        }

        private void OnDestroy()
        {
            GameService.Instance.EventService.OnPointGained.RemoveListener(OnPointsGained);
        }
    }
}


