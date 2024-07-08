using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    [Header("Origin Card")]
    public Card parentCard;
    public Transform parentCardTransform;

    public Transform ShakeTransform;
    public Transform TiltTransform;

    [Header("Properties")]
    public float positionSpeed = 30;
    public float rotationSpeed = 20;
    public float rotationMagnitude = 50;

    [Header("Details")]
    public Image CardImage;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Cost;

    private Vector3 rotationDelta;
    private Vector3 movementDelta;


    void Update()
    {
        transform.position = Vector3.Slerp(transform.position, parentCardTransform.position, positionSpeed * Time.deltaTime);

        Vector3 movement = transform.position - parentCardTransform.position;
        movement.x /= Screen.width;
        movement.y /= Screen.height;
        movement.z /= Screen.height;
        Vector3 movementRotation = movement * (rotationMagnitude * 10);

        rotationDelta = Vector3.Slerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotationDelta.x);
    }


    public void UpdateCard(Image newImage, string newName, string newDescription, int newCost)
    {
        if(newImage != null)       CardImage = newImage;
        if(newName != null)        Name.text = newName;
        if(newDescription != null) Description.text = newDescription;
        if(newCost != -1)          Cost.text = "Cost: " + newCost + "hp";
    }
}
