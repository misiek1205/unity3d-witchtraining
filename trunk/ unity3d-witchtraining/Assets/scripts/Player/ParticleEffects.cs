using UnityEngine;
using System.Collections;

public class ParticleEffects : MonoBehaviour
{
    public GameObject playerRunParticles;
    private ThirdPersonFlyingController flyingController;


    private GameObject playerGO;

    private bool isflyingFast = false;
    private GameObject fastPlayerParticleSystem;
    private bool fadeOutPlayerParticles = false;

    // Use this for initialization
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        flyingController = playerGO.GetComponent<ThirdPersonFlyingController>();
       
    }

    // Update is called once per frame
    void Update()
    {
        //Player effects
        if (flyingController.MoveSpeed() > (flyingController.flyingSpeed * flyingController.runFlyMultiplier - 1) && !isflyingFast)
        {
            fastPlayerParticleSystem = null;
            fastPlayerParticleSystem = (GameObject)Instantiate(playerRunParticles, (playerGO.transform.position + new Vector3(0,0,-1)), playerGO.transform.rotation);
            fastPlayerParticleSystem.transform.parent = playerGO.transform;
            
           // Debug.Log("Instantiate");

            isflyingFast = true;
        
        }

        if (flyingController.MoveSpeed() <= (flyingController.flyingSpeed * flyingController.runFlyMultiplier - 1) && isflyingFast)
        {
            fadeOutPlayerParticles = true;
            isflyingFast = false;
        }


        if (fadeOutPlayerParticles)
        {
            FadeOutParticleSystem();
        }


    }

   private void FadeOutParticleSystem()
   {


       if (fastPlayerParticleSystem.particleEmitter.maxEmission <= 1)
       {

           fastPlayerParticleSystem.particleEmitter.minEmission = 0;
           fastPlayerParticleSystem.particleEmitter.maxEmission = 0;
           fastPlayerParticleSystem.particleEmitter.minEnergy = 0;
           fastPlayerParticleSystem.particleEmitter.maxEnergy = 0;

           fastPlayerParticleSystem.particleEmitter.emit = false;
			
			Destroy(fastPlayerParticleSystem);
			
           fadeOutPlayerParticles = false;
			
       }
       else
       {
           if (fastPlayerParticleSystem.particleEmitter.minEmission > 0)
                fastPlayerParticleSystem.particleEmitter.minEmission -= Time.deltaTime * 10;

          if ( fastPlayerParticleSystem.particleEmitter.maxEmission > 0)
           fastPlayerParticleSystem.particleEmitter.maxEmission -= Time.deltaTime * 10;

          if (fastPlayerParticleSystem.particleEmitter.minEnergy > 0)
           fastPlayerParticleSystem.particleEmitter.minEnergy -= Time.deltaTime * 10;

          if (fastPlayerParticleSystem.particleEmitter.maxEnergy > 0) 
           fastPlayerParticleSystem.particleEmitter.maxEnergy -= Time.deltaTime * 10;


       }



   }
}
