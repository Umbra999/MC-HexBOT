﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MCHexBOT.Utils.Data
{
	public class Modifier
	{
		public UUID Uuid;
		public double Amount;
		public ModifierMode Operation;

		public Modifier() { }

		public Modifier(UUID uuid, double amount, ModifierMode mode)
		{
			Uuid = uuid;
			Amount = amount;
			Operation = mode;
		}
	}
}
