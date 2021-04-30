﻿using System.Collections;
using UnityEngine;

public class Eruption : MonoBehaviour
{
    [SerializeField] private float eruptionTime = 2f;
    [SerializeField] private float eruptionDuration = 5f;
    [SerializeField] private new ParticleSystem particleSystem;
    private BoxCollider boxCollider;
    private float timer = 0f;

    private Health playerHP;
    [SerializeField] private float damage = 10f;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void OnEnable()
    {
        particleSystem.Stop();

        StartCoroutine("Erupt");
    }

    void OnDisable()
    {
        boxCollider.enabled = false;
        timer = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
            playerHP.Damage(damage);
    }

    IEnumerator Erupt()
    {
        while (timer < eruptionTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        boxCollider.enabled = true;
        timer = 0f;
        particleSystem.Play();

        StartCoroutine("Erupting");
    }

    IEnumerator Erupting()
    {
        while (timer < eruptionDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        transform.parent.gameObject.SetActive(false);
    }
}