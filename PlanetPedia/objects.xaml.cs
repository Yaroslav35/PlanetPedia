namespace PlanetPedia;

public partial class objects : ContentPage
{
	public objects()
	{
		InitializeComponent();
	}

    private void pulsarb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("pulsar.txt","sunmoons.txt"));
    }

    private void blackholeb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("blackhole.txt", "sunmoons.txt"));
    }

    private void cometb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("comet.txt", "sunmoons.txt"));
    }

    private void oortab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("oorta.txt","sunmoons.txt"));
    }

    private void milkywayb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("milkyway.txt","sunmoons.txt"));
    }

    private void attractorb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("attractor.txt","sunmoons.txt"));
    }

    private void voidb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("void.txt", "sunmoons.txt"));
    }

    private void nebulab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("nebula.txt","sunmoons.txt"));
    }

    private void laniakeab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("laniakea.txt", "sunmoons.txt"));
    }

    private void webspaceb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("webspace.txt", "sunmoons.txt"));
    }
}