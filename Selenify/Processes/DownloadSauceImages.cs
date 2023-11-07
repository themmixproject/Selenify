﻿using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenify.Utility;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using System.Configuration;
using static Selenify.Utility.WebDriverManager;

namespace Selenify.Processes
{

	public class DownloadSauceImages : ProcessBase<DownloadSauceImages.ProcessState>
	{
		public class ProcessState
		{
			public bool isLoggedIn { get; set; }
			public int i { get; set; } = 0;
		};

		public DownloadSauceImages() : base( "Download Sauce Images" ) { }

		public override void Run()
		{
			Uri url = new Uri( "https://www.saucedemo.com/" );
			Driver.Navigate().GoToUrl( url );

			LoginUser();

			State.isLoggedIn = true;
			SaveState();

			LoopThroughProducts();

			Driver.Quit();

			ResetState();
		}

		private void LoginUser()
		{
			AppSettingsSection secrets = Utility.ConfigurationManager.Secrets;
			string siteUsername = secrets.Settings["site_username"].Value;
			string sitePassword = secrets.Settings["site_password"].Value;

			Utility.ConfigurationManager.UnloadSecretsConfig();

			WebDriverWait loginWait = new WebDriverWait( Driver, TimeSpan.FromSeconds( 5 ) );
			loginWait.Until( ExpectedConditions.ElementIsVisible( By.Id( "user-name" ) ) );

			IWebElement usernameField = Driver.FindElement( By.Id( "user-name" ) );
			usernameField.SendKeys( siteUsername );

			IWebElement passwordField = Driver.FindElement( By.Id( "password" ) );
			passwordField.SendKeys( sitePassword );

			IWebElement loginButton = Driver.FindElement( By.Id( "login-button" ) );
			loginButton.Click();

			WebDriverWait inventoryHeaderWait = new WebDriverWait( Driver, TimeSpan.FromSeconds( 5 ) );
			inventoryHeaderWait.Until(
				ExpectedConditions.ElementExists(
					By.XPath( "//*[@id='header_container']/div[1]/div[2]/div" )
				)
			);
		}

		private void LoopThroughProducts()
		{
			ReadOnlyCollection<IWebElement> inventoryItems = GetInventoryItems();

			for (; State.i < inventoryItems.Count; State.i++)
			{
				IWebElement inventoryItem = inventoryItems[State.i];

				if (ElementIsStale( inventoryItem ))
				{
					inventoryItems = GetInventoryItems();
					inventoryItem = inventoryItems[State.i];
				}

				IWebElement inventoryItemLink = inventoryItem
					.FindElement( By.ClassName( "inventory_item_label" ) )
					.FindElement( By.TagName( "a" ) );
				inventoryItemLink.Click();

				SaveInventoryItemImage();

				Driver.Navigate().Back();

				Thread.Sleep( 500 );

				SaveState();
			}
		}

		private void SaveInventoryItemImage()
		{
			IWebElement inventoryImage = Driver
				.FindElement( By.Id( "inventory_item_container" ) )
				.FindElement( By.ClassName( "inventory_details_img" ) );

			string imageSource = inventoryImage.GetAttribute( "src" );
			string imageName = Path.GetFileName( imageSource );

			CreateDownloadsFolder();

			string savePath = GetProjectDirectoryPath() + "\\Downloads";
			DownloadFileToDirectory( imageSource, savePath, imageName );
		}

		private void CreateDownloadsFolder()
		{
			string path = GetProjectDirectoryPath() + "\\Downloads";

			if (!Directory.Exists( path ))
			{
				Directory.CreateDirectory( path );
			}
		}

		private string GetProjectDirectoryPath()
		{
			return Directory.GetParent(
				Directory.GetCurrentDirectory()!
			)!.Parent!.Parent!.FullName;
		}

		private async void DownloadFileToDirectory( string fileUrl, string path, string fileName )
		{
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync( fileUrl );

				using (Stream memoryStream = await response.Content.ReadAsStreamAsync())
				{
					string savePath = Path.Combine( path, fileName );
					using (FileStream fileStream = File.Create( savePath ))
					{
						memoryStream.CopyTo( fileStream );
					}
				}
			}
		}

		private ReadOnlyCollection<IWebElement> GetInventoryItems()
		{
			return Driver
				.FindElement( By.Id( "inventory_container" ) )
				.FindElements( By.CssSelector( ".inventory_item" ) );
		}

		private bool ElementIsStale( IWebElement element )
		{
			try
			{
				bool _ = element.Displayed;
				return false;
			}
			catch (StaleElementReferenceException)
			{
				return true;
			}
		}
	}
}