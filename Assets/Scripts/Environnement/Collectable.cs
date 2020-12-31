using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour, ICollectable
{
    [SerializeField] UnityEvent OnCollect;
    [SerializeField] int scoreAmount = 1;
    bool isCollected = false;
    public void Collect()
    {
        OnCollect?.Invoke();
        ScoreManager.Instance.UpdateCollectedAmount(scoreAmount);
    }
    public int GetScoreAmount() => scoreAmount;

    public void Remove()
    {
        Destroy(gameObject);
    }

    public bool GetIsCollected() => isCollected;

    public void SetIsCollected(bool toggle) => isCollected = toggle;
}

public interface ICollectable
{
    void Collect();
    int GetScoreAmount();
    void Remove();
    bool GetIsCollected();
    void SetIsCollected(bool toggle);
}
