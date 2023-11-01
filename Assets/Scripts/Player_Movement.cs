using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//CITATION: used tutorial https://www.youtube.com/watch?v=Ov9ekwAGhMA
public class Player_Movement : MonoBehaviour
{


    public FootstepSFX footstepSFX;
    public float footstepDelay = 0.5f;

 

    private float footstepCounter;

   

    // Start is called before the first frame update
    void Start(){
        

        
    }

   

   

    // Update is called once per frame
   
        //crouching
      


    private void HandleFootstepSFX()
    {
        if (this.footstepCounter > 0)
        {
            this.footstepCounter -= Time.deltaTime;
        } else
        {
            this.footstepCounter = this.footstepDelay;
            this.footstepSFX.PlaySFX();
        }
    }

    private void ResetFootstepSFX()
    {
        this.footstepCounter = 0;
    }

   

    

   



 
}
