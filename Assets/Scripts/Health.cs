using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Health : MonoBehaviour
{

    public UnityEvent OnDeath;

    [SerializeField] GameObject Healthbar;
    [SerializeField] float OffsetY = 8;
    [SerializeField] float Scale = 0.1f;
    public float MaxHealth = 100;
    public float health = 100;

    GameObject H;

    private void Start()
    {
        H = Instantiate(Healthbar, transform.position + Vector3.up * OffsetY, Quaternion.identity);
    }

    private void Update()
    {
        Transform CamT = Camera.main.transform;
        H.transform.position = transform.position + Vector3.up * OffsetY;
        H.transform.localScale = Scale * Vector3.Distance(CamT.position, H.transform.position) * Vector3.one;
        H.transform.forward = -CamT.forward;
        H.GetComponentInChildren<Slider>().value = health / MaxHealth;
        H.SetActive(health < MaxHealth);
        if (health <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Die()
    {
        Destroy(H);
        Destroy(gameObject);
    }
}
