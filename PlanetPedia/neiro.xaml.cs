namespace PlanetPedia;

public partial class neiro : ContentPage
{
	public neiro()
	{
		InitializeComponent();
	}

    private void kepler_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new card("kepler22.txt","sunmoons.txt"));
    }
}