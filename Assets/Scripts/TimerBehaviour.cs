using System;

public class TimerBehaviour
{
    public float RemainingTime { get; private set; }
    public bool IsStopped { get; private set; }
    public event Action OnTimerEnd;

    private readonly float _originTime;

    public TimerBehaviour(float duration)
    {
        _originTime = duration;

        IsStopped = true;
    }

    public void RestartTimer()
    {
        RemainingTime = _originTime;

        IsStopped = false;
    }

    public void AdjustTime(float time)
    {
        RemainingTime += time;
    }

    public void StopTimer()
    {
        IsStopped = true;
    }

    public void UpdateTime(float time)
    {
        if (IsStopped == true)
            return;

        RemainingTime -= time;

        if (RemainingTime <= 0)
        {
            OnTimerEnd?.Invoke();
            IsStopped = true;
        }
    }
}
