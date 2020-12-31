using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour, ICollectable
{
    [SerializeField] UnityEvent OnCollect;
    [SerializeField] int scoreAmount = 1;
    bool isCollected = false;

    [SerializeField] AudioSource collectableSource;
    [SerializeField] string collectedSoundName;

    private void Awake()
    {
        if (collectableSource == null && TryGetComponent(out AudioSource component))
            collectableSource = component;
    }

    public void Collect()
    {
        OnCollect?.Invoke();
        ScoreManager.Instance.UpdateCollectedAmount(scoreAmount);
        SoundManager.Instance.PlaySound(collectableSource, collectedSoundName);
    }
    public int GetScoreAmount() => scoreAmount;

    public void Remove()
    {
        Destroy(gameObject);
    }

    public bool GetIsCollected() => isCollected;

    public void SetIsCollected(bool toggle) => isCollected = toggle;

    public void PlayCollectedSound() => SoundManager.Instance.PlaySound(collectableSource, collectedSoundName);
}

public interface ICollectable
{
    void Collect();
    int GetScoreAmount();
    void Remove();
    bool GetIsCollected();
    void SetIsCollected(bool toggle);
}
