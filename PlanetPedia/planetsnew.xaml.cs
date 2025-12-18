namespace PlanetPedia;

public partial class planetsnew : ContentPage
{
    bool run1 = false;
    bool run2 = false;
    bool run3 = false;
    string empty = "sunmoons.txt";
    public planetsnew()
	{
		InitializeComponent();
        astro.Rotation = 0;
        spacei.Rotation = 0;
        settingsb.Rotation = 0;
        camerab.Rotation = 0;
        permission();
        Update();
        LateUpdate();
        Rot();
	}

    private async void permission()
    {
        var readExternalPermission = await CheckAndRequestPermission<Permissions.StorageRead>();
        var writeExternalPermission = await CheckAndRequestPermission<Permissions.StorageWrite>();
        var mediaPermission = await CheckAndRequestPermission<Permissions.Media>();
    }
    public async Task<PermissionStatus> CheckAndRequestPermission<T>() where T : Permissions.BasePermission, new()
    {
        var status = await Permissions.CheckStatusAsync<T>();

        if (status == PermissionStatus.Granted)
            return status;

        status = await Permissions.RequestAsync<T>();
        return status;
    }
    private void sunb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("sun.txt", "sunmoons.txt"));
    }

    private void mercuryb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("mercury.txt", "mercurymoons.txt"));
    }

    private void venusb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("venus.txt", "venusmoons.txt"));
    }

    private void earthb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("earth.txt", "earthmoons.txt"));
    }

    private void marsb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("mars.txt", "marsmoons.txt"));
    }

    private void jupiterb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("jupiter.txt", "jupitermoons.txt"));
    }

    private void saturnb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("saturn.txt", "saturnmoons.txt"));
    }

    private void uranusb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("uranus.txt", "uranusmoons.txt"));
    }

    private void neptuneb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("neptune.txt", "neptunemoons.txt"));
    }

    private void neirob_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new neiro());
    }

    private void voyager_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("voyager2.txt", "sunmoons.txt"));
    }

    private void spaceb_Clicked(object sender, EventArgs e)
    {
        List<string> unique = new List<string>();
        foreach (string test in Preferences.Get("tests", "").Split(";")) if (!unique.Contains(test)) unique.Add(test);
        if (unique.Count > 0) unique.RemoveAt(0);
        if (unique.Count >= 5) Navigation.PushAsync(new space());
    }

    private void moonb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("moon.txt", empty));
    }

    private void fobosb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("fobos.txt", empty));
    }

    private void deimosb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("deimos.txt", empty));
    }

    private void iob_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("io.txt", empty));
    }

    private void callisto_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("callisto.txt", empty));
    }

    private void ganymedeb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("ganymede.txt", empty));
    }

    private void europab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("europa.txt", empty));
    }

    private void titanb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("titan.txt", empty));
    }

    private void mimasb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("mimas.txt", empty));
    }

    private void enceladusb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("enceladus.txt", empty));
    }

    private void reyab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("reya.txt", empty));
    }

    private void yapetb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("yapet.txt", empty));
    }

    private void mirandab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("miranda.txt", empty));
    }

    private void titaniab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("titania.txt", empty));
    }

    private void oberonb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("oberon.txt", empty));
    }

    private void tritoneb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("tritone.txt", empty));
    }

    private void tg_Tapped(object sender, EventArgs e)
    {
        Navigation.PushAsync(new web("https://t.me/tezoria8"));
    }

    private async void Update()
    {
        run1 = true;
        while (run1)
        {
            await Task.Delay(100);
            astro.Rotation += 1;
            spacei.Rotation += 0.5;
            camerab.Rotation += 1;
            settingsb.Rotation += 1;

            if (Math.Round(scrollview.ScrollY / 100, 1) <= 6) scroll.Opacity = 0;
            else scroll.Opacity = Math.Min(1 - 2.0 / Math.Round(scrollview.ScrollY / 100, 1), 1);

            scroll.Text = $"Расстояние: {Math.Round(scrollview.ScrollY / 100, 1)} ед";
        }
    }

    private async void LateUpdate()
    {
        run2 = true;
        string text = "1001010101110 Console.WriteLine('Вы лучшие :)'); 1011101";
        string current = "";
        while(run2)
        {
            foreach(char el in text)
            {
                await Task.Delay(200);
                current += el;
                final.Text = current;
            }
            await Task.Delay(1500);
            current = "";

        }
    }

    private async void Rot()
    {
        run3 = true;
        while(run3)
        {
            await Task.Delay(100);

            earthbox.Rotation += 0.1f;
            earth.Rotation -= 0.1f;
            marsbox.Rotation += 0.1f;
            mars.Rotation -= 0.1f;
            jupiterbox.Rotation += 0.1f;
            jupiter.Rotation -= 0.1f;
            saturnbox.Rotation += 0.1f;
            saturn.Rotation -= 0.1f;
            uranusbox.Rotation += 0.1f;
            uranus.Rotation -= 0.1f;
            neptunebox.Rotation += 0.1f;
            neptune.Rotation -= 0.1f;
        }
    }

    private async void camera_Tapped(object sender, EventArgs e)
    {
        var cameraPermission = await CheckAndRequestPermission<Permissions.Camera>();
        if (cameraPermission == PermissionStatus.Granted) await Navigation.PushAsync(new camera());
    }
    private void settings_Tapped(object sender, EventArgs e)
    {
        Navigation.PushAsync(new settings());
    }
    private void neiro_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new neiro());
    }
    private void ton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("ton618.txt", "sunmoons.txt"));
    }
}