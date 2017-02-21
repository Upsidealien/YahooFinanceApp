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
		static Label CompanyName = new Label
		{
			Text = "",
			TextColor = Color.Black,
			FontSize = 35,
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
		static Label TitleLow = new Label
		{
			Text = "Days Low",
			FontSize = 25,
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			HorizontalOptions = LayoutOptions.FillAndExpand,
			TextColor = Color.Black,
			FontFamily = Device.OnPlatform(
				"Oswald-Bold",
				null,
				null
			),
		};
		static Label TitleHigh = new Label
		{
			Text = "Days High",
			FontSize = 25,
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			HorizontalOptions = LayoutOptions.FillAndExpand,
			TextColor = Color.Black,
			FontFamily = Device.OnPlatform(
				"Oswald-Bold",
				null,
				null
			),
		};
		static Label MarketCap = new Label
		{
			Text = "",
			FontSize = 35,
			HorizontalOptions = LayoutOptions.FillAndExpand,
			HorizontalTextAlignment = TextAlignment.Center,
			TextColor = Color.Gray,
			FontFamily = Device.OnPlatform(
				"Oswald-Bold",
				null,
				null
			),
		};
		static Label DayLow = new Label
		{
			Text = "",
			FontSize = 20,
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
		static Label DayHigh = new Label
		{
			Text = "",
			FontSize = 20,
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
				Padding = new Thickness(30, 50, 30, 10);
				Children.Add(new StackLayout
				{
					Spacing = 2,
					WidthRequest = 0,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Children = {
						TitleLow,
						DayLow,
					}
				});
					
				Children.Add(new StackLayout
				{
					Spacing = 2,
					WidthRequest = 0,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Children = {
						TitleHigh,
						DayHigh
					},
				});
					
			}
		}

		public class InfoPageLayout : StackLayout
		{
			public InfoPageLayout()
			{
				//HeightRequest = 140;
				Spacing = 5;
				Orientation = StackOrientation.Vertical;
				Padding = new Thickness(30, 10, 30, 10);
				Children.Add(
					CompanyName
				);
				Children.Add(
					MarketCap
				);
				Children.Add(new InfoPageLayoutChildren());
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
			CompanyName.Text = list[1];
			MarketCap.Text = list[2];
			DayLow.Text = list[3];
			DayHigh.Text = list[4];
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
			string marketcap = jsonResult["query"]["results"]["quote"]["MarketCapitalization"].Value.ToString();

			items.Add(symbol);
			items.Add(name);
			items.Add(marketcap);
			items.Add(daysLow);
			items.Add(daysHigh);

			return items;

		}

	
	}
}