using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace Polyjam2020
{
	public class MenuWithPages : Submenu
	{
		[SerializeField] private List<GameObject> pages = new List<GameObject>();
		[SerializeField] private Button nextButton = null;
		[SerializeField] private Button previousButton = null;
		private int currentPageIndex = 0;


		private int CurrentPageIndex
		{
			get
			{
				return currentPageIndex;
			}

			set
			{
				Assert.IsTrue(value >= 0 && value < pages.Count);
				pages[currentPageIndex].gameObject.SetActive(false);
				currentPageIndex = value;
				pages[currentPageIndex].gameObject.SetActive(true);
				previousButton.gameObject.SetActive(currentPageIndex > 0);
				nextButton.gameObject.SetActive(currentPageIndex < pages.Count - 1);
				BackButton.onClick.AddListener(OnBackButtonClicked);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			Assert.IsTrue(pages.Count > 0);
			foreach (var page in pages)
			{
				page.gameObject.SetActive(false);
			}
			Assert.IsNotNull(nextButton);
			nextButton.onClick.AddListener(OnNextButtonClicked);
			Assert.IsNotNull(previousButton);
			previousButton.onClick.AddListener(OnPreviousButtonClicked);
			CurrentPageIndex = 0;
		}

		private void OnNextButtonClicked()
		{
			CurrentPageIndex = Mathf.Min(pages.Count - 1, currentPageIndex + 1);
		}

		private void OnPreviousButtonClicked()
		{
			CurrentPageIndex = Mathf.Max(0, currentPageIndex - 1);
		}

		private void OnBackButtonClicked()
		{
			CurrentPageIndex = 0;
		}
	}
}
