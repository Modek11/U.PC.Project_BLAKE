Naming and code style tips for C# scripting in Unity: https://unity.com/how-to/naming-and-code-style-tips-c-scripting-unity

// EXAMPLE: Public and private variables
<pre><code class='language-cs'>
public float DamageMultiplier = 1.5f;
public float MaxHealth;
public bool IsInvincible;

[SerializeField, Header("Equipment"), Tooltip("Sword")]
private GameObject sword;

[SerializeField]
private GameObject bag;

// don't use prefixes
private bool isDead;
private float currentHealth;

public void InflictDamage(float damage, bool isSpecialDamage)
{
    // local variable
    int totalDamage = damage;

    // local variable versus public member variable
    if (isSpecialDamage)
    {
    	totalDamage *= DamageMultiplier;
    }
}

// avoid nested if statements
public void Attack()
{
    if(sword == null) return;

    if(sword.durability > 0f)
    {
        //...
    }

    //instead of
    if(sword != null)
    {
        if(sword.durability > 0f)
        {
            //...
        }
    }
}
</code></pre>
