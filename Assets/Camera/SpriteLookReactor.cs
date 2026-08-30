using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteLookReactor : MonoBehaviour
{
    public List<Sprite> sprites;

    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var pi = FindAnyObjectByType<PlayerInteract>();
        if (!pi) return;
        
        _spriteRenderer.sprite = pi.lastseen == transform.parent.gameObject ? sprites[1] : sprites[0];
    }
}
