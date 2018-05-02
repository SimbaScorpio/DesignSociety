using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignSociety
{
	public class UILocationIcon : MonoBehaviour
	{
		private CameraPerspectiveEditor editor;
		private Ray ray;
		private RaycastHit hit;

		private bool over = false;
		private float speed = 10f;
		private float min = 0.07f;
		private float max = 0.08f;

		void Start ()
		{
			editor = Camera.main.GetComponent<CameraPerspectiveEditor> ();
		}

		void Update ()
		{
			if (Input.GetMouseButton (0)) {
				ray = editor.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, 300f)) {
					if (hit.collider.gameObject == this.gameObject) {
						over = true;
					} else {
						over = false;
					}
				} else {
					over = false;
				}
			} else {
				over = false;
			}
			Vector3 s = transform.localScale;
			float delta = speed * Time.deltaTime;

			if (over) {
				s.x += delta;
				s.y += delta;
			} else {
				s.x -= delta;
				s.y -= delta;
			}
			Clamp (ref s);
			transform.localScale = s;
		}

		void Clamp (ref Vector3 size)
		{
			size.x = size.x <= min ? min : (size.x >= max ? max : size.x);
			size.y = size.y <= min ? min : (size.y >= max ? max : size.y);
		}
	}
}