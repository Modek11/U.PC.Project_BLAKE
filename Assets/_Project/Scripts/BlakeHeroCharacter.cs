using Unity.Mathematics;

public class BlakeHeroCharacter : BlakeCharacter
{
    private void Awake()
    {
        ReferenceManager.BlakeHeroCharacter = this;

        defaultHealth = ReferenceManager.SceneHandler.isNormalDifficulty ? 3 : 1;
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
