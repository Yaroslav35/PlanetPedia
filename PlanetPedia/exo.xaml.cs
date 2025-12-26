using System.Diagnostics;

namespace PlanetPedia;

public partial class exo : ContentPage
{
    bool anim = false;
	public exo()
	{
		InitializeComponent();

        List<VisualElement> elements = new List<VisualElement>() {bor1, bor2, bor3, bor4, bor5, bor6, bor7};
        foreach (VisualElement element in elements) element.Opacity = 0;
        anim = true;

        bor1.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor2.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor3.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor4.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor5.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor6.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor7.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<VisualElement> elements = new List<VisualElement>() { bor1, bor2, bor3, bor4, bor5, bor6, bor7 };

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

    private void kepler22b_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new card("kepler22.txt","sunmoons.txt", true, 0));
    }

    private void corot7b_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("corot7b.txt","sunmoons.txt", true, 0));
    }

    private void tres2b_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("tres2b.txt","sunmoons.txt", true, 0));
    }

    private void proximab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("proximab.txt","sunmoons.txt", true, 0));
    }

    private void hd189733Ab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("hd189733ab.txt","sunmoons.txt", true, 0));
    }

    private void supersaturn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("supersaturn.txt","sunmoons.txt", true, 0));
    }

    private void gj1214b_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("gj1214b.txt","sunmoons.txt", true, 0));
    }
}