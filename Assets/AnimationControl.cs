using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private Vector3 lastPosition;
    private float moveSpeed = 0f;
    private Animator animator;

    void Start() {
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update() {
        moveSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        animator.SetFloat("moveSpeed", moveSpeed);
    }
}
