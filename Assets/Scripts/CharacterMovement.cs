using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15;
    [SerializeField] private Animator animator;
    [SerializeField] private float punchRadius = 5;
    [SerializeField] private List<RagdollController> ragdollTargets = new List<RagdollController>();
    [SerializeField] private CollectAndStackObjects collectAndStackObjects;
    [SerializeField] private CurrencyTextUpdater currencyTextUpdater;
    private float currentPunchDistance;
    private int stackingCapacity;
    [SerializeField] private Material[] materials;
    [SerializeField] private SkinnedMeshRenderer[] characterMeshes;
    [SerializeField] private Joystick joystick;
    private void Start()
    {
        stackingCapacity = 2;
    }
    private void Update()
    {
        /*float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");*/
        Vector3 moveDirection = new Vector3(joystick.Horizontal, 0f, joystick.Vertical).normalized;transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        float movementAmount = moveDirection.magnitude;
        animator.SetFloat("velocity", movementAmount);

        VerifyPunchableCharacters();
    }

    private void VerifyPunchableCharacters()
    {
        for (int i = ragdollTargets.Count - 1; i >= 0; i--)
        {
            currentPunchDistance = Vector3.Distance(transform.position, ragdollTargets[i].transform.position);
            if (currentPunchDistance <= punchRadius)
            {
                if (!ragdollTargets[i].isPunched)
                {
                    animator.Play("Punching", 1, 0);
                    ragdollTargets[i].EnableRagdoll();
                }
            }
        }
    }
    public void IncreaseStackingCapacity()
    {
        if (CurrencyManager.Instance.CurrentCash < 10)
        {
            currencyTextUpdater.ShowWarningMessage("Não há dinheiro sucficiente!");
            return;
        }
        CurrencyManager.Instance.SpendCash(10);
        stackingCapacity += 1;
    }
    public void ChangeColorPallete(int colorIndex)
    {
        if (CurrencyManager.Instance.CurrentCash < 5)
        {
            currencyTextUpdater.ShowWarningMessage("Não há dinheiro sucficiente!");
            return;
        }
        CurrencyManager.Instance.SpendCash(5);
        UpdateCharacterMaterial(colorIndex);
    }
    private void PickUp(int index)
    {
        if(collectAndStackObjects.CurrentStackSize() >= stackingCapacity)
        {
            currencyTextUpdater.ShowWarningMessage("Capacidade de empilhamento insuficiente!");
            return;
        }
        ragdollTargets[index].PrepareForPickup();
        ragdollTargets[index].transform.rotation = Quaternion.identity;
        collectAndStackObjects.AddObject(ragdollTargets[index].gameObject);
        ragdollTargets.Remove(ragdollTargets[index]);
    }
    public void PickUpButton()
    {
        for (int i = ragdollTargets.Count - 1; i >= 0; i--)
        {
            currentPunchDistance = Vector3.Distance(transform.position, ragdollTargets[i].transform.position);
            if (currentPunchDistance <= punchRadius)
            {
                if (ragdollTargets[i].isPunched)
                {
                    PickUp(i);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Dump"))
        {
            collectAndStackObjects.RemoveObjects();
        }
    }
    private void UpdateCharacterMaterial(int materialIndex)
    {
        for(int i = 0; i < characterMeshes.Length; i++)
        {
            characterMeshes[i].material = materials[materialIndex];
        }
    }
}