using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System.Collections;


namespace Polyjam2020
{
	public class GameplayManager : MonoBehaviour
	{
		private float score = 0.0f;
		private int destroyedCitiesLimit = 0;
		private bool isDefeat = false;
		private Graph graph = null;
		public System.Action OnDefeat;
		private float timeBetweenNodeDamage = 5.0f;
		private float nodeDamageCooldown = 2.0f;


		public float Score => score;

		public bool IsDefeat
		{
			get => isDefeat;

			private set
			{
				isDefeat = value;
				if (isDefeat)
				{
					OnDefeat?.Invoke();
				}
			}
		}


		private void Awake()
		{
			graph = Object.FindObjectOfType<Graph>();
			Assert.IsNotNull(graph);
			destroyedCitiesLimit = graph.Nodes.Count / 2;
			nodeDamageCooldown = timeBetweenNodeDamage;
		}

		private void LateUpdate()
		{
			if (IsDefeat)
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SceneManager.LoadScene("MainMenu");
			}
			int destroyedCities = 0;
			foreach (var node in graph.Nodes)
			{
				if (node.GetComponent<HealthComponent>().CurrentValue <= 0)
				{
					++destroyedCities;
				}
			}
			if (destroyedCities >= destroyedCitiesLimit)
			{
				IsDefeat = true;
				StartCoroutine(GameEndCoroutine());
				return;
			}			
			score += Time.deltaTime;
		}

		private IEnumerator GameEndCoroutine()
		{
			yield return new WaitForSeconds(5);
			SceneManager.LoadScene("MainMenu");
		}
	}
}