using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.IO;
using System.Reflection;

namespace AutoDimGrids
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId(GlobalVars.APP_GUID)]
    public class ThisApplication : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication uiApp)
        {
            #region GAS ADDIN BOILERPLATE

            // Finds and creates the tab, finds and creates the panel

            

            string ThisDllPath = Assembly.GetExecutingAssembly().Location;
            Assembly ThisAssembly = Assembly.GetExecutingAssembly();

            // Assembly that contains the invoke method
            String exeConfigPath = Path.GetDirectoryName(ThisDllPath) + "\\AutoDimGrids.dll";

            RibbonPanel DefaultPanel = null;

            // First try to create the tab
            try
            {
                uiApp.CreateRibbonTab(GlobalVars.TAB_NAME);
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                // Tab is already created
            }


            // Then create the panel
            try
            {
                DefaultPanel = uiApp.CreateRibbonPanel(GlobalVars.TAB_NAME, GlobalVars.PANEL_NAME);
            }

            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                DefaultPanel = uiApp.GetRibbonPanels(GlobalVars.TAB_NAME).FirstOrDefault(n => n.Name.Equals(GlobalVars.PANEL_NAME, StringComparison.InvariantCulture));
            }
            #endregion


            try
            {

                string AutoDimGridsName = "Auto Dimension Grids";
                PushButtonData AutoDimGridsData = new PushButtonData(AutoDimGridsName, AutoDimGridsName, exeConfigPath, "AutoDimGrids.ThisCommand"); // Invoke class, pushbutton data
                AutoDimGridsData.LargeImage = Utils.RetriveImage("AutoDimGrids.Resources.GridsIcons32x32.ico"); // Pushbutton image
                AutoDimGridsData.ToolTip = "Automatically turn on or off grid bubbles correctly";
                PushButton SmartGridsButton = DefaultPanel.AddItem(AutoDimGridsData) as PushButton;

                return Result.Succeeded;
            }

            catch (Exception ex)
            {
                Utils.CatchDialog(ex);
                return Result.Failed;
            }
        }



    }
}
