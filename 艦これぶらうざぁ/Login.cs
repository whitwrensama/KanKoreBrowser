﻿using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 艦これぶらうざぁ
{
	public partial class Login : Form
	{
		public Login()
		{
			InitializeComponent();

			// タイマーのinterval
			timer1.Interval = 3000;

			// xml読み込み
			Settings.LoadFromXmlFile();
		}

		private async void Login_Load(object sender, EventArgs e)
		{
			// ログインページへ
			geckoWebBrowser1.Navigate("https://www.dmm.com/my/-/login/=/path=Sg9VTQFXDFcXFl5bWlcKGExKUVdUXgFNEU0KSVMVR28MBQ0BUwJZBwxK/");

			try
			{
				// 繰り返し 条件は常にTrue
				while (true)
				{
					// 非同期で0.5秒間待機
					await Task.Delay(500);

					if (geckoWebBrowser1.DomDocument.ReadyState.ToString() == "complete")
					{
						// Window位置
						geckoWebBrowser1.Window.ScrollTo(135, 200);

						// スクロールバー隠し
						geckoWebBrowser1.Navigate("javascript:(function(){document.body.style.overflow = \"hidden\"})();");

						// タイトルテキスト変更
						Text = "Login (Step 1/3)";

						// タイマー開始
						timer1.Start();

						// ループ抜け出し
						break;
					}
				}
			}
			catch (Exception ex)
			{
				// Debug
				Console.WriteLine(ex.ToString());
			}
		}

		private void Close_Click(object sender, EventArgs e)
		{
			// タイマーストップ
			timer1.Stop();

			// 閉じる
			Close();
		}

		private async void timer1_Tick(object sender, EventArgs e)
		{
			// ログイン処理
			if (geckoWebBrowser1.Url.ToString() == "http://www.dmm.com/netgame/social/-/gadgets/=/app_id=854854/")
			{
				// Javascript実行
				geckoWebBrowser1.Navigate("javascript:(function(){location.href=$(\"iframe\").attr(\"src\")})();");

				// タイトルテキスト変更
				Text = "Login (Step 2/3)";

				while (true)
				{
					// 非同期で0.5秒間待機
					await Task.Delay(500);

					if (geckoWebBrowser1.DomDocument.ReadyState.ToString() == "complete")
					{
						// Javascript実行
						geckoWebBrowser1.Navigate("javascript:(function(){location.href=$(\"embed\").attr(\"src\")})();");

						// タイトルテキスト変更
						Text = "Login (Step 3/3)";

						// ループ抜け出し
						break;
					}
				}

				// 非同期で1秒間待機
				await Task.Delay(1000);

				// xmlにtokenurlを指定
				Settings.Instance.tokenurl_s = geckoWebBrowser1.Url.ToString();

				// xmlに保存
				Settings.SaveToXmlFile();

				// タイマー終了
				timer1.Stop();

				// 閉じる
				Close();
			}
		}

		private void Logout_Click(object sender, EventArgs e)
		{
			// ログアウトURL
			geckoWebBrowser1.Navigate("http://www.dmm.com/my/-/login/logout/");
		}

		private async void GameLogin_Click(object sender, EventArgs e)
		{
			// 艦これゲームURL(未ログインの場合はログイン画面)
			geckoWebBrowser1.Navigate("https://www.dmm.com/my/-/login/=/path=Sg9VTQFXDFcXFl5bWlcKGExKUVdUXgFNEU0KSVMVR28MBQ0BUwJZBwxK/");

			// 非同期で1秒間待機
			await Task.Delay(1000);

			// Window位置
			geckoWebBrowser1.Window.ScrollTo(135, 200);

			// スクロールバー隠し
			geckoWebBrowser1.Navigate("javascript:(function(){document.body.style.overflow = \"hidden\"})();");

			// タイトルテキスト変更
			Text = "Login (Step 1/3)";
		}

		private void Login_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// タイマーストップ
			timer1.Stop();
		}
	}
}