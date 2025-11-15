using System.Net.NetworkInformation;

namespace PlanetPedia;

public partial class settings : ContentPage
{
    bool video;
    bool obj;
    bool mus;
    bool vib;
    bool isrun = false;
    bool initialized = false;
    public settings()
	{
		InitializeComponent();
        #if ANDROID
            vid_desc.Margin = new Thickness(5,13,0,0);
            obj_desc.Margin = new Thickness(5,13,0,0);
            music_desc.Margin = new Thickness(5,13,0,0);
            vibro_desc.Margin = new Thickness(5,13,0,0);
        #endif
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        update();
        video = Preferences.Get("video", true);
        videos.IsToggled = video;
        obj = Preferences.Get("obj", true);
        objs.IsToggled = video;
        mus = Preferences.Get("music", true);
        music.IsToggled = mus;
        vib = Preferences.Get("vibro", true);
        vibro.IsToggled = vib;

        string tests = Preferences.Get("tests","");
        List<string> unique = new List<string>();
        foreach(string test in tests.Split(';'))
        {
            if(!unique.Contains(test)) unique.Add(test);
        }
        unique.RemoveAt(0);
        count.Text = $"Пройдено тестов: {unique.Count}";
        foreach (string test in unique)
        {
            HorizontalStackLayout hor = new HorizontalStackLayout();
            testsview.Add(hor);

            Image image = new Image();
            image.Source = "testico.png";
            image.HeightRequest = 50;
            image.Margin = new Thickness(20);
            hor.Add(image);

            Label name = new Label();
            name.Text = test;
            name.TextColor = Colors.Black;
            name.FontSize = 24;
            name.FontAttributes = FontAttributes.Bold;
            name.VerticalOptions = LayoutOptions.Center;
            hor.Add(name);
        }
        initialized = true;
    }
    private void back_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new MainPage());
    }

    private void wikib_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new web("https://ru.wikipedia.org"));
    }

    private void ton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("ton618.txt","sunmoons.txt"));
    }

    private void cosmob_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new web("https://dzen.ru/off_kosmo?tab=longs"));
    }

    private void video_Toggled(object sender, ToggledEventArgs e)
    {
        video = videos.IsToggled;
        Preferences.Set("video", video);
    }

    private void objs_Toggled(object sender, ToggledEventArgs e)
    {
        obj = objs.IsToggled;
        Preferences.Set("obj", obj);
    }

    private void objb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new web("https://sketchfab.com/"));
    }

    private async void update()
    {
        isrun = true;
        while (isrun)
        {
            await Task.Delay(1000);
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync("8.8.8.8", 100);

                if(reply.Status == IPStatus.Success)
                {
                    check.Text = "Подключение в норме!";
                    check.SetAppThemeColor(Label.TextColorProperty, Colors.Green, Colors.LightGreen);
                }
                else
                {
                    check.Text = "Соединение...";
                    check.SetAppThemeColor(Label.TextColorProperty, Colors.DarkRed, Colors.Red);
                }
            }
            catch (Exception ex)
            {
                check.Text = "Соединение...";
                check.SetAppThemeColor(Label.TextColorProperty, Colors.DarkRed, Colors.Red);
            }
        }
    }

    private void music_Toggled(object sender, ToggledEventArgs e)
    {
        mus = music.IsToggled;
        Preferences.Set("music", mus);
    }

    private void vibro_Toggled(object sender, ToggledEventArgs e)
    {
        vib = vibro.IsToggled;
        Preferences.Set("vibro", vib);

#if ANDROID || IOS
        if (vibro.IsToggled && initialized) Vibration.Default.Vibrate();
#endif

    }

    private async void offlineb_Clicked(object sender, EventArgs e)
    {
        var readExternalPermission = await CheckAndRequestPermission<Permissions.StorageRead>();
        var writeExternalPermission = await CheckAndRequestPermission<Permissions.StorageWrite>();
        var mediaPermission = await CheckAndRequestPermission<Permissions.Media>();
        if(readExternalPermission == PermissionStatus.Granted && writeExternalPermission == PermissionStatus.Granted && mediaPermission == PermissionStatus.Granted) await Navigation.PushAsync(new offline());

    }

    public async Task<PermissionStatus> CheckAndRequestPermission<T>() where T : Permissions.BasePermission, new()
    {
        var status = await Permissions.CheckStatusAsync<T>();

        if (status == PermissionStatus.Granted)
            return status;

        status = await Permissions.RequestAsync<T>();
        return status;
    }
}