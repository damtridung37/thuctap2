using UnityEngine;
public class ExpPickUp : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private int expLevel = 0;
    public BoxCollider2D boxCollider;
    
    public void SetExpLevel(int expLevel)
    {
        if(expLevel <= 0) expLevel = 1;
        else if(expLevel > 5) expLevel = 5;
        this.expLevel = expLevel;

        switch (expLevel)
        {
            case 1:
                spriteRenderer.color = Color.green;
                break;
            case 2:
                spriteRenderer.color = Color.blue;
                break;
            case 3:
                spriteRenderer.color = Color.magenta;
                break;
            case 4:
                spriteRenderer.color = Color.yellow;
                break;
            case 5:
                spriteRenderer.color = Color.red;
                break;
        }
    }
    
    public int GetExpAmount()
    {
        return (int)Mathf.Pow(2f,(float)expLevel) * 10;
    }
}
