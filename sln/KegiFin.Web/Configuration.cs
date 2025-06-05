using MudBlazor;

namespace KegiFin.Web;

public static class Configuration
{
    public const string HttpClientName = "KegiFin";
    
    //API ROUTES BASE URL
    public const string ReportsBaseUrl = "v1/reports";
    public static string BackendUrl { get; set; } = null!;

    public static MudTheme Theme = new()
    {
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Raleway", "sans-serif"]
            }
        },

        // LIGHT MODE
        PaletteLight = new PaletteLight
        {
            Primary = "#4CAF50",              // Verde equilibrado
            Secondary = "#212121",            // Preto suave para contrastes
            Background = "#FFFFFF",           // Fundo branco
            Surface = "#F5F5F5",              // Cartões e containers
            AppbarBackground = "#FFFFFF",
            AppbarText = "#212121",
            DrawerBackground = "#FAFAFA",
            DrawerText = "#212121",
            TextPrimary = "#212121",
            TextSecondary = "#757575",
            ActionDefault = "#757575",
            ActionDisabled = "#BDBDBD",
            ActionDisabledBackground = "#E0E0E0",
            Error = "#D32F2F"
        },

        // DARK MODE
        PaletteDark = new PaletteDark
        {
            Primary = "#66BB6A",               // Verde mais claro
            Secondary = "#EEEEEE",            // Texto secundário mais claro
            Background = "#121212",
            Surface = "#1E1E1E",
            AppbarBackground = "#1E1E1E",
            AppbarText = "#FFFFFF",
            DrawerBackground = "#1C1C1C",
            DrawerText = "#FFFFFF",
            TextPrimary = "#FFFFFF",
            TextSecondary = "#B0BEC5",
            ActionDefault = "#B0BEC5",
            ActionDisabled = "#757575",
            ActionDisabledBackground = "#424242",
            Error = "#EF5350"
        },
    };

    
}