using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Settings", menuName = "BookPage/Settings", order = 5)]
public class SettingsBookPage:BookPage{
    public override void Activate(BookController bc){
        bc.activePageController=bc.settingsPageController;
        bc.settingsPageController.SetSliders();
        bc.settingsPageController.SetToggles();
    }
}