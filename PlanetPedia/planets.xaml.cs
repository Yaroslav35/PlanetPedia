namespace PlanetPedia;

public partial class planets : ContentPage
{
	public planets()
	{
		InitializeComponent();
	}

    private void back_Clicked(object sender, EventArgs e)
    {
		Navigation.PopAsync();
    }

    private void tnob_Clicked(object sender, EventArgs e)
    {
        tnob.SetAppThemeColor(Label.TextColorProperty, Color.FromArgb("551A8B"), Color.FromArgb("db96f2"));
        Navigation.PushAsync(new web("https://ru.wikipedia.org/wiki/%D0%A2%D1%80%D0%B0%D0%BD%D1%81%D0%BD%D0%B5%D0%BF%D1%82%D1%83%D0%BD%D0%BE%D0%B2%D1%8B%D0%B9_%D0%BE%D0%B1%D1%8A%D0%B5%D0%BA%D1%82"));
    }

    private void sunb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("sun.txt","sunmoons.txt"));
    }

    private void mercuryb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("mercury.txt", "mercurymoons.txt"));
    }

    private void venusb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("venus.txt","venusmoons.txt"));
    }

    private void earthb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("earth.txt","earthmoons.txt"));
    }

    private void marsb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("mars.txt","marsmoons.txt"));
    }

    private void jupiterb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("jupiter.txt","jupitermoons.txt"));
    }

    private void saturnb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("saturn.txt","saturnmoons.txt"));
    }

    private void uranusb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("uranus.txt", "uranusmoons.txt"));
    }

    private void neptuneb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("neptune.txt","neptunemoons.txt"));
    }

    private void neirob_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new neiro());
    }

    private void voyager_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("voyager2.txt","sunmoons.txt"));
    }

    private void spaceb_Clicked(object sender, EventArgs e)
    {
        List<string> unique = new List<string>();
        foreach(string test in Preferences.Get("tests", "").Split(";")) if (!unique.Contains(test)) unique.Add(test);
        if(unique.Count > 0) unique.RemoveAt(0);
        if (unique.Count >= 20) Navigation.PushAsync(new space());
    }
}