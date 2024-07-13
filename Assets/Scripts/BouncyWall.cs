using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyWall : MonoBehaviour
{
    [SerializeField]
    private AudioClip _bounceSound;
    [SerializeField]
    private bool _bounceOnBonk = true;    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            Debug.Log("Bounce!");
            SoundManager.Instance.PlaySound(_bounceSound);
            Vector3 forceVector = playerController.GetForceVector();
            playerController.SetForceVector(Vector3.Reflect(forceVector, transform.up));
            float forceMagnitude = playerController.GetForceMagnitude();
            float newForce = (!_bounceOnBonk ? GameUtils.Instance.BounceFactor * 0.75f : (forceMagnitude > 0f ? forceMagnitude : GameUtils.Instance.BounceFactor));
            playerController.SetForceMagnitude(newForce);
        }
    }
}
