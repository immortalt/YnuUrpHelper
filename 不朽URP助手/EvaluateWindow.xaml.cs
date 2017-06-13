using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using 不朽URP助手.Entities;
using 不朽URP助手.Helpers;

namespace 不朽URP助手
{
    /// <summary>
    /// EvaluateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EvaluateWindow : Window
    {
        public static Survey survey;
        public static TeachClass teachClass;
        public static EvaluationResult evaluationResult;
        public EvaluateWindow(Survey sv, TeachClass cla, EvaluationResult er)
        {
            InitializeComponent();
            Uri iconUri = new Uri("pack://application:,,,/immortalt.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            survey = sv;
            teachClass = cla;
            evaluationResult = er;
            this.Title = "教学评估-" + cla.CourseName;
            stack_head.DataContext = cla;
            lv_evaluate.ItemsSource = evaluateVM;
            if(evaluationResult.Comment == null)
            {
                Random rand = new Random(EncryptHelper.GetRandomSeed());
                var id = rand.Next(0, StringValues.Comments.Length);
                evaluationResult.Comment = StringValues.Comments[id];
            }
            grid_comment.DataContext = evaluationResult;
        }
        private ObservableCollection<EvaluateVM> _evaluateVM = null;

        public ObservableCollection<EvaluateVM> evaluateVM
        {
            get
            {
                if (this._evaluateVM == null)
                {
                    this._evaluateVM = new ObservableCollection<EvaluateVM>();
                    if (evaluationResult.Score == 0)
                    {
                        evaluationResult = new EvaluationResult();
                        Random rand = new Random(EncryptHelper.GetRandomSeed());
                        var results = new char[survey.SurveyItemList.Count].ToList();
                        var c = 2;
                        for (int i = 0; i < results.Count; i++)
                        {
                            results[i] = '0';
                        }
                        for (int i = 0; i < c; i++)
                        {
                            var index = rand.Next(0, results.Count);
                            results[index] = '1';
                        }
                        survey.SurveyItemList.ForEach(
                            t =>
                            {
                                int index = Convert.ToInt32(results.First().ToString());
                                _evaluateVM.Add(new EvaluateVM
                                {
                                    Code = t.Code,
                                    Name = t.Name,
                                    Weight = t.Weight,
                                    Result = index
                                });
                                results.RemoveAt(0);
                            }
                            );
                    }
                    else
                    {
                        var results = evaluationResult.SurveyAnswerStr.ToCharArray().ToList();
                        survey.SurveyItemList.ForEach(
                            t =>
                            {
                                int index = Convert.ToInt32(results.First().ToString()) - 1;
                                _evaluateVM.Add(new EvaluateVM
                                {
                                    Code = t.Code,
                                    Name = t.Name,
                                    Weight = t.Weight,
                                    Result = index
                                });
                                results.RemoveAt(0);
                            }
                            );
                    }
                }
                return _evaluateVM;
            }
            set
            {
                this._evaluateVM = value;
            }
        }
        public class EvaluateVM
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public double Weight { get; set; }
            public int Result { get; set; }
        }

        private void btu_submit_Click(object sender, RoutedEventArgs e)
        {
            var results = new List<char>();
            for (int i = 0; i < evaluateVM.Count; i++)
            {
                var t = evaluateVM[i];
                results.Add(
               Convert.ToChar((t.Result + 1).ToString())
                );
            }
            evaluationResult.SurveyAnswerStr = new string(results.ToArray());
            this.Tag = evaluationResult;
            Debug.WriteLine(evaluationResult.SurveyAnswerStr);
            Debug.WriteLine(evaluationResult.Comment);
            this.Close();
        }
    }
}
