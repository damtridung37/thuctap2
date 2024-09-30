using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{	private void Update()
		{
		if (player != null)
			{
			if (player.position.x < transform.position.x)
			{
					// Người chơi bên trái -> Quái vật quay trái (lật trục X)
				transform.localScale = new Vector3(-3, 3, 3);
			}
			else
			{
				// Người chơi bên phải -> Quái vật quay phải (bình thường)
				transform.localScale = new Vector3(3, 3, 3);
			}
			
		}
	}
}
