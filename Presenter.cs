using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThirdKR
{
    public interface IPresenter
    {
        void Run();
    }
    public class Presenter : IPresenter
    {
        const int MAX_CHARTS_COUNT = 5;
        const int EXPECTED_AMOUNT_DATA = 4; 
        private readonly IMyAppForm appForm;
        private readonly ICalculator calculator;
        private readonly IMessagesService messageService;
        private readonly IValidator validator;
        private readonly IFileManager fileManager;

        private List<DataHolder<double>> dataHolders = new List<DataHolder<double>>();
        private List<GraphData> graphDatas = new List<GraphData>();


        public Presenter(IMyAppForm appForm, ICalculator calculator, IMessagesService messageService, IValidator validator, IFileManager fileManager)
        {
            this.appForm = appForm;
            this.calculator = calculator;
            this.messageService = messageService;
            this.validator = validator;
            this.fileManager = fileManager;



            appForm.TryDraw += AppForm_TryDraw;
            appForm.Clear += AppForm_Clear;
            appForm.Removelast += AppForm_RemoveLast;
            appForm.OpenFile += AppForm_OpenFile;
            appForm.SaveInitialData += AppForm_SaveInitialData;
            appForm.NoData += AppForm_NoData;

        }

        public void Run()
        {
            appForm.Show();
        }

        private void Validator_ValidationError(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

 
        private void AppForm_SaveInitialData(object? sender, EventArgs e)
        {
            string tempData = "";
            foreach (var item in dataHolders)
            {
                tempData += item.GetStringData() + Environment.NewLine;
            }

            fileManager.SaveInitialData(tempData, appForm.FilePath);
        }

        private void AppForm_TryDraw(object? sender, EventArgs e)
        {
            Draw();
        }

        private void AppForm_Clear(object? sender, EventArgs e)
        {

            dataHolders.Clear();
            graphDatas.Clear();
        }
        private void AppForm_RemoveLast(object? sender, EventArgs e)
        {

            dataHolders.RemoveAt(dataHolders.Count - 1);
            graphDatas.RemoveAt(graphDatas.Count - 1);
        }

        private void AppForm_OpenFile(object? sender, EventArgs e)
        {
            string[] fileValue = fileManager.OpenFile(appForm.FilePath);
            for (int i = 0; i < fileValue.Length; i++)
            {
                string[] chartLine = fileValue[i].Split("").Where(x => x != "").ToArray();
                if (chartLine.Length != EXPECTED_AMOUNT_DATA)
                {
                    messageService.ShowError("Wrong file data");
                    return;
                }
                appForm.ClearFields();
                appForm.LeftBorder = chartLine[0];
                Thread.Sleep(500);
                appForm.RightBorder = chartLine[1];
                Thread.Sleep(500);
                appForm.Step = chartLine[2];
                Thread.Sleep(500);
                appForm.A = chartLine[3];
                Draw();
            }
        }

        private void AppForm_NoData(object? sender, EventArgs e)
        {
            messageService.ShowMessage("Сharts missing!!");
        }

        private void Draw()
        {
            if (graphDatas.Count >= MAX_CHARTS_COUNT)
            {
                messageService.ShowMessage("The maximum number of charts has been entered.");
                return;
            }

            string errors = validator.PreValidation(new DataHolder<string>(appForm.LeftBorder, appForm.RightBorder, appForm.Step, appForm.A));
            if (errors != "")
            {
                messageService.ShowError(errors);
                return;
            }
            dataHolders.Add(new DataHolder<double>(double.Parse(appForm.LeftBorder), double.Parse(appForm.RightBorder), double.Parse(appForm.Step), double.Parse(appForm.A)));


            if (!validator.AccetableRange(dataHolders[dataHolders.Count - 1]))
            {
                dataHolders.RemoveAt(dataHolders.Count - 1);
                messageService.ShowError("Values does not match accetable range");
                return;
            }
            graphDatas.Add(calculator.GetGraphData(dataHolders[dataHolders.Count - 1]));

            if (!validator.PostValidation(graphDatas[dataHolders.Count - 1].ys))
            {
                messageService.ShowError("Values does not match accetable range");

                dataHolders.RemoveAt(dataHolders.Count - 1);
                graphDatas.RemoveAt(graphDatas.Count - 1);
                return;
            }


            string? label = "[" + dataHolders[dataHolders.Count - 1].LeftBorder.ToString() + "," + dataHolders[dataHolders.Count - 1].RightBorder.ToString() + ","
                + dataHolders[dataHolders.Count - 1].Step.ToString() + "," + dataHolders[dataHolders.Count - 1].A.ToString() + "]";

            appForm.Draw(graphDatas[graphDatas.Count - 1], label);
           
        }


        private Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);

            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);

                return image;
            }
        }


    }
}
