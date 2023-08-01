using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.IO;
using Utilities;
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


            // Create the panel in Add-ins tab
            try
            {
                DefaultPanel = uiApp.CreateRibbonPanel(GlobalVars.PANEL_NAME);
            }

            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                DefaultPanel = uiApp.GetRibbonPanels().FirstOrDefault(n => n.Name.Equals(GlobalVars.PANEL_NAME, StringComparison.InvariantCulture));
            }
            #endregion


            try
            {

                string AutoDimGridsName = "Auto Dimension Grids";
                PushButtonData AutoDimGridsData = new PushButtonData(AutoDimGridsName, AutoDimGridsName, exeConfigPath, "AutoDimGrids.ThisCommand"); // Invoke class, pushbutton data
                AutoDimGridsData.LargeImage = Utils.RetriveImage("AutoDimGrids.Resources.DimGrids32x32.ico", Assembly.GetExecutingAssembly()); // Pushbutton image
                AutoDimGridsData.ToolTip = "Automatically dimensions grids to the top and left";
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
