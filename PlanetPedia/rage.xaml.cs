namespace PlanetPedia;

public partial class rage : ContentPage
{
	public rage()
	{
        InitializeComponent();
		Update();
	}

	private async void Update()
	{
		while(true)
		{
			await Task.Delay(100);

			int sunpart = Preferences.Get("sun_rage", 0);
            int jupiterpart = Preferences.Get("jupiter_rage", 0);
            int netronpart = Preferences.Get("netron_rage", 0);
            int quarkpart = Preferences.Get("quark_rage", 0);

			sun.Text = $"Собрано фрагментов: {Math.Min(sunpart, 200)}/200";
            jupiter.Text = $"Собрано фрагментов: {Math.Min(jupiterpart, 200)}/200";
            netron.Text = $"Собрано фрагментов: {Math.Min(netronpart, 200)}/200";
            quark.Text = $"Собрано фрагментов: {Math.Min(quarkpart, 200)}/200";

			bool free = Preferences.Get("freerage", true);
			int stars = Preferences.Get("stars", 0);

			if(stars > 4000 && free) freerage.IsVisible = true;
			else freerage.IsVisible = false;
        }
	}

    private void get_Clicked(object sender, EventArgs e)
    {
		Random rand = new Random();
		int chance = rand.Next(1, 5);
		switch(chance)
		{
			case 1:
				Preferences.Set("sun_rage", 200);
				break;
            case 2:
                Preferences.Set("jupiter_rage", 200);
                break;
            case 3:
                Preferences.Set("netron_rage", 200);
                break;
            case 4:
                Preferences.Set("quark_rage", 200);
                break;
        }
		Preferences.Set("freerage", false);
    }
}