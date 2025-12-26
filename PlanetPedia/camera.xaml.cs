namespace PlanetPedia;

public partial class camera : ContentPage
{
    string lastDetectedBarcode = string.Empty;
    DateTime lastDetectedTime = DateTime.MinValue;
    Dictionary<string, List<string>> codes = new Dictionary<string, List<string>>()
    {
        {"pp0", ["sun.txt", "sunmoons.txt"]},
        {"pp1", ["mercury.txt", "mercurymoons.txt"]},
        {"pp2", ["venus.txt", "venusmoons.txt"]},
        {"pp3", ["earth.txt", "earthmoons.txt"]},
        {"pp4", ["mars.txt", "marsmoons.txt"]},
        {"pp5", ["jupiter.txt", "jupitermoons.txt"]},
        {"pp6", ["saturn.txt", "saturnmoons.txt"]},
        {"pp7", ["uranus.txt", "uranusmoons.txt"]},
        {"pp8", ["neptune.txt", "neptunemoons.txt"]},
        {"ps1", ["moon.txt","sunmoons.txt"]},
        {"ps2", ["fobos.txt","sunmoons.txt"]},
        {"ps3", ["deimos.txt","sunmoons.txt"]},
        {"ps4", ["io.txt","sunmoons.txt"]},
        {"ps5", ["callisto.txt","sunmoons.txt"]},
        {"ps6", ["ganymede.txt","sunmoons.txt"]},
        {"ps7", ["europa.txt","sunmoons.txt"]},
        {"ps8", ["titan.txt","sunmoons.txt"]},
        {"ps9", ["mimas.txt","sunmoons.txt"]},
        {"ps10", ["enceladus.txt","sunmoons.txt"]},
        {"ps11", ["reya.txt","sunmoons.txt"]},
        {"ps12", ["yapet.txt","sunmoons.txt"]},
        {"ps13", ["miranda.txt","sunmoons.txt"]},
        {"ps14", ["titania.txt","sunmoons.txt"]},
        {"ps15", ["oberon.txt","sunmoons.txt"]},
        {"ps16", ["tritone.txt","sunmoons.txt"]}
    };
    public camera()
	{
		InitializeComponent();
        cameraBarcodeReaderView.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.Code128,
            AutoRotate = true,
            Multiple = true
        };
    }
    protected void BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();
        if (first is null)
        {
            return;
        }

        // Check if the same barcode was detected within the last second
        if (first.Value == lastDetectedBarcode && (DateTime.Now - lastDetectedTime).TotalSeconds < 1)
        {
            return;
        }

        lastDetectedBarcode = first.Value;
        lastDetectedTime = DateTime.Now;

        Dispatcher.DispatchAsync(async () =>
        {
            if (codes.ContainsKey(first.Value)) Navigation.PushAsync(new card(codes[first.Value][0], codes[first.Value][1], true, 0));

        });
    }

    private void what_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new web("camera.html"));
    }
}