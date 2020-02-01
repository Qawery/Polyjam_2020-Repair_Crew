using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class GameOverScreen : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI score = null;
		private GameplayManager gameplayManager = null;


		private void Awake()
		{
			Assert.IsNotNull(score);
			gameplayManager = Object.FindObjectOfType<GameplayManager>();
			Assert.IsNotNull(gameplayManager);
			gameplayManager.OnDefeat += () =>
				{
					gameObject.SetActive(true);
					
					score.text = "Score: " + gameplayManager.Score.ToString("#.#");
				};
			if (!gameplayManager.IsDefeat)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
