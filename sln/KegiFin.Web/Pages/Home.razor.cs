using Microsoft.AspNetCore.Components;

namespace KegiFin.Web.Pages;

public partial class HomePageComponent : ComponentBase
{
    #region Properties

    protected bool ShowValues { get; set; } = true;

    #endregion

    #region Methods

    protected void ToggleShowValues()
    {
        ShowValues = !ShowValues;
    }

    #endregion
}