using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace AppAPITemplate
{
	public class InfoPage : ContentPage
	{
		/*
			Displays all the information for the API menus	
		*/
		static Label One = new Label
		{
			Text = "",
			TextColor = Color.Gray,
			FontSize = 25,
	        HorizontalTextAlignment = TextAlignment.Center,
	        VerticalTextAlignment = TextAlignment.Center,
			HorizontalOptions = LayoutOptions.Fill,
			FontFamily = Device.OnPlatform(
				"Oswald-Bold",
				null,
				null
			), 
		};
		static Label Two = new Label
		{
			Text = "",
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			HorizontalOptions = LayoutOptions.FillAndExpand,
			TextColor = Color.Gray,
			FontFamily = Device.OnPlatform(
				"Oswald-Bold",
				null,
				null
			),
		};
		static Label TitleOne = new Label
		{
			Text = "Days Low",
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			HorizontalOptions = LayoutOptions.FillAndExpand,
			TextColor = Color.Gray,
			FontFamily = Device.OnPlatform(
				"Oswald-Bold",
				null,
				null
			),
		};
		static Label TitleTwo = new Label
		{
			Text = "Days High",
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			HorizontalOptions = LayoutOptions.FillAndExpand,
			TextColor = Color.Gray,
			FontFamily = Device.OnPlatform(
				"Oswald-Bold",
				null,
				null
			),
		};
		static Label Three = new Label
		{
			Text = "",
			WidthRequest = 0,
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			VerticalTextAlignment = TextAlignment.End,
			HorizontalTextAlignment = TextAlignment.Center,
			TextColor = Color.Gray,
			FontFamily = Device.OnPlatform(
				"Oswald-Bold",
				null,
				null
			),
		};
		static Label Four = new Label
		{
			Text = "",
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			HorizontalOptions = LayoutOptions.FillAndExpand,
			TextColor = Color.Gray,
			FontFamily = Device.OnPlatform(
				"Oswald-Bold",
				null,
				null
			),
		};
		static Label Five = new Label
		{
			Text = "",
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			HorizontalOptions = LayoutOptions.FillAndExpand,
			TextColor = Color.Gray,
			FontFamily = Device.OnPlatform(
				"Oswald-Bold",
				null,
				null
			),
		};

		public class InfoPageLayoutChildren : StackLayout
		{
			public InfoPageLayoutChildren()
			{
				Spacing = 2;
				WidthRequest = 0;
				Orientation = StackOrientation.Horizontal;
				Children.Add(new StackLayout
				{
					Spacing = 2,
					WidthRequest = 0,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Children = {
						TitleOne,
						Four,
					}
				});
					
				Children.Add(new StackLayout
				{
					Spacing = 2,
					WidthRequest = 0,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Children = {
						TitleTwo,
						Five
					},
				});
					
			}
		}

		public class InfoPageLayout : StackLayout
		{
			public InfoPageLayout()
			{
				HeightRequest = 70;
				Spacing = 5;
				Orientation = StackOrientation.Vertical;
				Children.Add(
					One
				);
				Children.Add(
					Two
				);
				Children.Add(new InfoPageLayoutChildren());
				Children.Add(
					Three
				);
			}
		}


		public MenuItem currentItem;

		//This holds the Views (it will switch between saying "Loading" and showing the info)
		public InfoPage(MenuItem item)
		{
			currentItem = item;
			Content = new InfoPageLayout();

		}


		protected override async void OnAppearing()
		{
			base.OnAppearing();

			List<string> list = await CallAPI(currentItem);
			Title = list[0];
			One.Text = list[1];
			Three.Text = list[2];
			Four.Text = list[3];
			Five.Text = list[4];
		}

		static async Task<List<string>> CallAPI(MenuItem menuItem)
		{
			string response = await GetResponseFromAPI(menuItem);

			List<string> list = ConstructList(response);

			return list;
		}

		static async Task<string> GetResponseFromAPI(MenuItem menuItem)
		{
			string query = ConstructQuery(menuItem);

			//string results = "[{ One : \"Thomas 2\", Two : \"Is the best 2\", Three : \"And number three \", Four : \"And is four too much\", Five : \"A a high five\"}]"; //string results = call query.
			using (var client = new HttpClient())
			{
				var response = await client.GetStringAsync(query);
				return response.ToString();
			}
		}

		static string ConstructQuery(MenuItem menuItem)
		{
			string query = "https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quote%20where%20symbol%20in%20(%22" + menuItem.Name + "%22)&format=json&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=";

			return query;
		}

		static List<string> ConstructList(string response)
		{

			List<string> items = new List<string>();

			dynamic jsonResult = JsonConvert.DeserializeObject(response); //var jsonResult = Newtonsoft.Json.Linq.JObject.Parse(results);


			string symbol = jsonResult["query"]["results"]["quote"]["symbol"].Value;
			string name = jsonResult["query"]["results"]["quote"]["Name"].Value;
			string daysLow = jsonResult["query"]["results"]["quote"]["DaysLow"].Value;
			string daysHigh = jsonResult["query"]["results"]["quote"]["DaysHigh"].Value;
			string time = jsonResult["query"]["created"].Value.ToString();

			items.Add(symbol);
			items.Add(name);
			items.Add(time);
			items.Add(daysLow);
			items.Add(daysHigh);

			return items;

		}

	
	}
}