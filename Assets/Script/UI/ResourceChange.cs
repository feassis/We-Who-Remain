using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceChange : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Animator anim;
    [SerializeField] private float lifeTime = 1.1f;

    public void Setup(int amountChange)
    {
        amountText.text = amountChange.ToString();
        anim.Play("UpAnimation");

        if(amountChange < 0)
        {
            transform.position = new Vector3(transform.position.x -15, 
                amountText.rectTransform.position.y, 
                amountText.rectTransform.position.z);
            amountText.color = Color.red;
        }
        else
        {
            transform.position = new Vector3(transform.position.x + 15,
                amountText.rectTransform.position.y,
                amountText.rectTransform.position.z);
            amountText.color = Color.green;
        }

        Destroy(gameObject, lifeTime);
    }
}
