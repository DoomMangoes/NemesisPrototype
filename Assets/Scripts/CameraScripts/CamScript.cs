using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour
{
    private ObjectFade[] _fader;
   

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     

        if(player != null){
            Vector3 dir = player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)){

                if(hit.collider == null)
                    return;

                if(hit.collider.tag == "Player"){
                    
                    if(_fader != null){

                        foreach(ObjectFade fade in _fader){
                             fade.doFade = false;
                        }
                       
                    }
                }else{
                    if(_fader != null){
                         foreach(ObjectFade fade in _fader){
                             fade.doFade = false;
                        }
                        _fader = null;
                    }
                   

                    if(hit.collider.tag == "Wall"){
                        
                         _fader = hit.collider.GetComponentsInChildren<ObjectFade>();
                         if(_fader != null){

                        foreach(ObjectFade fade in _fader){
                             fade.doFade = true;
                        }
                        }
                    }
                    /*
                     || hit.collider.tag == "Prop"
                    else{
                         _fader[0] = hit.collider.GetComponent<ObjectFade>();
                        if(_fader != null){

                        foreach(ObjectFade fade in _fader){
                             fade.doFade = true;
                        }
                       
                        }
                    }
                   */
                    
                }
            }
        }
    }
}
