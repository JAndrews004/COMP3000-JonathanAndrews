using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FXManager : MonoBehaviour
{
    [Header("Sprite Flash")]
    public float flashDuration;
    public float shieldFlashDuration;

    [Header("Particle effects")]
    public GameObject buffParticlePrefab;
    public GameObject debuffParticlePrefab;
    public GameObject healParticlePrefab;
    public GameObject reviveParticlePrefab;
    public GameObject stunParticlePrefab;
    
    public IEnumerator SpriteFlash(PartySlot slot)
    {
        slot.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 1f);
        yield return new WaitForSeconds(flashDuration);
        slot.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0f);
    }

    public IEnumerator SpriteFlash(EnemySlot slot)
    {
        slot.GetComponent<SpriteRenderer>().material.SetColor("_FlashColor", Color.blue);
        slot.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 1f);
        yield return new WaitForSeconds(flashDuration);
        slot.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0f);
        slot.GetComponent<SpriteRenderer>().material.SetColor("_FlashColor", Color.white);
    }

    public IEnumerator ShieldFlashEffect(PartySlot slot)
    {
        slot.GetComponent<SpriteRenderer>().material.SetColor("_FlashColor", Color.blue);
        slot.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 1f);
        yield return new WaitForSeconds(shieldFlashDuration);
        slot.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0f);
        slot.GetComponent<SpriteRenderer>().material.SetColor("_FlashColor", Color.white);
    }

    public IEnumerator ShieldFlashEffect(EnemySlot slot)
    {
        slot.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 1f);
        yield return new WaitForSeconds(flashDuration);
        slot.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0f);
    }
    public void SetAlpha(PartySlot slot,float alpha)
    {
        Color tint = slot.mat.GetColor("_Color");
        tint.a = alpha;
        slot.mat.SetColor("_Color", tint);
        
    }
    public void SetAlpha(EnemySlot slot, float alpha)
    {
        Color tint = slot.mat.GetColor("_Color");
        tint.a = alpha;
        slot.mat.SetColor("_Color", tint);
    }
    public void SpawnBuffEffect(Transform slotTransform, bool Enemy)
    {
        GameObject fx = Instantiate(buffParticlePrefab, slotTransform.position, Quaternion.identity);
        fx.transform.SetParent(slotTransform, worldPositionStays: true); // follow the slot
        fx.transform.localScale = Vector3.one;
        if (Enemy)
        {
            fx.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y - 2.5f, fx.transform.position.z);
        }
        else{
            fx.transform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y - 2.5f, fx.transform.position.z);
        }
        
        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(fx, 2f);
        }
    }
    public void SpawnDebuffEffect(Transform slotTransform, bool Enemy)
    {
        GameObject fx = Instantiate(debuffParticlePrefab, slotTransform.position, Quaternion.identity);
        fx.transform.SetParent(slotTransform, worldPositionStays: true); // follow the slot
        fx.transform.localScale = Vector3.one;
        if (Enemy)
        {
            fx.transform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y + 2.5f, fx.transform.position.z);
        }
        else
        {
            fx.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y + 2.5f, fx.transform.position.z);
        }

        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(fx, 2f);
        }
    }
    public void spawnHealEffect(Transform slotTransform, bool Enemy)
    {
        GameObject fx = Instantiate(healParticlePrefab, slotTransform.position, Quaternion.identity);
        fx.transform.SetParent(slotTransform, worldPositionStays: true); // follow the slot
        fx.transform.localScale = Vector3.one;
        if (Enemy)
        { 
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y, fx.transform.position.z);
        }
        else
        {
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y, fx.transform.position.z);
        }

        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(fx, 2f);
        }
    }
    public void spawnReviveEffect(Transform slotTransform, bool Enemy)
    {
        GameObject fx = Instantiate(reviveParticlePrefab, slotTransform.position, Quaternion.identity);
        fx.transform.SetParent(slotTransform, worldPositionStays: true); // follow the slot
        fx.transform.localScale = Vector3.one;
        if (Enemy)
        {
            fx.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y-1, fx.transform.position.z);
        }
        else
        {
            fx.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y-1, fx.transform.position.z);
        }

        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(fx, 2f);
        }
    }
    public void spawnStunEffect(Transform slotTransform, bool Enemy)
    {
        GameObject fx = Instantiate(stunParticlePrefab, slotTransform.position, Quaternion.identity);
        fx.transform.SetParent(slotTransform, worldPositionStays: true); // follow the slot
        fx.transform.localScale = Vector3.one;
        if (Enemy)
        {
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y + 1f, fx.transform.position.z);
        }
        else
        {
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y + 1f, fx.transform.position.z);
        }

        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(fx, 2f);
        }
    }
}
