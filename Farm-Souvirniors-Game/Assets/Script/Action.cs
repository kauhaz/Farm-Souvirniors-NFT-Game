using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Action : MonoBehaviour
{

    bool clash = false;
    [SerializeField] int checkAreaCrop ;

    public string checkNftId ;
    public string statusNFT ;

    public string typeNFT ;

    private string getNft_id ;

    // private bool checkAlreadyPlant = false;

    public Items[] itemData ;

    private List<string> ok = new List<string>();



    
    string urlCropNFT = "https://farm-souvirniors-api.herokuapp.com/in-game/plant-nft";
    string urlHavestNFT = "https://farm-souvirniors-api.herokuapp.com/in-game/harvest-nft";

    string urlFeedNFT = "https://farm-souvirniors-api.herokuapp.com/in-game/feed-nft";
    string addressWallet = "0x629812063124cE2448703B889D754b232B3622BA";
    string itemID ;

    bool selectNftUsed = true;
     private void OnTriggerEnter2D(Collider2D other) {
      if(other.tag == "Player"){

         GameManager.instance.itemsCrop[checkAreaCrop].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.28f);
    
          clash = true;
      }
   }
     private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player"){
            clash = false;
         
                if(GameManager.instance.checkClickItem && GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<SpriteRenderer>().sprite == null ){
                    GameManager.instance.itemsCrop[checkAreaCrop].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0,0.5f,0.3f,0.5f);
                }
                else
                {
                      GameManager.instance.itemsCrop[checkAreaCrop].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
                }
      
               
            
            
        }
    }

   void OnCropAndHavest (InputValue value){
          
        //ถ้าชน
         if(clash){

                    Debug.Log("OnCropAndHavest");
                    //เชคเพื่อการปลูก โดยเช็คว่าพื้นตรงนั้นปลูกไปหรือยัง ถ้ายังให้ปลูกได้
                    if((GameManager.instance.checkClickItem &&  GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<SpriteRenderer>().sprite == null)){
                        Debug.Log("checkCrop");
                        clash = false;
                        for(int i=0;i<GameManager.instance.dataTest.Count;i++){
                             if(selectNftUsed){
                                 
                                       
                                        if(GameManager.instance.dataTest[i].name == GameManager.instance.chooseItem.itemName && GameManager.instance.dataTest[i].status == "not_plant"){

                                                itemID = GameManager.instance.dataTest[i].nft_id;
                                                        GameManager.instance.actionTimeout = true;
                                                        GameManager.instance.numberLand = checkAreaCrop;
                                                        GameManager.instance.timeStart[checkAreaCrop] = 60f;
                                                        GameManager.instance.dataTest[i].status = "wait_feed";
                                                        GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<Action>().typeNFT = GameManager.instance.dataTest[i].type;
                                                        GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<Action>().checkNftId = GameManager.instance.dataTest[i].nft_id;
                                                        GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<Action>().statusNFT = "wait_feed";
                                                        selectNftUsed = false;
                                         
                                        } 
                         
                          
                                }
                        }
                        GameManager.instance.Crop(urlCropNFT,addressWallet,itemID,checkAreaCrop);
                    }
            for(int k=0;k<GameManager.instance.dataTest.Count;k++){ 
                //ถ้าตำแหน่งที่ปลูกที่ดึงมาจาก api ตรงกับช่องที่ปลูกที่ยืนอยู่
                if(GameManager.instance.dataTest[k].position_plant == checkAreaCrop){
                       
                    if((GameManager.instance.items.Count < GameManager.instance.slots.Length) && (GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<Action>().statusNFT == "wait_harvest" && GameManager.instance.textTime[checkAreaCrop].text == "0")){
                       
                            Debug.Log("checkHavest");
                            clash = false;      
                            Sprite getSprite = GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<SpriteRenderer>().sprite;
                            getNft_id = GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<Action>().checkNftId;
                            if(getSprite != null){
                                GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<Action>().statusNFT = "not_plant";
                                GameManager.instance.dataTest[k].status = "not_plant";
                                GameManager.instance.showCrop[checkAreaCrop].transform.GetComponent<SpriteRenderer>().sprite = null;
                                StartCoroutine(GameManager.instance.HttpHavestPost(urlHavestNFT,addressWallet,getNft_id));
                            }
                            //ทำให้รูปหาย ไม่ได้ลบแต่เปลี่ยนเปนว่างแทน
                            GameManager.instance.itemsCrop[checkAreaCrop].transform.GetComponent<SpriteRenderer>().sprite = null;
                            //เชคถ้าเก็บเกี่ยวแล้วแต่ยังกดไอเทมในเป๋าอยู่ให้ขึ้นกรอบเขียวให้้ปลูกได้
                            if(GameManager.instance.checkClickItem){
                            GameManager.instance.itemsCrop[checkAreaCrop].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0,0.5f,0.3f,0.5f);
                            }
                            for(int i=0 ; i<itemData.Length;i++){
                                if(getSprite == itemData[i].itemSprite){
                                    GameManager.instance.addItem(itemData[i] , checkAreaCrop);
                                }
                            }
                        
                        
                       
            
                    }else if(GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<Action>().statusNFT == "wait_feed" && GameManager.instance.textTime[checkAreaCrop].text == "0" ){
                        // && GameManager.instance.textTime[checkAreaCrop].text == null
                        Debug.Log("checkFeed");
                        clash = false;
                        Sprite getSprite = GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<SpriteRenderer>().sprite;
                        getNft_id = GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<Action>().checkNftId;
                        if(getSprite != null){
                        GameManager.instance.itemsCrop[checkAreaCrop].GetComponent<Action>().statusNFT = "wait_harvest";
                        GameManager.instance.dataTest[k].status = "wait_harvest";
                        GameManager.instance.showCrop[checkAreaCrop].transform.GetComponent<SpriteRenderer>().sprite = null;
                         GameManager.instance.timeStart[checkAreaCrop] = 60f;
                        GameManager.instance.actionTimeout = true;
                        GameManager.instance.numberLand = checkAreaCrop;
                        StartCoroutine(GameManager.instance.HttpFeedPost(urlFeedNFT,addressWallet,getNft_id));
                        
                        }
                    }
                
                
                }
                      
            }
            
           
            
            //    else if(GameManager.instance.showCrop[checkAreaCrop] == "")
          
        }
      
      }
}
