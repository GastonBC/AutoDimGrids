using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDimGrids
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId(GlobalVars.CMD_GUID)]
    class ThisCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)

        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {

                ReferenceArray GridsRefRow = new ReferenceArray();
                ReferenceArray GridsRefCol = new ReferenceArray();

                XYZ TopPoint = null;
                XYZ LeftPoint = null;

                foreach (ElementId elemId in uidoc.Selection.GetElementIds())
                {
                    Element SelectedElem = doc.GetElement(elemId);
                    if (SelectedElem is DatumPlane)
                    {
                        DatumPlane xGrid = SelectedElem as DatumPlane;

                        IList<Curve> CurvesInGrid = xGrid.GetCurvesInView(DatumExtentType.ViewSpecific, uidoc.ActiveView);

                        Curve cv = CurvesInGrid.First(); // only one segment grids allowed

                        XYZ cvStart = cv.GetEndPoint(0);
                        XYZ cvEnd = cv.GetEndPoint(1);

                        if (Convert.ToInt32(cvStart.Y) == Convert.ToInt32(cvEnd.Y)) // then its a horizontal line, need to round the double bc the comparison will never work
                        {
                            GridsRefRow.Append(new Reference(xGrid));


                            if (LeftPoint is null && cvStart.X < cvEnd.X) // drawn left to right, keep start    X--------->
                            {
                                LeftPoint = cvStart;
                            }

                            else if (LeftPoint is null && cvStart.X > cvEnd.X) // drawn right to left, keep end    <---------X
                            {
                                LeftPoint = cvEnd;
                            }
                        }

                        else if (Convert.ToInt32(cvStart.X) == Convert.ToInt32(cvEnd.X)) // then its a vertical
                        {
                            GridsRefCol.Append(new Reference(xGrid));

                            if (TopPoint is null && cvStart.Y < cvEnd.Y) // drawn bottom to top, keep end
                            {
                                TopPoint = cvEnd;
                            }

                            else if (TopPoint is null && cvStart.Y > cvEnd.Y) // drawn top to bottom, keep start
                            {
                                TopPoint = cvStart;
                            }
                        }



                    }
                }

                using (Transaction t = new Transaction(doc, "Auto dimension grids"))
                {
                    t.Start();

                    if (GridsRefCol.Size >= 2)
                    {
                        XYZ SecondTopPoint = new XYZ(TopPoint.X + 10d, TopPoint.Y, TopPoint.Z);

                        Line lineC = Line.CreateBound(TopPoint, SecondTopPoint);
                        doc.Create.NewDimension(uidoc.ActiveView, lineC, GridsRefCol);
                    }

                    if (GridsRefRow.Size >= 2)
                    {
                        XYZ SecondLeftPoint = new XYZ(LeftPoint.X, LeftPoint.Y + 10d, LeftPoint.Z);

                        Line lineR = Line.CreateBound(LeftPoint, SecondLeftPoint);
                        doc.Create.NewDimension(uidoc.ActiveView, lineR, GridsRefRow);
                    }

                    t.Commit();
                }
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
