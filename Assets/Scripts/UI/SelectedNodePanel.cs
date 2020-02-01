using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Polyjam2020
{
	public class SelectedNodePanel : MonoBehaviour
	{

		[SerializeField] private Text nameText;
		private void Awake()
		{
			var nodeController = FindObjectOfType<NodeController>();
			nodeController.OnSelectedNodeChanged += OnSelectedNodeChanged;
			gameObject.SetActive(false);
		}

		private void OnSelectedNodeChanged(Node node)
		{
			if (node != null)
			{
				string text = node.name;
				if (node.GetComponent<UnitFactory>())
				{
					text += "[factory]";
					//TODO: display factory-specific stuff
				}

				if (node.GetComponent<Scrapyard>())
				{
					text += "[scrapyard]";
					//TODO: display scrapyard-specific stuff
				}

				nameText.text = text;
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}
