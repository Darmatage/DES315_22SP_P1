using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BenThompson_Switch : MonoBehaviour
{
		public Sprite SwitchOffArt;
		public Sprite SwitchOnArt;
		public SpriteRenderer sp;
		public Color offColor;
		public Color onColor;
		public UnityEvent ue;


		[SerializeField]
		SpriteRenderer minimapLock;

		// Start is called before the first frame update
		void Start()
		{
				sp.sprite = SwitchOffArt;
				sp.color = offColor;

				minimapLock.color = offColor;
		}

		void OnTriggerEnter2D(Collider2D other)
		{
				if ((other.gameObject.tag == "Player"))
				{
						sp.sprite = SwitchOnArt;
						sp.color = onColor;
						minimapLock.enabled = false;
						ue.Invoke();
				}
		}
}
