using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleController : MonoBehaviour
{   
  public static void OnLocaleDropdownValueChanged(int v) { LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[v]; }
}