using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//收集物品
public class CollectableItem : MonoBehaviour
{
    public int itemID;
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        GameManager.Instance.progressManager.AddItem();
        gameObject.SetActive(false);

        // 可以添加收集效果
    }
}
