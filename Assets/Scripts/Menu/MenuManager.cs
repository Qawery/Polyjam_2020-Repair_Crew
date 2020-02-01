using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public abstract class MenuManager : MonoBehaviour
	{
		[SerializeField] private Submenu baseMenu = null;
		private List<Submenu> presentSubmenus = new List<Submenu>();
		private Stack<Submenu> submenuStack = new Stack<Submenu>();


		protected virtual void Awake()
		{
			Assert.IsNotNull(baseMenu, "Missing baseMenu on: " + gameObject.name);
			presentSubmenus.AddRange(GetComponentsInChildren<Submenu>(true));
			foreach (var presentSubmenu in presentSubmenus)
			{
				presentSubmenu.gameObject.SetActive(true);
			}
			foreach (var presentSubmenu in presentSubmenus)
			{
				presentSubmenu.gameObject.SetActive(false);
			}
			NavigateToMenu(baseMenu);
		}

		private void NavigateToMenu(Submenu submenu)
		{
			Assert.IsNotNull(submenu, "Null submenu to navigate to on: " + gameObject.name);
			Assert.IsTrue(presentSubmenus.Contains(submenu), "Submenu not present in collected submenus on: " + gameObject.name);
			Assert.IsFalse(submenuStack.Contains(submenu), "Submenu already present on stack on: " + gameObject.name);
			if (submenuStack.Count > 0)
			{
				DeactivatAndUnbindSubmenu(submenuStack.Peek());
			}
			submenuStack.Push(submenu);
			ActivatAndBindSubmenu(submenuStack.Peek());
		}

		private void OnBackButtonClicked()
		{
			DeactivatAndUnbindSubmenu(submenuStack.Peek());
			submenuStack.Pop();
			if (submenuStack.Count > 0)
			{
				ActivatAndBindSubmenu(submenuStack.Peek());
			}
			else
			{
				ExitFromMenu();
			}
		}

		private void ActivatAndBindSubmenu(Submenu submenu)
		{
			submenu.gameObject.SetActive(true);
			submenu.BackButton.onClick.AddListener(OnBackButtonClicked);
			if (submenu is SubmenuWithNavigationToDifferentSubmenu)
			{
				(submenu as SubmenuWithNavigationToDifferentSubmenu).RequestTransitionToSubmenu += NavigateToMenu;
			}
		}

		private void DeactivatAndUnbindSubmenu(Submenu submenu)
		{
			submenu.gameObject.SetActive(false);
			submenu.BackButton.onClick.RemoveListener(OnBackButtonClicked);
			if (submenu is SubmenuWithNavigationToDifferentSubmenu)
			{
				(submenu as SubmenuWithNavigationToDifferentSubmenu).RequestTransitionToSubmenu -= NavigateToMenu;
			}
		}

		protected abstract void ExitFromMenu();
	}
}