using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{

    public float fadeSpeed = 10f;
    public float fadeAmount = 0.2f;
    float originalOpacity;
    Material[] myMaterials;
    public bool doFade = false;
    // Start is called before the first frame update
    void Start()
    {
        myMaterials = GetComponent<Renderer>().materials;

        foreach(Material mat in myMaterials){
             originalOpacity = mat.color.a;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if(doFade){
            Fade();
        }else{
            ResetFade();
        }
    }

    void Fade(){

        foreach(Material mat in myMaterials){
            Color currentColor = mat.color;
            Color smoothColor =  new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
            mat.color = smoothColor;
        }

       
        //StartCoroutine(AutoResetFade());
    }

    void ResetFade(){
         foreach(Material mat in myMaterials){
            Color currentColor = mat.color;
            Color smoothColor =  new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed * Time.deltaTime));
            mat.color = smoothColor;
        }
    }
    /*
    void OnTriggerEnter(Collider other) {

        if(this.tag == "Prop"){
            if(other.tag == "Player"){
                Fade();
            }
            

        }
        
    } 

    void OnTriggerExit(Collider other) {

        if(this.tag == "Prop"){
            if(other.tag == "Player"){
                ResetFade();
            }
            

        }
        
    } 
    
    IEnumerator AutoResetFade(){

        yield return new WaitForSeconds(3f);
        if(doFade){
            ResetFade();
            doFade = false;
        }
        
    }
    */
}
