using Unity.Mathematics;

public class BlakeHeroCharacter : BlakeCharacter
{
    private void Awake()
    {
        defaultHealth = SceneHandler.Instance.isNormalDifficulty ? 3 : 1;
        health = defaultHealth;
    }

    public override void Die()
    {
        explosionParticleInstantiated = Instantiate(explosionParticle, transform.position, quaternion.identity);
        gameObject.SetActive(false);
        Invoke("Respawn", 2f);

        base.Die();
    }
}
