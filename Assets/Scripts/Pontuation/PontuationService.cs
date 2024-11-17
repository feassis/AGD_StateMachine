using StatePattern.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontuationService : MonoBehaviour
{
    public int Score { get; private set; }

    public void AddPoints(int points)
    {
        Score += points;
        GameService.Instance.EventService.OnPointGained.InvokeEvent(Score);
        GameService.Instance.SoundService.PlaySoundEffects(StatePattern.Sound.SoundType.COIN_PICK_UP);
    }
}
