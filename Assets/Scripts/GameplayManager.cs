using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System.Collections;


namespace Polyjam2020
{
	public class GameplayManager : MonoBehaviour
	{
		//[SerializeField] private GameObject defeatScreen = null;
		private int destroyedCitiesLimit = 0;
		private bool isDefeat = false;
		private Graph graph = null;


		public bool IsDefeat => isDefeat;


		private void Awake()
		{
			//Assert.IsNotNull(defeatScreen);
			//defeatScreen.gameObject.SetActive(false);
			graph = Object.FindObjectOfType<Graph>();
			Assert.IsNotNull(graph);
			destroyedCitiesLimit = graph.Nodes.Count / 2;
		}

		private void LateUpdate()
		{
			if (isDefeat)
			{
				return;
			}
			int destroyedCities = 0;
			foreach (var node in graph.Nodes)
			{
				if (node.CurrentHealth <= 0.0f)
				{
					++destroyedCities;
				}
			}
			if (destroyedCities >= destroyedCitiesLimit)
			{
				isDefeat = true;
				//defeatScreen.gameObject.SetActive(true);
				StartCoroutine(GameEndCoroutine());
			}
		}

		private IEnumerator GameEndCoroutine()
		{
			yield return new WaitForSeconds(5);
			SceneManager.LoadScene("MainMenu");
		}
	}
}