using MudBlazor;

namespace KegiFin.Web;

public static class Configuration
{
    public static MudTheme Theme = new()
    {
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Raleway", "sans-serif"]
            }
        },
        PaletteLight = new PaletteLight
        {
            Primary = "#5C6BC0", // Roxo moderno (botões, foco)
            Secondary = "#43A047", // Verde escuro (conecta com o logo)
            Background = "#FFFFFF", // Fundo branco puro
            Surface = "#FAFAFA", // Leve contraste nos cartões
            AppbarBackground = "#FFFFFF",
            AppbarText = "#212121",
            DrawerBackground = "#F3F3F3",
            DrawerText = "#212121",
            TextPrimary = "#212121",
            TextSecondary = "#616161",
            ActionDefault = "#616161",
            ActionDisabled = "#BDBDBD",
            ActionDisabledBackground = "#E0E0E0",
            Error = "#D32F2F"
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#5C6BC0", // Roxo
            Secondary = "#66BB6A", // Verde suave
            Background = "#121212", // Fundo escuro
            Surface = "#1E1E1E",
            AppbarBackground = "#1E1E1E",
            AppbarText = "#FFFFFF",
            DrawerBackground = "#212121",
            DrawerText = "#FFFFFF",
            TextPrimary = "#FFFFFF",
            TextSecondary = "#B0BEC5",
            ActionDefault = "#B0BEC5",
            ActionDisabled = "#616161",
            ActionDisabledBackground = "#424242",
            Error = "#EF5350" // Vermelho claro para melhor contraste no escuro
        },
    };
}