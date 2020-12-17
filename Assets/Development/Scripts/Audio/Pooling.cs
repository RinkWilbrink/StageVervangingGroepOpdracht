using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Pooling : MonoBehaviour
{
    private Transform _AudioObjectPool;
    public GameObject _PooledObject;
    public int _PoolSize;
    List<PoolItem> _Pool;

    void Start()
    {
        _AudioObjectPool = gameObject.transform;
        _Pool = new List<PoolItem>();

        GameObject temporary;

        for (int i = 0; i < _PoolSize; i++)
        {
            temporary = Instantiate(_PooledObject);
            AddItem(temporary.GetComponent<PoolItem>());
            _Pool[i].Pool = this;
        }
    }

    public void AddItem(PoolItem item)
    {
        _Pool.Add(item);
        item.transform.parent = _AudioObjectPool;
        item.gameObject.SetActive(false);
    }

    public GameObject InstantiateItem(AudioClip audioClip, AudioMixerGroup audioMixerGroup)
    {
        if (_Pool.Count <= 0)
        {
            return null;
        }

        _Pool[0].Initiate();
        GameObject item = _Pool[0].gameObject;
        item.GetComponent<AudioObject>().PlayAudio(audioClip, audioMixerGroup);
        _Pool.RemoveAt(0);
        return item;
    }
}
