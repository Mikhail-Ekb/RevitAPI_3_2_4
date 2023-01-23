using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI_3_2_4
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ElementCategoryFilter wallCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ElementId level1Id = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Where(x => x.Name.Equals("Этаж 01"))
                .FirstOrDefault().Id;

            ElementId level2Id = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Where(x => x.Name.Equals("Этаж 02"))
                .FirstOrDefault().Id;

            ElementLevelFilter level1Filter = new ElementLevelFilter(level1Id);
            LogicalAndFilter wallsFilter1 = new LogicalAndFilter(wallCategoryFilter, level1Filter);

            ElementLevelFilter level2Filter = new ElementLevelFilter(level1Id);
            LogicalAndFilter wallsFilter2 = new LogicalAndFilter(wallCategoryFilter, level2Filter);

            var walls1 = new FilteredElementCollector(doc)
                .WherePasses(wallsFilter1)
                .Cast<Wall>()
                .ToList();

            TaskDialog.Show("Количество стен", $"Количество стен на 1 этаже: {walls1.Count.ToString()}");

            var walls2 = new FilteredElementCollector(doc)
                .WherePasses(wallsFilter2)
                .Cast<Wall>()
                .ToList();

            TaskDialog.Show("Количество стен", $"Количество стен на 2 этаже: {walls2.Count.ToString()}");
            return Result.Succeeded;
        }
    }
}
