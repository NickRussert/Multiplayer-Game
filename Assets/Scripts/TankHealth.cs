using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class TankHealth : NetworkBehaviour
{
    public int maxHealth = 3;
    private NetworkVariable<int> currentHealth = new NetworkVariable<int>(3);

    private NetworkVariable<bool> isInvincible = new NetworkVariable<bool>(false);

    public float invincibilityDuration = 2f;
    public float blinkInterval = 0.2f;
    public float explosionDuration = 0.5f;

    private SpriteRenderer[] spriteRenderers;
    private SpriteRenderer mainSpriteRenderer;
    private Sprite originalSprite;

    public GameObject[] hearts;

    public AudioClip hitSound;
    public AudioClip explosionSound;
    public float soundVolume = 0.7f;

    public Sprite explosionSprite;

    private AudioSource audioSource;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }

        currentHealth.OnValueChanged += (oldValue, newValue) =>
        {
            UpdateHeartsClientRpc(newValue);
        };

        UpdateHeartsClientRpc(currentHealth.Value);
    }

    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (mainSpriteRenderer != null)
        {
            originalSprite = mainSpriteRenderer.sprite;
        }

        UpdateHeartsClientRpc(currentHealth.Value);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible.Value) return;

        if (IsServer)
        {
            currentHealth.Value -= damage;

            if (currentHealth.Value <= 0)
            {
                TriggerExplosionClientRpc();
                StartCoroutine(ExplosionEffect());
            }
            else
            {
                StartInvincibilityServerRpc(); // Now properly calls invincibility function
            }
        }

        PlayHitSoundClientRpc();
    }

    public void Heal(int amount)
    {
        if (IsServer)
        {
            currentHealth.Value = Mathf.Min(currentHealth.Value + amount, maxHealth);
        }
    }

    [ClientRpc]
    private void UpdateHeartsClientRpc(int health)
    {
        if (hearts == null || hearts.Length == 0) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < health);
        }
    }

    [ClientRpc]
    private void PlayHitSoundClientRpc()
    {
        if (hitSound != null) AudioSource.PlayClipAtPoint(hitSound, transform.position, soundVolume);
    }

    [ClientRpc]
    private void TriggerExplosionClientRpc()
    {
        if (explosionSprite != null)
        {
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.sprite = explosionSprite;
            }
        }

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, soundVolume);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartInvincibilityServerRpc()
    {
        if (isInvincible.Value) return;

        isInvincible.Value = true;
        StartCoroutine(BecomeInvincible());
    }

    IEnumerator BecomeInvincible()
    {
        isInvincible.Value = true;
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            ToggleSpriteVisibilityClientRpc(false);
            yield return new WaitForSeconds(blinkInterval);
            ToggleSpriteVisibilityClientRpc(true);
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval * 2;
        }

        ToggleSpriteVisibilityClientRpc(true);
        isInvincible.Value = false;
    }

    [ClientRpc]
    private void ToggleSpriteVisibilityClientRpc(bool visible)
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = visible;
        }
    }

    IEnumerator ExplosionEffect()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        yield return new WaitForSeconds(explosionDuration);

        DestroyTankServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyTankServerRpc()
    {
        DestroyTankClientRpc();
    }

    [ClientRpc]
    private void DestroyTankClientRpc()
    {
        Destroy(gameObject);
    }
}
