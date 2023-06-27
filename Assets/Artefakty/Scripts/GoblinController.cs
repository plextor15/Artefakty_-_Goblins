using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    public bool IsSelected;
    private float azimuth;

    public enum State
    {
        Standing = 0
        , Walking = 1
        , Attack = 2
    }

    public State CharacterState;
    public bool IsAlerted;

    public float MinDistance = 0.1f;
    public float RotationSpeed = 60.0f;
    public float WalkSpeed = 10.0f;
    public float RunSpeed = 20.0f;

    private Animator animator;
    public bool IsAlive;

    private Vector3 target;
    public float MinWanderDistance = 1.5f;
    public float MaxWanderDistance = 5.0f;

    void Start()
    {
        IsSelected = false;
        animator = this.GetComponent<Animator>();
        azimuth = transform.rotation.eulerAngles.y;
        IsAlive = true;
        target = transform.position;
        CharacterState = State.Standing;
    }

    public float Hp = 10.0f;
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("sword hit");
        if (collider.GetComponent<Sword>() != null)
        {
            var arthur = collider.GetComponentInParent<ArthurCharacterController>();

            if (!arthur.HasBeenHit)
            {
                arthur.HasBeenHit = true;
                Hp -= arthur.Damage;
                if (Hp <= 0.0f)
                {
                    IsAlive = false;
                }
                else { 
                    IsAlerted = true; 
                }
            }
        }
    }

    public void Hit()
    {
        if ((target - transform.position).sqrMagnitude < 2.0f * 2.0f) {
            FindObjectOfType<ArthurCharacterController>().IsAlive = false;
        }
    }
    void Update()
    {
        animator.SetBool("IsAlive", IsAlive);
        if (IsAlive)
        {
            var materialPropertyBlock = new MaterialPropertyBlock();
            if (IsSelected)
                materialPropertyBlock.SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
            else
                materialPropertyBlock.SetColor("_EmissionColor", new Color(0f, 0f, 0f));

            foreach (var mesh in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                mesh.SetPropertyBlock(materialPropertyBlock);
            }
            foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
            {
                mesh.SetPropertyBlock(materialPropertyBlock);
            }

            var distance = target - this.transform.position;
            if (distance.sqrMagnitude > MinDistance * MinDistance)
            {
                var desiredAzimuth = Mathf.Atan2(distance.x, distance.z) * Mathf.Rad2Deg;
                azimuth = Mathf.MoveTowardsAngle(azimuth, desiredAzimuth, RotationSpeed * Time.deltaTime);

                CharacterState = State.Walking;

                if (IsAlerted){
                    GetComponent<Rigidbody>().position += transform.forward * RunSpeed * Time.deltaTime;
                }
                else{
                    GetComponent<Rigidbody>().position += transform.forward * WalkSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (!IsAlerted)
                {
                    target = transform.position + Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0) * Vector3.forward * Random.Range(MinWanderDistance, MaxWanderDistance);
                }
                
                CharacterState = State.Standing;
            }

            if (IsAlerted)
            {
                target = FindObjectOfType<ArthurCharacterController>().transform.position;

                if((target - transform.position).sqrMagnitude < 2.0f * 2.0f)
                {
                    CharacterState = State.Attack;
                }
            }

            animator.SetBool("IsAlerted", IsAlerted);
            animator.SetInteger("State", (int)CharacterState);
        }
        transform.rotation = Quaternion.Euler(0, azimuth, 0);
    }
}
