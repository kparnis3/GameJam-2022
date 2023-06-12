
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    
    public Sprite pressedBtn;
    public Sprite unPressedBtn;
    public Button btn;


    private void OnMouseDown()
    {
        btn.image.sprite = pressedBtn;
        //AudioManager.instance.PlaySFX("ButtonClick");
    }

    private void OnMouseUpAsButton()
    {
        btn.image.sprite = unPressedBtn;
    }


}
