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

        //Mouse 1'e bastýðýnda ateþ etmesi için
        if (Input.GetMouseButton(0))
        {
            //Taret belirli süre aralýðýnda ateþ ediyor
            if (Time.time > shootRateTimeStamp)
            {
                //Süre bitince ateþ edebilir
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
        //Turret, Kamera sayesinde Mouse'un tuttuðu yere yönelicek ve ateþ edicek
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, range))
        {
            //Taret'in iki namlusuna lazer spawn etmek için
            GameObject laser1 = GameObject.Instantiate(laser, turretBarrel1.position, turretBarrel1.rotation) as GameObject;
            GameObject laser2 = GameObject.Instantiate(laser, turretBarrel2.position, turretBarrel2.rotation) as GameObject;

            laser1.GetComponent<Rigidbody>().velocity = turretBarrel1.right * -shootSpeed;
            laser2.GetComponent<Rigidbody>().velocity = turretBarrel2.right * -shootSpeed;

            //Lazerler belirli bir süreden sonra yok olucak
            GameObject.Destroy(laser1, 5f);
            GameObject.Destroy(laser2, 5f);
        }
    }
    void TurretRotate()
    {
        //Taret'in boyun kýsmýný saga sola döndürebilmek için
        turretAngle += Input.GetAxis("Mouse X") * turretRotateSpeed * Time.deltaTime;
        turretLeftRight.localRotation = Quaternion.AngleAxis(turretAngle, Vector3.up);

        //Taret'in namlusunu yukarý asagý döndürebilmek için
        barrelAngle += Input.GetAxis("Mouse Y") * barrelRotateSpeed * Time.deltaTime;
        barrelAngle = Mathf.Clamp(barrelAngle, -45, 95);
        turretUpDown.localRotation = Quaternion.AngleAxis(barrelAngle, Vector3.up);
    }
}
