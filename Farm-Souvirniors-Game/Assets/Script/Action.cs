using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Action : MonoBehaviour
{

    bool clash = false;
    [SerializeField] int checkAreaCrop ;
    public Items[] itemData ;
     private void OnTriggerEnter2D(Collider2D other) {
      if(other.tag == "Player"){
        // GameManager.instance.getPositionAreaCrop = checkAreaCrop;
          clash = true;
      }
   }

   void OnCropAndHavest (InputValue value){
         if(clash){
       
            if(GameManager.instance.checkClickItem &&  GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<SpriteRenderer>().sprite == null){
                Debug.Log("Test1");
                clash = false;
                GameManager.instance.Crop(checkAreaCrop);
            }
              else if(GameManager.instance.items.Count < GameManager.instance.slots.Length ){ 
                          Debug.Log("Test2");
                          clash = false;
                        Sprite getSprite = GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<SpriteRenderer>().sprite;
                        Debug.Log(getSprite);
                        for(int i=0 ; i<itemData.Length;i++){
                            if(getSprite == itemData[i].itemSprite){
                                  GameManager.instance.addItem(itemData[i]);
                            }
                        }
            GameManager.instance.itemsCrop[checkAreaCrop].transform.GetComponent<SpriteRenderer>().sprite = null;
            //เชคถ้าเก็บเกี่ยวแล้วแต่ยังกดไอเทมในเป๋าอยู่ให้ขึ้นกรอบเขียวให้้ปลูกได้
            if(GameManager.instance.checkClickItem){
                   GameManager.instance.itemsCrop[checkAreaCrop].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0,0.8f,0.3f,0.5f);
            }
            
            
           }else{
             Debug.Log("Slot Full");
           }
          
         }
      
      }
}
