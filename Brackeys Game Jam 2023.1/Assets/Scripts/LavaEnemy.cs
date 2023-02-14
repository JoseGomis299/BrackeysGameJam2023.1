using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LavaEnemy : MonoBehaviour, Iinteractable
{
    private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private LayerMask layerMask;
    private GameObject _canvas;
    [SerializeField] private GameObject gem;
    public bool throweable { get; private set; }
    [SerializeField] private float radius = 3;
    private bool _thrown;
    private Transform player;

    private RaycastHit2D rightHit;
    private RaycastHit2D leftHit;
    private RaycastHit2D upHit;
    private RaycastHit2D downHit;

    private Vector3 direction;
    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _canvas = transform.GetChild(1).gameObject;
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (_thrown)
        {
            _canvas.SetActive(false);
            throweable = false;
            transform.localScale -= Vector3.one*Time.deltaTime;
            if(transform.localScale.x <= 0) gameObject.SetActive(false);
            return;
        }
        if (Vector3.Distance(player.position, transform.position) > radius)
        {
            _canvas.SetActive(false);
            throweable = false;
            return;
        }
        
        direction = Vector3.zero;
        rightHit = Physics2D.Raycast(transform.position, Vector2.right, _capsuleCollider.size.x / 2 + 1f, layerMask);
        if(rightHit) direction = Vector3.right;
        leftHit = Physics2D.Raycast(transform.position, Vector2.left, _capsuleCollider.size.x / 2 + 1f, layerMask);
        if(leftHit) direction = Vector3.left;
        upHit = Physics2D.Raycast(transform.position, Vector2.up, _capsuleCollider.size.x / 2 + 1f, layerMask);
        if(upHit) direction = Vector3.up;
        downHit = Physics2D.Raycast(transform.position, Vector2.down, _capsuleCollider.size.x / 2 + 1f, layerMask);
        if(downHit) direction = Vector3.down;

        if (direction != Vector3.zero) 
        {
            _canvas.SetActive(true);
            throweable = true;
        }
    }

    public void Interact()
    {
        if (throweable)
        {
            BeThrown();
        }
    }

    private void BeThrown()
    {
        if (gem != null)
        {
            gem.transform.position = transform.position;
            gem.SetActive(true);
        }
        _canvas.SetActive(false);
        throweable = false;
        _thrown = true;
        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg);
        transform.position += direction * 1.5f;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Lava"))
        {
            Debug.Log("VAR");
        }
    }
}
