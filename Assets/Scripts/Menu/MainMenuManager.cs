using UnityEngine;


namespace Polyjam2020
{
	public class MainMenuManager : MenuManager
	{
		protected override void ExitFromMenu()
		{
			Application.Quit();
		}
	}
}