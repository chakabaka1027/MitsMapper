using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DISUnity.DataType.Enums;

namespace DISUnity.UI
{
	[RequireComponent(typeof(Dropdown))]
	public class PDUTypeDropdown : MonoBehaviour
	{
		public Dropdown DropDownUI
		{
			get
			{
				if (_DropdownUI == null)
					_DropdownUI = GetComponent<Dropdown>();
				return _DropdownUI;
			}
		}
		private Dropdown _DropdownUI;

		[ContextMenu("Populate Options")]
		void Populate()
		{
			var names = System.Enum.GetNames(typeof(PDUType));
			var options = new List<Dropdown.OptionData>(names.Length);
			foreach (var name in names)
			{
				options.Add(new Dropdown.OptionData(name));
			}
			DropDownUI.options = options;
		}
	}
}