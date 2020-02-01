using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class Submenu : MonoBehaviour
	{
		[SerializeField] private Button backButton = null;


		public Button BackButton
		{
			get
			{
				return backButton;
			}
		}


		protected virtual void Awake()
		{
			Assert.IsNotNull(backButton, "Missing backButton on: " + gameObject.name);
		}
	}
}