namespace PlanetPedia;

public partial class objects : ContentPage
{
    bool anim = false;
	public objects()
	{
		InitializeComponent();

        List<VisualElement> elements = new List<VisualElement>() {bor1, bor2, bor3, bor4, bor5, bor6, bor7, bor8, bor9, bor10};
        foreach (VisualElement element in elements) element.Opacity = 0;
        anim = true;

        bor1.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor2.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor3.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor4.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor5.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor6.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor7.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor8.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor9.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor10.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<VisualElement> elements = new List<VisualElement>() { bor1, bor2, bor3, bor4, bor5, bor6, bor7, bor8, bor9, bor10 };

        await Task.Delay(300);
        foreach (VisualElement element in elements)
        {
            float tr = anim ? 0f : 1f;
            while (tr < 1)
            {
                tr += 0.1f;
                element.Opacity = tr;
                await Task.Delay(10);
            }
            await Task.Delay(10);
        }
        anim = false;
    }

    private void pulsarb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("pulsar.txt","sunmoons.txt", true, 0));
    }

    private void blackholeb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("blackhole.txt", "sunmoons.txt", true, 0));
    }

    private void cometb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("comet.txt", "sunmoons.txt", true, 0));
    }

    private void oortab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("oorta.txt","sunmoons.txt", true, 0));
    }

    private void milkywayb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("milkyway.txt","sunmoons.txt", true, 0));
    }

    private void attractorb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("attractor.txt","sunmoons.txt", true, 0));
    }

    private void voidb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("void.txt", "sunmoons.txt", true, 0));
    }

    private void nebulab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("nebula.txt","sunmoons.txt", true, 0));
    }

    private void laniakeab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("laniakea.txt", "sunmoons.txt", true, 0));
    }

    private void webspaceb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("webspace.txt", "sunmoons.txt", true, 0));
    }
}