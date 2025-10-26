namespace PlanetPedia;

public partial class exo : ContentPage
{
	public exo()
	{
		InitializeComponent();
	}

    private void kepler22b_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new card("kepler22.txt","sunmoons.txt"));
    }

    private void corot7b_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("corot7b.txt","sunmoons.txt"));
    }

    private void tres2b_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("tres2b.txt","sunmoons.txt"));
    }

    private void proximab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("proximab.txt","sunmoons.txt"));
    }

    private void hd189733Ab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("hd189733ab.txt","sunmoons.txt"));
    }

    private void supersaturn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("supersaturn.txt","sunmoons.txt"));
    }

    private void gj1214b_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("gj1214b.txt","sunmoons.txt"));
    }
}