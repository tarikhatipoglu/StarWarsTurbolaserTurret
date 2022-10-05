using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Raycast and Range")]
    float range = 1000.0f;
    RaycastHit hit;

    [Header("Turret Rotating")]
    [SerializeField] Transform turretLeftRight;
    [SerializeField] Transform turretUpDown;
    [SerializeField] float turretRotateSpeed;
    [SerializeField] float barrelRotateSpeed;
    float turretAngle;
    float barrelAngle;

    [Header("Raycast and Range")]
    [SerializeField] Animator turretAnim;

    [Header("Turret fire")]
    [SerializeField] Transform turretBarrel1;
    [SerializeField] Transform turretBarrel2;

    [Header("Laser and Explosion prefabs")]
    [SerializeField] GameObject laser;
    [SerializeField] GameObject explosion;

    [Header("Shooting Settings")]
    [SerializeField] float shootRate;
    [SerializeField] float shootRateTimeStamp;
    [SerializeField] float shootSpeed;

    [Header("Audio Source")]
    [SerializeField] AudioSource A_S;

    [Header("Random Fire sounds")]
    [SerializeField] AudioClip[] fireSounds;

    [Header("Random Hit sounds")]
    [SerializeField] AudioClip[] hitSounds;

    [Header("Rotate sounds")]
    [SerializeField] AudioClip turretRotateSound;
    [SerializeField] AudioClip barrelRotateSound;

    void Start()
    {
        A_S = GetComponent<AudioSource>();
    }

    void Update()
    {
        TurretRotate();

        //Mouse 1'e bast���nda ate� etmesi i�in
        if (Input.GetMouseButton(0))
        {
            //Taret belirli s�re aral���nda ate� ediyor
            if (Time.time > shootRateTimeStamp)
            {
                //S�re bitince ate� edebilir
                Shooting();
                A_S.clip = fireSounds[Random.Range(0, fireSounds.Length)];
                A_S.Play();
                turretAnim.Play("Fire");
                shootRateTimeStamp = Time.time + shootRate;
            }
        }
    }

    void Shooting()
    {
        //Turret, Kamera sayesinde Mouse'un tuttu�u yere y�nelicek ve ate� edicek
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, range))
        {
            //Taret'in iki namlusuna lazer spawn etmek i�in
            GameObject laser1 = GameObject.Instantiate(laser, turretBarrel1.position, turretBarrel1.rotation) as GameObject;
            GameObject laser2 = GameObject.Instantiate(laser, turretBarrel2.position, turretBarrel2.rotation) as GameObject;

            laser1.GetComponent<Rigidbody>().velocity = turretBarrel1.right * -shootSpeed;
            laser2.GetComponent<Rigidbody>().velocity = turretBarrel2.right * -shootSpeed;

            //Lazerler belirli bir s�reden sonra yok olucak
            GameObject.Destroy(laser1, 5f);
            GameObject.Destroy(laser2, 5f);
        }
    }
    void TurretRotate()
    {
        //Taret'in boyun k�sm�n� saga sola d�nd�rebilmek i�in
        turretAngle += Input.GetAxis("Mouse X") * turretRotateSpeed * Time.deltaTime;
        turretLeftRight.localRotation = Quaternion.AngleAxis(turretAngle, Vector3.up);

        //Taret'in namlusunu yukar� asag� d�nd�rebilmek i�in
        barrelAngle += Input.GetAxis("Mouse Y") * barrelRotateSpeed * Time.deltaTime;
        barrelAngle = Mathf.Clamp(barrelAngle, -45, 95);
        turretUpDown.localRotation = Quaternion.AngleAxis(barrelAngle, Vector3.up);
    }
}
