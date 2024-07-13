using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField]
    private Image cooldownImage;
    [SerializeField]
    private GameObject powerUpModel;
    [SerializeField]
    private float spawnRate;
    [SerializeField]
    private float StrengthIncrease;
    [SerializeField]
    private float RadiusIncrease;
    [SerializeField]
    private AudioClip _powerUpSound;
    private float currentCooldown;
    private bool powerUpSpawned;
    // Start is called before the first frame update
    void Start()
    {
        
        currentCooldown = spawnRate;
        powerUpModel.SetActive(false);
        powerUpSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCooldown <= 0)
        {
            currentCooldown = spawnRate;
            powerUpModel.SetActive(true);
            powerUpSpawned = true;
        }

        if (!powerUpSpawned) 
        { 
            currentCooldown -= Time.deltaTime;
            cooldownImage.fillAmount = currentCooldown / spawnRate;
        } else
        {
            powerUpModel.transform.Rotate(Vector3.up, 180 * Time.deltaTime, Space.World);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (powerUpSpawned)
        {
            Debug.Log("picked up power-up");
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.SetAttackRadius(player.GetAttackRadius() * RadiusIncrease);
                player.SetAttackStrength(player.GetAttackStrength() * StrengthIncrease);
                player._canvas.transform.localScale *= RadiusIncrease;
                player.EnlargeWeapon(RadiusIncrease);
                SoundManager.Instance.PlaySound(_powerUpSound);
            }

            currentCooldown = spawnRate;
            powerUpModel.SetActive(false);
            powerUpSpawned = false;
        }
    }
}
