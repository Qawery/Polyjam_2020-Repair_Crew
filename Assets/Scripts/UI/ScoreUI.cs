using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class ScoreUI : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI scoreText = null;
		private GameplayManager gameplayManager = null;


		private void Awake()
		{
			Assert.IsNotNull(scoreText);
			gameplayManager = FindObjectOfType<GameplayManager>();
			Assert.IsNotNull(gameplayManager);
		}

		private void LateUpdate()
		{
			scoreText.text = "Score: " + gameplayManager.Score.ToString("#.#");
		}
	}
}