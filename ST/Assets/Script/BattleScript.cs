using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BattleScript : MonoBehaviourPun
{
    public Spinner spinnerScript;

    private Rigidbody rb;

    private float startSpinSpeed;
    private float currentSpinSpeed;

    public Image spinSpeedBar_Image;
    public TextMeshProUGUI SpinSpeedRatio_Text;
    public GameObject UI_3DGameObject;
    public GameObject deathPanalUIPrefab;
    private GameObject deathPanalUIGameObject;

    private bool isDead = false;

    public bool isAttacker;
    public bool isDefender;

    public float Common_Damage_Coefficient = 0.04f;
    [Header("Player Type Damage Coefficient")]
    //Attacker
    public float dodamage_Coefficient_Attacker = 10f;
    public float getdamage_Coefficient_Attacker = 1.2f;
    //Defender
    public float dodamage_Coefficient_Defender = 0.75f;
    public float getdamage_Coefficient_Defender = 0.2f;




    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                Vector3 effectPosition = (gameObject.transform.position + collision.transform.position) / 2 + new Vector3(0, 0.05f, 0);

                //Instantiate Collision Effect ParticleSystem
                GameObject collisionEffectGameobject = GetPooledObject();
                if (collisionEffectGameobject != null)
                {
                    collisionEffectGameobject.transform.position = effectPosition;
                    collisionEffectGameobject.SetActive(true);
                    collisionEffectGameobject.GetComponentInChildren<ParticleSystem>().Play();

                    //De-activate Collision Effect Particle System after some seconds.
                    StartCoroutine(DeactivateAfterSeconds(collisionEffectGameobject, 0.5f));

                }
            }
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float defult_Damage_Amount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600f * Common_Damage_Coefficient;

            if (isAttacker)
            {
                defult_Damage_Amount *= dodamage_Coefficient_Attacker;
            }
            else if (isDefender)
            {
                defult_Damage_Amount *= dodamage_Coefficient_Defender;
            }

            if (mySpeed > otherPlayerSpeed)
            {
                
                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, defult_Damage_Amount);
                }
            }
        }
    }


    [PunRPC]
    public void DoDamage(float _damegeAmount)
    {
        if (!isDead)
        {
            if (isAttacker)
            {
                _damegeAmount *= getdamage_Coefficient_Attacker;

                if (_damegeAmount > 1000)
                {
                    _damegeAmount = 400f;
                }
            }
            else if (isDefender)
            {
                _damegeAmount *= getdamage_Coefficient_Defender;
            }

            spinnerScript.spinSpeed -= _damegeAmount;
            currentSpinSpeed = spinnerScript.spinSpeed;
            spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
            SpinSpeedRatio_Text.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed;

            if (currentSpinSpeed < 100)
            {
                //Die
                Die();
            }
        }
    }
    private void CheckPlayerType()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else if (gameObject.name.Contains("Defender"))
        {
            isDefender = true;
            isAttacker = false;
            spinnerScript.spinSpeed = 4400f;
            startSpinSpeed = spinnerScript.spinSpeed;
            currentSpinSpeed = spinnerScript.spinSpeed;
            SpinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;

        }
    }
    public void  Die()
    {
        isDead = true;  
        GetComponent<MovementController>().enabled = false ;
        rb.freezeRotation = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        spinnerScript.spinSpeed = 0f;
        UI_3DGameObject.SetActive(false);

        if (photonView.IsMine)
        {
            StartCoroutine(ReSpawn());
        }
       


    }

    IEnumerator ReSpawn()
    {
        GameObject canvasGameObject = GameObject.Find("Canvas");
        if (deathPanalUIGameObject == null)
        {
            deathPanalUIGameObject = Instantiate(deathPanalUIPrefab, canvasGameObject.transform); 
        }
        else
        {
            deathPanalUIGameObject.SetActive(true);
        }
        Text respawnTimeText = deathPanalUIGameObject.transform.Find("RespawnTimeText").GetComponent<Text>() ;
        float respawnTime = 8.0f;
        respawnTimeText.text = respawnTime.ToString(".00");
        while (respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;
            respawnTimeText.text = respawnTime.ToString(".00");
            GetComponent<MovementController>().enabled = false;
        }
        deathPanalUIGameObject?.SetActive(false);   
        GetComponent<MovementController>().enabled = true;

        photonView.RPC("ReBorn",RpcTarget.AllBuffered);
        
    }

    [PunRPC]
    public void ReBorn()
    {
    spinnerScript.spinSpeed = startSpinSpeed;
    currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
        SpinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;

        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        UI_3DGameObject.SetActive(true);
        isDead = false;
    }
    public List<GameObject> pooledObjects;
    public int amountToPool = 8;
    public GameObject CollisionEffectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        CheckPlayerType();

        rb = GetComponent<Rigidbody>();


        if (photonView.IsMine)
        {
            pooledObjects = new List<GameObject>();
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(CollisionEffectPrefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }



    }

    public GameObject GetPooledObject()
    {

        for (int i = 0; i < pooledObjects.Count; i++)
        {

            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);

    }
}
