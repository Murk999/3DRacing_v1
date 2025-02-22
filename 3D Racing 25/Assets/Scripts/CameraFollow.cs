using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour //слежение камеры
{
    [SerializeField] private Transform target; //на что будеи смотреть камера
    [SerializeField] private new Rigidbody rigidbody; //чтобы брать скорость

    [Header("Offset")]
    [SerializeField] private float viewHeight; //регулирует угол наклона камеры
    [SerializeField] private float height; //высота
    [SerializeField] private float distance; //дистанция

    [Header("Damping")]
    [SerializeField] private float rotationDamping;
    [SerializeField] private float heightDamping;
    [SerializeField] private float speedThreshold;

    private void FixedUpdate()
    {
        Vector3 velocity = rigidbody.velocity; //вектор направления
        Vector3 targetRotation = target.eulerAngles;

        if (velocity.magnitude > speedThreshold)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up).eulerAngles;
        }

        //Lerp
        float currentAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation.y, rotationDamping * Time.fixedDeltaTime);
        float currentHeight = Mathf.Lerp(transform.position.y, target.position.y + height, heightDamping * Time.fixedDeltaTime);

        //Position
        Vector3 positionOffsey = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance; //направление
        transform.position = target.position - positionOffsey;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        //Rotation
        transform.LookAt(target.position + new Vector3(0, viewHeight, 0)); //куда будет смотреть камера
    }

}
