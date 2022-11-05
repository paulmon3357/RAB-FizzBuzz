#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace RAB_FizzBuzz
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;



            XYZ insPoint = uidoc.Selection.PickPoint("Pick a point for sample text.");
            double offset = 0.04;
            double calcOffset = offset * doc.ActiveView.Scale;
            XYZ offsetPoint = new XYZ(0, calcOffset, 0);

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(TextNoteType));


            int range = 100;

            //Get usert input on the range of the number
            //string rangeasstring = Console.ReadLine();

            //if (int.TryParse(rangeasstring, out int value))
            //{
            //    range = Convert.ToInt32(rangeasstring);
            //}


            using (Transaction t = new Transaction(doc))
            { 
                t.Start("FizzBuzz");

                for (int i = 1; i <= range; i++)
                {
                    string result = " ";

                    result = i.ToString() + " ";

                    if (i % 3 == 0)
                    {
                        result = result + "Fizz";
                    }

                    if (i % 5 == 0)
                    {
                        result = result + "Buzz";
                    }

                    //if (i % 3 != 0 && i % 5 != 0)
                    //{
                    //    result = i.ToString();
                    //}

                    Debug.Print(result);

                    TextNote curNote = TextNote.Create(doc, doc.ActiveView.Id, insPoint, result, collector.FirstElementId()); ;

                    //incriment increatment inseration point position

                    insPoint = insPoint.Subtract(offsetPoint);
                }

                t.Commit();
            }

           
            return Result.Succeeded;
        }
    }
}
