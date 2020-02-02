using UnityEngine;


namespace Polyjam2020
{
	public class SelectionCircle : MonoBehaviour
	{
		private Vector3 originalScale = Vector3.one;
		private float amplitudeInPercentages = 0.1f;
		private float changeSpeedInPercentagesPerSecond = 0.2f;
		private Vector2 changeSpeedInScaleUnitsPerSecond = Vector2.one;
		private bool increasing = true;


		private void Awake()
		{
			originalScale = transform.localScale;
			changeSpeedInScaleUnitsPerSecond = originalScale * changeSpeedInPercentagesPerSecond;
		}

		private void Update()
		{
			if (increasing)
			{
				transform.localScale += new Vector3(changeSpeedInScaleUnitsPerSecond.x, changeSpeedInScaleUnitsPerSecond.y, 0.0f) * Time.deltaTime;
				if (transform.localScale.x >= originalScale.x + (originalScale.x * amplitudeInPercentages))
				{
					transform.localScale = originalScale + (originalScale * amplitudeInPercentages);
					increasing = false;
				}
			}
			else
			{
				transform.localScale -= new Vector3(changeSpeedInScaleUnitsPerSecond.x, changeSpeedInScaleUnitsPerSecond.y, 0.0f) * Time.deltaTime;
				if (transform.localScale.x <= originalScale.x - (originalScale.x * amplitudeInPercentages))
				{
					transform.localScale = originalScale - (originalScale * amplitudeInPercentages);
					increasing = true;
				}
			}
		}
	}
}