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
    [Autodesk.Revit.DB.Macros.AddInId("33770B64-A9B3-49D0-B99D-2C70A648DE8E")]
    public class ThisApplication : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication uiApp)
        {
            #region GAS ADDIN BOILERPLATE

            // Assembly that contains the invoke method
            string exeConfigPath = Utils.GetExeConfigPath("AutoDimGrids.dll");

            // Finds and creates the tab, finds and creates the panel
            RibbonPanel DefaultPanel = Utils.GetRevitPanel(uiApp, GasToolsGlobals.PANEL_NAME);
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
