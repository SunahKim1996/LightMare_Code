using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected virtual void Use() 
    {
        SoundManager.instance.PlaySFX(SoundClip.PotionSFX, 0.1f);
        Destroy(gameObject);
    }
}
