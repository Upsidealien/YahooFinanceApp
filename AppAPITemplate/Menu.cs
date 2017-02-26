﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppAPITemplate
{
	public class Menu : ContentPage
	{
		protected readonly ListView list = new ListView
		{
			/*
				- Starts by just saying "Loading..."
				- When the API call is complete it contains the MenuCells.
			*/
			ItemsSource = new List<MenuItem>
			{
				new MenuItem { Name = "Please Wait", Description = "..." },
			},
			ItemTemplate = new DataTemplate(typeof(MenuCell)),
			RowHeight = MenuCell.RowHeight,
		};
	}
}

