using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.PlatformConfiguration;
using MySqlConnector;
using System.Data.Common;
using System.Text;
using static ZXing.QrCode.Internal.Mode;
#if ANDROID
using Android.OS;
#endif
namespace PlanetPedia;

public partial class card : ContentPage
{
	string file = "";
	string file_moons = "";
	string source = "";
	string vid_url = "";
	string obj_url = "";
	bool filemode = false;
	bool anim = false;
	bool opened = false;
	int id = 0;

	public card(string file_get, string file_moons_get, bool filemode_get, int id_get)
	{
        InitializeComponent();
        System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(ppp);
        List<VisualElement> elements = new List<VisualElement>() { title, img, block1, block2, block3, block4, block5, block6, block7, block8};
        file = file_get;
		file_moons = file_moons_get;
		anim = true;
		filemode = filemode_get;
		id = id_get;

        foreach (VisualElement element in elements) element.Opacity = 0;

        propertybox.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
		aboutbox.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        uniquebox.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
		moonsbox.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
		picbox.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
		vidbox.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
		box3d.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
		tests.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);

		if (filemode) read();
	}

	private async void database()
	{
		try
		{
			if(opened)
			{
				wiki.IsVisible = false;
                bool vid_enabled = Preferences.Get("video", true);
                bool obj_enabled = Preferences.Get("obj", true);
                Dictionary<string, string> gallery = new Dictionary<string, string>();
				piclist.Children.Clear();
				using (var conn = new MySqlConnection(SQLClass.CONNECTION_STRING))
				{
					await conn.OpenAsync();
					MySqlCommand cmd = new MySqlCommand($"SELECT * FROM pages WHERE id = '{id}'", conn);
					try
					{
						DbDataReader reader = cmd.ExecuteReader();
						while (reader.Read())
						{
							title.Text = reader.GetString(1);
							img.Source = ImageSource.FromUri(new Uri(reader.GetString(2)));

							Label property = new Label();
							property.Text = reader.GetString(3);
							property.FontSize = 18;
							property.Margin = new Thickness(15, 7);
							property.TextColor = Colors.Black;
							propertylist.Children.Add(property);

							about.Text = reader.GetString(4);

							Label unique = new Label();
							unique.Text = reader.GetString(5);
							unique.FontSize = 18;
							unique.Margin = new Thickness(15, 7);
							unique.TextColor = Colors.Black;
							uniquelist.Children.Add(unique);

							if (!string.IsNullOrEmpty(reader.GetString(6)))
							{
								List<string> gallery_pairs = new List<string>();
								if (reader.GetString(6).Contains(";")) gallery_pairs = reader.GetString(6).Split(";").ToList();
								else gallery_pairs.Add(reader.GetString(6));
								try
								{
									foreach (string pair in gallery_pairs) gallery.Add(pair.Split("|")[0], pair.Split("|")[1]);
								}
								catch { }
								foreach (KeyValuePair<string, string> pair in gallery)
								{
									Border frame = new Border();
									frame.Margin = new Thickness(5);
									frame.Stroke = Colors.Black;
									piclist.Children.Add(frame);

									VerticalStackLayout vertical = new VerticalStackLayout();
									frame.Content = vertical;

									Image pic = new Image();
									pic.Source = ImageSource.FromUri(new Uri(pair.Key));
									pic.IsAnimationPlaying = true;
									pic.MaximumWidthRequest = 300;
									pic.HeightRequest = 300;
									pic.Aspect = Aspect.AspectFit;
									var tapGesture = new TapGestureRecognizer();
									tapGesture.Tapped += (s, e) =>
									{
										OnImageTapped(s, e, pair.Key, pair.Value);
									};
									pic.GestureRecognizers.Add(tapGesture);
									vertical.Children.Add(pic);

									Label desc = new Label();
									desc.Text = pair.Value;
									desc.HorizontalOptions = LayoutOptions.Center;
									desc.FontAttributes = FontAttributes.Italic;
									desc.FontSize = 12;
									desc.TextColor = Colors.Black;
									desc.Margin = new Thickness(5, 0);
									vertical.Children.Add(desc);
								}
							}

							vid_url = reader.GetString(7);
							obj_url = reader.GetString(8);

							if (vid_enabled) web.Source = vid_url;
							else web.Source = "disabledvid.html";

							if (obj_enabled) web3d.Source = obj_url;
							else web3d.Source = "disabled3d.html";

							full.IsVisible = vid_enabled;
							web3d_full.IsVisible = obj_enabled;

							Label nomoons = new Label();
							nomoons.Text = "У данного космического тела отсутствуют спутники...";
							nomoons.FontSize = 18;
							nomoons.FontAttributes = FontAttributes.Italic;
							nomoons.Margin = new Thickness(15, 15, 0, 20);
							nomoons.TextColor = Colors.Black;
							moonslist.Children.Clear();
							moonslist.Children.Add(nomoons);
						}
						reader.Close();
						Console.WriteLine();
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
				}
            }
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
            await Task.Delay(5000);
			database();
        }
	}

    async private void read()
	{
		bool vid_enabled = Preferences.Get("video", true);
        bool obj_enabled = Preferences.Get("obj", true);
        List<string> data = new List<string>();
		List<string> properties;
		List<string> facts;
		List<string> tests;
		Dictionary<string, string> gallery = new Dictionary<string, string>();
        using var stream = await FileSystem.OpenAppPackageFileAsync(file);
        using var reader = new StreamReader(stream);

		string line;
		while((line = await reader.ReadLineAsync()) != null)
		{
			data.Add(line);
		}

		title.Text = data[0];
		img.Source = data[1];
		properties = data[2].Split(";").ToList();
        about.Text = data[3];
        facts = data[4].Split(";").ToList();
		List<string> gallery_pairs = data[5].Split(";").ToList();
		try
		{
            foreach (string pair in gallery_pairs) gallery.Add(pair.Split(":")[0], pair.Split(":")[1]);
        }
		catch { }
		vid_url = data[6];
		obj_url = data[7];
		tests = data[8].Split(";").ToList();
        source = data[9];

		foreach(string prop in properties)
		{
			Label property = new Label();
			property.Text = prop;
			property.FontSize = 18;
			property.Margin = new Thickness(15, 7);
			property.TextColor = Colors.Black;
			propertylist.Children.Add(property);
		}

		int j = 1;
        foreach (string fact in facts)
        {
            Label unique = new Label();
            unique.Text = $"{j}. {fact}";
            unique.FontSize = 18;
            unique.Margin = new Thickness(15, 7);
			unique.TextColor = Colors.Black;
            uniquelist.Children.Add(unique);
			j++;
        }
		foreach(KeyValuePair<string, string> pair in gallery)
		{
			Border frame = new Border();
			frame.Margin = new Thickness(5);
			frame.Stroke = Colors.Black;
			piclist.Children.Add(frame);

			VerticalStackLayout vertical = new VerticalStackLayout();
			frame.Content = vertical;

			Image pic = new Image();
			pic.Source = ImageSource.FromFile(pair.Key);
			pic.MaximumWidthRequest = 300;
			pic.HeightRequest = 300;
			pic.IsAnimationPlaying = true;
			pic.Aspect = Aspect.AspectFit;
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) =>
            {
                OnImageTapped(s, e, pair.Key, pair.Value );
            };
            pic.GestureRecognizers.Add(tapGesture);
            vertical.Children.Add(pic);

			Label desc = new Label();
			desc.Text = pair.Value;
			desc.HorizontalOptions = LayoutOptions.Center;
			desc.FontAttributes = FontAttributes.Italic;
			desc.FontSize = 12;
			desc.TextColor = Colors.Black;
			desc.Margin = new Thickness(5,0);
			vertical.Children.Add(desc);
		}
		if (tests[0] != "")
		{
			foreach (string test in tests)
			{
				string[] els = test.Split(':');
				Grid testbox = new Grid();
				testbox.HeightRequest = 100;
				testbox.Margin = new Thickness(10);
				testbox.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgba(187, 187, 187, 255);
				tests_stack.Add(testbox);

				Image image = new Image();
				image.Source = "testico.png";
				image.HeightRequest = 80;
				image.HorizontalOptions = LayoutOptions.Start;
				image.Margin = new Thickness(15, 0, 0, 0);
				testbox.Add(image);

				Label name = new Label();
				name.Text = els[1];
				name.TextColor = Colors.Black;
				name.VerticalOptions = LayoutOptions.Center;
				name.HorizontalTextAlignment = TextAlignment.Start;
				name.VerticalTextAlignment = TextAlignment.Center;
				name.HeightRequest = 80;
				name.FontAttributes = FontAttributes.Bold;
				name.Margin = new Thickness(110, 0, 10, 0);
				name.FontSize = 24;
				var tap = new TapGestureRecognizer();
				tap.Tapped += (s, e) =>
				{
					test_go(s, e, els[1], els[0]);
				};
				name.GestureRecognizers.Add(tap);
				testbox.Add(name);
			}
		}

		if (vid_enabled) web.Source = vid_url;
		else web.Source = "disabledvid.html";

        if (obj_enabled) web3d.Source = obj_url;
        else web3d.Source = "disabled3d.html";

        full.IsVisible = vid_enabled;
        web3d_full.IsVisible = obj_enabled;

        if (data[0] == "Сатурн") img.WidthRequest = 400;

		wikibox.Children.Remove(wiki);

        List<string> datamoon = new List<string>();
        using var streammoon = await FileSystem.OpenAppPackageFileAsync(file_moons);
        using var readermoon = new StreamReader(streammoon);

        string linemoon;
        while ((linemoon = await readermoon.ReadLineAsync()) != null)
        {
            datamoon.Add(linemoon);
        }

		if(datamoon.Count == 0)
		{
			Label nomoons = new Label();
			nomoons.Text = "У данного космического тела отсутствуют спутники...";
			nomoons.FontSize = 18;
			nomoons.FontAttributes = FontAttributes.Italic;
			nomoons.Margin = new Thickness(15, 15, 0, 20);
			nomoons.TextColor = Colors.Black;
			moonslist.Children.Add(nomoons);
		}
		for(int i = 0; i < datamoon.Count; i += 2)
		{
			Grid box = new Grid();
			box.Margin = new Thickness(5,0,0,5);
			moonslist.Children.Add(box);

			BoxView bg = new BoxView();
			bg.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgba(187,187,187,255);
			bg.HeightRequest = 100;
			bg.CornerRadius = 0;
			bg.HorizontalOptions = LayoutOptions.Fill;
			bg.Margin = new Thickness(5,0,5,0);
			box.Children.Add(bg);

			HorizontalStackLayout horizontal = new HorizontalStackLayout();
			horizontal.HorizontalOptions = LayoutOptions.Start;
			box.Children.Add(horizontal);

			Image img = new Image();
			img.HeightRequest = 80;
			img.WidthRequest = 80;
			img.Source = datamoon[i + 1];
			img.Margin = new Thickness(10, 0, 10, 0);
			horizontal.Children.Add(img);

			Button title = new Button();
			title.Text = datamoon[i];
			title.FontSize = 20;
			title.VerticalOptions = LayoutOptions.Center;
			title.TextColor = Colors.Black;
			title.FontAttributes = FontAttributes.Bold;
			title.BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent;
            title.Clicked += Title_Clicked;
			horizontal.Children.Add(title);
		}

#if ANDROID
		try
		{
		string dir = "";
		var context = Android.App.Application.Context;

        // Для Android 10+ (API 29+) используем Scoped Storage
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
        {
            dir = context.GetExternalFilesDir(null)?.AbsolutePath;
        }
        else
        {
            // Для более старых версий
            var externalPath = Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryDocuments)?.AbsolutePath;
            var packageName = context.PackageName;
            dir =  Path.Combine(externalPath, "Android", "data", packageName);
        }

		try
    {
        var directoryPath = dir;
        
        if (!string.IsNullOrEmpty(directoryPath))
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Папка создана: {directoryPath}");
            }
            else
            {
                Console.WriteLine($"Папка уже существует: {directoryPath}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при создании папки: {ex.Message}");
    }

		var basePath = dir;
		var fullPath = Path.Combine(basePath, "offline");

		if(File.Exists(Path.Combine(fullPath, file.Split(".")[0] + ".mp4"))) 
		{
			string videoPath = Path.Combine(fullPath, file.Split(".")[0] + ".mp4");
            var htmlContent = $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body {{ margin: 0; padding: 0; background: transparent; }}
            video {{ width: 100%; height: 100%; background: black; }}
        </style>
    </head>
    <body>
        <video controls>
            <source src='file://{videoPath}' type='video/mp4'>
            Your browser does not support the video tag.
        </video>
    </body>
    </html>";

            var htmlSource = new HtmlWebViewSource { Html = htmlContent };
            web.Source = htmlSource;
		}
	}
	catch
	{
		//
	}
#elif WINDOWS
        string userFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        if (File.Exists(Path.Combine(userFolder, "PlanetPedia", file.Split(".")[0] + ".mp4")))
        {
            string videoPath = Path.Combine(userFolder, "PlanetPedia" , file.Split(".")[0] + ".mp4");
            var htmlContent = $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body {{ margin: 0; padding: 0; background: transparent; }}
            video {{ width: 100%; height: 100%; background: black; }}
        </style>
    </head>
    <body>
        <video controls>
            <source src='file://{videoPath}' type='video/mp4'>
            Your browser does not support the video tag.
        </video>
    </body>
    </html>";

            var htmlSource = new HtmlWebViewSource { Html = htmlContent };
            web.Source = htmlSource;
        }
#endif

        wikibox.Children.Add(wiki);
    }

    protected async override void OnAppearing()
    {
		try
		{
			if (!filemode)
			{
				opened = true;
                database();
            }
		}
		catch { }
        List<VisualElement> elements = new List<VisualElement>() { title, img, block1, block2, block3, block4, block5, block6, block7, block8 };
        base.OnAppearing();

        //плавное проявление
        await Task.Delay(300);
        foreach (VisualElement element in elements)
		{
			float tr = anim ? 0f : 1f;
			while(tr < 1)
			{
				tr += 0.1f;
				element.Opacity = tr;
				await Task.Delay(10);
			}
			await Task.Delay(10);
		}
		anim = false;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
		try
		{
			if (!filemode)
			{
				opened = false;
			}
		}
		catch { }
    }

    private void test_go(object? sender, TappedEventArgs e, string title, string dir)
    {
		Navigation.PushAsync(new test(title, dir));
    }

    private void Title_Clicked(object? sender, EventArgs e)
    {
		Dictionary<string, string> moons = new Dictionary<string, string>()
		{
			{"Луна","moon"},
			{"Фобос","fobos"},
			{"Деймос","deimos"},
			{"Ио","io"},
			{"Каллисто","callisto"},
			{"Ганимед","ganymede"},
			{"Европа","europa"},
			{"Титан","titan"},
			{"Мимас","mimas"},
			{"Энцелад","enceladus"},
			{"Рея","reya"},
			{"Япет","yapet"},
			{"Миранда","miranda"},
			{"Титания","titania"},
			{"Оберон","oberon"},
			{"Тритон","tritone"}
		};
		Navigation.PushAsync(new card(moons[(sender as Button).Text] + ".txt","sunmoons.txt", true, 0));
    }

    private void wiki_Clicked(object sender, EventArgs e)
    {
        wiki.SetAppThemeColor(Label.TextColorProperty, Color.FromArgb("551A8B"), Color.FromArgb("db96f2"));
        Navigation.PushAsync(new web(source));
    }
	private void OnImageTapped(object sender, EventArgs e, string path, string desc)
	{
		Navigation.PushAsync(new pictureview(path, desc, title.Text));
	}

    private void full_Clicked(object sender, EventArgs e)
    {
		Navigation.PushModalAsync(new web(vid_url, true));
    }

    private void vid_SizeChanged(object sender, EventArgs e)
    {
		pad.Margin = new Thickness(10,vid.Width / 2,10,9);
    }

    private void web3d_full_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new web(obj_url));
    }

    private void third_SizeChanged(object sender, EventArgs e)
    {
        pad2.Margin = new Thickness(10, third.Width / 2, 10, 9);
    }
}