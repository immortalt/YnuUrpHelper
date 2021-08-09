using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using 不朽URP助手.Entities;
using 不朽URP助手.Helpers;
using 不朽URP助手.ViewModel;

namespace 不朽URP助手
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static LoginData loginData { get; set; }//登陆信息
        public static UserData userData { get; set; }//用户信息
        public static TeachEvaluation teachEvaluation { get; set; }//教学评估
        public static List<NameCode> academyList { get; set; }//学院
        public static string selectAcademyCode { get; set; }
        public static List<NameCode> majorList { get; set; }//专业
        public static string selectMajorCode { get; set; }
        public static List<NameCode> gradeList { get; set; }//年级
        public static string selectGradeCode { get; set; }
        public static List<NameCode> courseNatureList { get; set; }//课程性质
        public static string selectCourseNatureCode { get; set; }
        public static List<TeachClassModel> teachClassList { get; set; }//选课列表
        public static List<TeachClassModel> selectedClassList { get; set; }//已选到的课程列表


        public MainWindow(LoginData _loginData, UserData _userData)
        {
            InitializeComponent();
            Uri iconUri = new Uri("pack://application:,,,/immortalt.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            loginData = _loginData;
            userData = _userData;
            if (userData != null)
            {
                this.Title = "不朽URP助手-当前用户：" + userData.Name;
                grid_userData.DataContext = userData;
            }
            else
            {
                Debug.WriteLine("没有登录！程序即将退出");
                Environment.Exit(0);
            }
        }
        private async Task RefreshTeachEvaluation()
        {
            HttpMessage resp = await TeachEvaluationHelper.ListTeachEvaluation(loginData.access_token);
            switch (resp.statusCode)
            {
                case HttpStatusCode.OK:
                    TeachEvaluation TE = resp.data as TeachEvaluation;
                    if (TE != null)
                    {
                        teachEvaluation = TE;
                        //test 模拟没有选课的情况
                        //teachEvaluation.EvaluationResultList.ForEach(t => {
                        //    t.SurveyAnswerStr = null;
                        //    t.Comment = null;
                        //    t.Score = 0;
                        //});
                        //test
                        var vm = new ArrayList();
                        TE.TeachClassList.ForEach(c =>
                        {
                            EvaluationResult er =
                            TE.EvaluationResultList.
                            Where(t => t.TeachClassId == c.TeachClassId).SingleOrDefault();
                            string btu_text = (er == null) ? "开始评估" : "修改评估";
                            vm.Add(new
                            {
                                TeachClassId = c.TeachClassId,
                                CourseName = c.CourseName,
                                CourseNature = c.CourseNature,
                                TeacherName = c.TeacherName,
                                Score = er.Score,
                                SurveyAnswerStr = er.SurveyAnswerStr,
                                Comment = er.Comment,
                                Btu_Text = btu_text
                            });
                        });
                        lv_TeClass.ItemsSource = vm;
                        //调整列宽  
                        GridView gv = lv_TeClass.View as GridView;
                        if (gv != null)
                        {
                            foreach (GridViewColumn gvc in gv.Columns)
                            {
                                gvc.Width = gvc.ActualWidth;
                                gvc.Width = Double.NaN;
                            }
                        }
                    }
                    break;
                default:
                    Debug.WriteLine(resp.data.ToString(), "RefreshTeachEvaluation异常错误！");
                    break;
            }
        }
        private async Task<bool> Evaluate(EvaluationResult er, bool showMessage)
        {
            HttpMessage resp = await TeachEvaluationHelper.TeachEvaluation(er, loginData.access_token);
            switch (resp.statusCode)
            {
                case HttpStatusCode.OK:
                    if (showMessage)
                    {
                        MessageBox.Show("评估成功！", "教学评估");
                        await RefreshTeachEvaluation();
                    }
                    return true;
                default:
                    if (showMessage)
                    {
                        MessageBox.Show(resp.data.ToString(), "Evaluate异常错误");
                    }
                    return false;
            }
        }
        private async void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (tabControl.SelectedIndex)
                {
                    case 1:
                        await RefreshTeachEvaluation();
                        break;
                    case 2:
                        if (teachClassList == null)
                        {
                            btu_refreshSelectCourse_Click(null, null);
                        }
                        break;
                    case 3:
                        await RefreshSelectCarStatus();
                        break;
                    case 4:
                        await RefreshSheet();
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void btu_refresh_Click(object sender, RoutedEventArgs e)
        {
            await RefreshTeachEvaluation();
            MessageBox.Show("刷新成功！", "提示");
        }

        private async void btu_evaluate_Click(object sender, RoutedEventArgs e)
        {
            var btu = sender as Button;
            string TeachClassId = btu.CommandParameter.ToString();
            TeachClass cla = teachEvaluation.TeachClassList.
                Where(t => t.TeachClassId == TeachClassId).SingleOrDefault();
            EvaluationResult er = teachEvaluation.EvaluationResultList.
                Where(t => t.TeachClassId == TeachClassId).SingleOrDefault();
            if (er == null)
            {
                er = new EvaluationResult
                {
                    TeachClassId = TeachClassId
                };
            }
            EvaluateWindow evaluateWindow = new EvaluateWindow(teachEvaluation.Survey, cla, er);
            evaluateWindow.ShowDialog();
            if (evaluateWindow.Tag != null)
            {
                EvaluationResult newer = evaluateWindow.Tag as EvaluationResult;
                await Evaluate(newer, true);
            }
        }

        private async void btu_EvaluationOneKey_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tryTime = 0;
                tryStart:
                Random rand;
                teachEvaluation.EvaluationResultList = new List<EvaluationResult>();
                teachEvaluation.TeachClassList.ForEach(t =>
                {
                    var temp = new EvaluationResult();
                    temp.TeachClassId = t.TeachClassId;
                    temp.Score = 100;
                    rand = new Random(EncryptHelper.GetRandomSeed());
                    var array = new char[teachEvaluation.Survey.SurveyItemList.Count];
                    var c = 2;
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = '1';
                    }
                    for (int i = 0; i < c; i++)
                    {
                        var index = rand.Next(0, array.Length);
                        array[index] = '2';
                    }
                    temp.SurveyAnswerStr = new string(array);
                    var id = rand.Next(0, StringValues.Comments.Length);
                    temp.Comment = StringValues.Comments[id];
                    teachEvaluation.EvaluationResultList.Add(temp);
                });
                var status = new List<bool>();
                teachEvaluation.EvaluationResultList.ForEach(async t =>
                status.Add(await Evaluate(t, false))
                );
                await RefreshTeachEvaluation();
                if (status.TrueForAll(t => t))
                {
                    MessageBox.Show("一键评估成功！");
                }
                else
                {
                    tryTime++;
                    if (tryTime < 2)
                    {
                        goto tryStart;
                    }
                    else
                    {
                        MessageBox.Show("一键评估失败！有科目没有评估成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btu_refreshSelectCourse_Click(object sender, RoutedEventArgs e)
        {
            selectAcademyCode = null;
            selectMajorCode = null;
            selectGradeCode = null;
            selectCourseNatureCode = null;
            cbx_academy.IsEnabled = false;
            cbx_major.IsEnabled = false;
            cbx_grade.IsEnabled = false;
            cbx_courseNature.IsEnabled = false;
            if (await RefreshAcademy())
            {
                cbx_academy.IsEnabled = true;
                if (await RefreshMajor())
                {
                    cbx_major.IsEnabled = true;
                    if (await RefreshGrade())
                    {
                        cbx_grade.IsEnabled = true;
                        if (await RefreshCourseNature())
                        {
                            cbx_courseNature.IsEnabled = true;
                        }
                    }
                }
            }
            if (sender != null)
            {
                MessageBox.Show("刷新成功！", "提示");
            }
        }
        private async Task<bool> RefreshAcademy()
        {
            HttpMessage resp = await SelectCourseHelper.ListAcadem(loginData.access_token);
            switch (resp.statusCode)
            {
                case HttpStatusCode.OK:
                    academyList = resp.data as List<NameCode>;
                    cbx_academy.ItemsSource = academyList;
                    cbx_academy.SelectedIndex = Convert.ToInt32(userData.AcademyCode) - 1;
                    return true;
                default:
                    Debug.WriteLine(resp.data.ToString(), "RefreshAcademy异常错误");
                    return false;
            }
        }
        private async Task<bool> RefreshMajor()
        {
            HttpMessage resp = await SelectCourseHelper.ListMajor(selectAcademyCode, loginData.access_token);
            switch (resp.statusCode)
            {
                case HttpStatusCode.OK:
                    majorList = resp.data as List<NameCode>;
                    cbx_major.ItemsSource = majorList;
                    var item = majorList.Where(t => t.Code == userData.MajorCode).SingleOrDefault();
                    if (item != null)
                    {
                        var index = majorList.IndexOf(item);
                        cbx_major.SelectedIndex = index;
                    }
                    else
                    {
                        cbx_major.SelectedIndex = 0;
                    }
                    return true;
                default:
                    Debug.WriteLine(resp.data.ToString(), "RefreshMajor异常错误");
                    return false;
            }
        }
        private async Task<bool> RefreshGrade()
        {
            HttpMessage resp = await SelectCourseHelper.ListGrade(selectMajorCode, loginData.access_token);
            switch (resp.statusCode)
            {
                case HttpStatusCode.OK:
                    gradeList = resp.data as List<NameCode>;
                    cbx_grade.ItemsSource = gradeList;
                    var item = gradeList.Where(t => t.Name.StartsWith(userData.Grade)).SingleOrDefault();
                    if (item != null)
                    {
                        var index = gradeList.IndexOf(item);
                        cbx_grade.SelectedIndex = index;
                    }
                    else
                    {
                        cbx_grade.SelectedIndex = 0;
                    }
                    return true;
                default:
                    Debug.WriteLine(resp.data.ToString(), "RefreshMajor异常错误");
                    return false;
            }
        }
        private async Task<bool> RefreshCourseNature()
        {
            HttpMessage resp = await SelectCourseHelper.ListCourseNature(
                selectMajorCode, selectGradeCode, loginData.access_token);
            switch (resp.statusCode)
            {
                case HttpStatusCode.OK:
                    courseNatureList = resp.data as List<NameCode>;
                    cbx_courseNature.ItemsSource = courseNatureList;
                    cbx_courseNature.SelectedIndex = 0;
                    return true;
                default:
                    Debug.WriteLine(resp.data.ToString(), "RefreshCourseNature异常错误");
                    return false;
            }
        }
        private async Task<bool> RefreshTeachClass()
        {
            HttpMessage resp = await SelectCourseHelper.ListTeachClass(
                selectMajorCode, selectGradeCode, selectCourseNatureCode, loginData.access_token);
            switch (resp.statusCode)
            {
                case HttpStatusCode.OK:
                    teachClassList = resp.data as List<TeachClassModel>;
                    selectClassVM = null;
                    lv_selectClass.ItemsSource = selectClassVM;
                    //调整列宽  
                    GridView gv = lv_selectClass.View as GridView;
                    if (gv != null)
                    {
                        foreach (GridViewColumn gvc in gv.Columns)
                        {
                            gvc.Width = gvc.ActualWidth;
                            gvc.Width = Double.NaN;
                        }
                    }
                    return true;
                default:
                    Debug.WriteLine(resp.data.ToString(), "RefreshTeachClass异常错误");
                    return false;
            }
        }
        private async void cbx_academy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (NameCode)cbx_academy.SelectedValue;
            if (item != null)
            {
                selectAcademyCode = item.Code;
                Debug.WriteLine("selectAcademyCode:" + selectAcademyCode);
            }
            else
            {
                selectAcademyCode = null;
            }
            if (selectAcademyCode != null)
            {
                RefreshMajor();
            }
            e.Handled = true;
        }

        private async void cbx_major_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (NameCode)cbx_major.SelectedValue;
            if (item != null)
            {
                selectMajorCode = item.Code;
                Debug.WriteLine("selectMajorCode:" + selectMajorCode);
            }
            else
            {
                selectMajorCode = null;
            }
            if (selectMajorCode != null)
            {
                RefreshGrade();
            }
            e.Handled = true;
        }

        private async void cbx_grade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (NameCode)cbx_grade.SelectedValue;
            if (item != null)
            {
                selectGradeCode = item.Code;
                Debug.WriteLine("selectGradeCode:" + selectGradeCode);
            }
            else
            {
                selectGradeCode = null;
            }
            if (selectGradeCode != null)
            {
                RefreshCourseNature();
            }
            e.Handled = true;
        }

        private void cbx_courseNature_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (NameCode)cbx_courseNature.SelectedValue;
            if (item != null)
            {
                selectCourseNatureCode = item.Code;
                Debug.WriteLine("selectCourseNatureCode:" + selectCourseNatureCode);
            }
            else
            {
                selectCourseNatureCode = null;
            }
            if (selectCourseNatureCode != null)
            {
                RefreshTeachClass();//刷新选课列表
            }
            e.Handled = true;
        }

        private void btu_selectClass_Click(object sender, RoutedEventArgs e)
        {
            var btu = sender as Button;
            var TeachClassId = btu.CommandParameter.ToString();
            if (TeachClassId != null)
            {
                Debug.WriteLine(TeachClassId);
                ValicodeWindow valicodeWindow = new ValicodeWindow(loginData.access_token);
                valicodeWindow.ShowDialog();
                if (ValicodeWindow.Valicode != null)
                {
                    var valicode = ValicodeWindow.Valicode as string;
                    ValicodeWindow.Valicode = null;
                    AddCourse(TeachClassId, valicode);
                }
            }
        }
        private async Task<string> AddCourse(string TeachClassId, string Valicode, bool showMessage = true)
        {
            HttpMessage resp = await SelectCourseHelper.AddCourse(
                TeachClassId, Valicode, loginData.access_token);
            if (resp != null)
            {
                switch (resp.statusCode)
                {
                    case HttpStatusCode.OK:
                        var msg = resp.data.ToString().Replace("\"", "");
                        if (showMessage)
                        {
                            MessageBox.Show(msg, "提示");
                        }
                        else
                        {
                            Debug.WriteLine(msg);
                        }
                        return msg;
                    default:
                        var err = resp.data.ToString().Replace("\"", "");
                        if (showMessage)
                        {
                            MessageBox.Show(err, "AddCourse异常错误");
                        }
                        else
                        {
                            Debug.WriteLine(err);
                        }
                        return null;
                }
            }
            else
            {
                return "选课失败";
            }
        }
        private async Task<string> AutoAddCourse(string TeachClassId, bool showMessage)
        {
            LoadingWindow loadingWindow = new LoadingWindow("正在获取验证码");
            if (showMessage)
            {
                loadingWindow.Show();
            }
            HttpMessage resp = await SelectCourseHelper.GetValiGuid(loginData.access_token);
            switch (resp.statusCode)
            {
                case HttpStatusCode.OK:
                    string guid = resp.data as string;
                    if (guid != null)
                    {
                        Debug.WriteLine(guid);
                        string url_login_vali = SelectCourseHelper.GetValiPicUrl(guid);
                        loadingWindow.StatusText = "正在识别验证码";
                        Debug.WriteLine("正在识别验证码");
                        byte[] pic = await HttpHelper.GetPicBytesAsync(url_login_vali);
                        if (pic != null)
                        {
                            var valicode = await Vercode(pic);
                            if (valicode != null)
                            {
                                loadingWindow.StatusText = "识别成功，正在选课";
                                var result = await AddCourse(TeachClassId, valicode, showMessage);
                                loadingWindow.Close();
                                return result;
                            }
                            else
                            {
                                return null;
                            }

                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                default:
                    Debug.WriteLine(resp.data.ToString(), "异常错误！");
                    return null;
            }
        }
        private async Task<string> Vercode(byte[] file)
        {
            VercodeResult resp = await VercodeHelper.Vercode(file);
            if (resp != null)
            {
                switch (resp.error_code)
                {
                    case 0:
                        return resp.result.ToString();
                        break;
                    default:
                        return null;
                        break;
                }
            }
            else
            {
                return "识别失败";
            }
        }
        private ObservableCollection<SelectClassVM> _selectClassVM = null;

        public ObservableCollection<SelectClassVM> selectClassVM
        {
            get
            {
                if (this._selectClassVM == null)
                {
                    this._selectClassVM = new ObservableCollection<SelectClassVM>();
                    teachClassList.ForEach(
                        t =>
                        {
                            _selectClassVM.Add(new SelectClassVM
                            {
                                TeachClassId = t.TeachClassId,
                                CourseName = t.CourseName,
                                CourseNatureName = t.CourseNatureName,
                                TeacherName = t.TeacherName,
                                PeopleCount = t.SelectedCount + "/" + t.Capacity,
                                PeopleCountColor = (t.SelectedCount == t.Capacity) ?
                                new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black),
                                TotalCredit = t.TotalCredit,
                                TeacherTitle = t.TeacherTitle,
                                Mon = t.Mon,
                                Tue = t.Tue,
                                Wed = t.Wed,
                                Thu = t.Thu,
                                Fri = t.Fri,
                                Sat = t.Sat,
                                Sun = t.Sun,
                                TestDate = t.TestDate,
                                TestPeroid = t.TestPeroid
                            });
                        }
                        );
                }

                return _selectClassVM;
            }
            set
            {
                this._selectClassVM = value;
            }
        }
        public ObservableCollection<SelectClassVM> selectClassCarVM
        {
            get
            {
                var settings = FileHelper.ReadSettings();
                var vm = new ObservableCollection<SelectClassVM>();
                if (settings.SelectCourseCar == null)
                {
                    settings.SelectCourseCar = vm.ToList();
                    FileHelper.WriteSettings(settings);
                }
                else
                {
                    settings.SelectCourseCar.ForEach(t =>
                    {
                        if (t.SelectStatus == null)
                        {
                            t.SelectStatus = "尚未选课";
                        }
                        vm.Add(t);
                    }
                    );
                }
                return vm;
            }
            set
            {
                var vm = value;
                var settings = FileHelper.ReadSettings();
                settings.SelectCourseCar = vm.ToList();
                FileHelper.WriteSettings(settings);
            }
        }
        bool selectClassCarVM_Delete(string TeachClassId)
        {
            try
            {
                var settings = FileHelper.ReadSettings();
                if (settings.SelectCourseCar == null)
                {
                    return true;
                }
                else
                {
                    var item = settings.SelectCourseCar.Where(t => t.TeachClassId == TeachClassId).SingleOrDefault();
                    if (item == null)
                    {
                        return true;
                    }
                    else
                    {
                        settings.SelectCourseCar.Remove(item);
                        FileHelper.WriteSettings(settings);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        bool selectClassCarVM_Update(SelectClassVM vm)
        {
            try
            {
                var settings = FileHelper.ReadSettings();
                if (settings.SelectCourseCar == null)
                {
                    return false;
                }
                else
                {
                    var item = settings.SelectCourseCar.Where(t => t.TeachClassId == vm.TeachClassId).SingleOrDefault();
                    if (item == null)
                    {
                        return false;
                    }
                    else
                    {
                        var index = settings.SelectCourseCar.IndexOf(item);
                        settings.SelectCourseCar[index] = vm;
                        FileHelper.WriteSettings(settings);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void btu_autoSelectClass_Click(object sender, RoutedEventArgs e)
        {
            var btu = sender as Button;
            var TeachClassId = btu.CommandParameter.ToString();
            if (TeachClassId != null)
            {
                Debug.WriteLine(TeachClassId);
                AutoAddCourse(TeachClassId, true);
            }
        }

        private void btu_addToSelectCar_Click(object sender, RoutedEventArgs e)
        {
            Button btu = sender as Button;
            string TeachClassId = btu.CommandParameter as string;
            SelectClassVM cla = selectClassVM.Where(
                t => t.TeachClassId == TeachClassId).SingleOrDefault();
            var settings = FileHelper.ReadSettings();
            if (settings.SelectCourseCar == null)
            {
                settings.SelectCourseCar = new List<SelectClassVM>();
                settings.SelectCourseCar.Add(cla);
                FileHelper.WriteSettings(settings);
            }
            else
            {
                if (settings.SelectCourseCar.Where(t => t.TeachClassId == cla.TeachClassId).Count()
                    == 0)
                {
                    settings.SelectCourseCar.Add(cla);
                    FileHelper.WriteSettings(settings);
                    MessageBox.Show("加入选课车成功");
                }
                else
                {
                    MessageBox.Show("该课程已存在于选课车");
                }
            }
        }

        private async void btu_deleteFromSelectCar_Click(object sender, RoutedEventArgs e)
        {
            var btu = sender as Button;
            var TeachClassId = btu.CommandParameter as string;
            selectClassCarVM_Delete(TeachClassId);
            await RrefreshSelectCar();
        }

        private async Task RrefreshSelectCar()
        {
            lv_selectClassCar.ItemsSource = null;
            lv_selectClassCar.ItemsSource = selectClassCarVM;
            //调整列宽  
            GridView gv = lv_selectClassCar.View as GridView;
            if (gv != null)
            {
                foreach (GridViewColumn gvc in gv.Columns)
                {
                    gvc.Width = gvc.ActualWidth;
                    gvc.Width = Double.NaN;
                }
            }
        }

        private async void btu_refreshSelectCar_Click(object sender, RoutedEventArgs e)
        {
            await RefreshSelectCarStatus();
            MessageBox.Show("刷新成功！", "提示");
        }

        private async void btu_OneKeySelectAllCourse_Click(object sender, RoutedEventArgs e)
        {
            var list = selectClassCarVM.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var cla = list[i];
                if (cla.SelectStatus != "已成功")
                {
                    cla.SelectStatus = "正在识别验证码";
                    selectClassCarVM_Update(cla);
                    await RrefreshSelectCar();
                    var result = await AutoAddCourse(cla.TeachClassId, false);
                    cla.SelectStatus = result;
                    selectClassCarVM_Update(cla);
                    await RrefreshSelectCar();
                }
                RefreshSelectCarStatus(cla);
            }
        }

        private async void btu_refreshSelectedCourse_Click(object sender, RoutedEventArgs e)
        {
            await RefreshSheet();
            MessageBox.Show("刷新成功！", "提示");
        }
        private async Task<bool> RefreshSheet()
        {
            try
            {
                HttpMessage resp = await SelectCourseHelper.ListSheet(loginData.access_token);
                switch (resp.statusCode)
                {
                    case HttpStatusCode.OK:
                        selectedClassList = resp.data as List<TeachClassModel>;
                        selectedClassVM = null;
                        lv_selectedClass.ItemsSource = selectedClassVM;
                        //调整列宽  
                        GridView gv = lv_selectedClass.View as GridView;
                        if (gv != null)
                        {
                            foreach (GridViewColumn gvc in gv.Columns)
                            {
                                gvc.Width = gvc.ActualWidth;
                                gvc.Width = Double.NaN;
                            }
                        }
                        return true;
                    default:
                        Debug.WriteLine(resp.data.ToString(), "RefreshSheet异常错误");
                        return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "RefreshSheet异常错误");
                return false;
            }
        }
        private ObservableCollection<SelectClassVM> _selectedClassVM = null;

        public ObservableCollection<SelectClassVM> selectedClassVM
        {
            get
            {
                if (this._selectedClassVM == null)
                {
                    this._selectedClassVM = new ObservableCollection<SelectClassVM>();
                }
                if (selectedClassList != null)
                {
                    selectedClassList.ForEach(
                        t =>
                        {
                            _selectedClassVM.Add(new SelectClassVM
                            {
                                TeachClassId = t.TeachClassId,
                                CourseName = t.CourseName,
                                CourseNatureName = t.CourseNatureName,
                                TeacherName = t.TeacherName,
                                PeopleCount = t.SelectedCount + "/" + t.Capacity,
                                PeopleCountColor = (t.SelectedCount == t.Capacity) ?
                                new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black),
                                TotalCredit = t.TotalCredit,
                                TeacherTitle = t.TeacherTitle,
                                Mon = t.Mon,
                                Tue = t.Tue,
                                Wed = t.Wed,
                                Thu = t.Thu,
                                Fri = t.Fri,
                                Sat = t.Sat,
                                Sun = t.Sun,
                                TestDate = t.TestDate,
                                TestPeroid = t.TestPeroid
                            });
                        }
                        );
                    return _selectedClassVM;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this._selectedClassVM = value;
            }
        }
        private async void btu_OneKeySelectAllCourseByHand_Click(object sender, RoutedEventArgs e)
        {
            var list = selectClassCarVM.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var cla = list[i];
                if (cla.SelectStatus != "已成功")
                {
                    cla.SelectStatus = "正在输入验证码";
                    selectClassCarVM_Update(cla);
                    await RrefreshSelectCar();
                    ValicodeWindow valicodeWindow = new ValicodeWindow(loginData.access_token, "验证码-" + cla.CourseName);
                    valicodeWindow.ShowDialog();
                    if (ValicodeWindow.Valicode != null)
                    {
                        var valicode = ValicodeWindow.Valicode as string;
                        ValicodeWindow.Valicode = null;
                        var result = await AddCourse(cla.TeachClassId, valicode);
                        Debug.WriteLine(result);
                        cla.SelectStatus = result;
                        selectClassCarVM_Update(cla);
                        await RrefreshSelectCar();
                    }
                }
                RefreshSelectCarStatus(cla);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            lv_selectedClass.Height = tabControl.ActualHeight - selected_head.ActualHeight - 40;
            lv_selectClassCar.Height = tabControl.ActualHeight - selectCar_head.ActualHeight - 40;
            lv_selectClass.Height = tabControl.ActualHeight - selectCourse_head.ActualHeight - 40;
        }

        private void selected_head_Loaded(object sender, RoutedEventArgs e)
        {
            Window_SizeChanged(sender, null);
        }

        private void selectCar_head_Loaded(object sender, RoutedEventArgs e)
        {
            Window_SizeChanged(sender, null);
        }

        private void selectCourse_head_Loaded(object sender, RoutedEventArgs e)
        {
            Window_SizeChanged(sender, null);
        }

        private async void btu_removeCourse_Click(object sender, RoutedEventArgs e)
        {
            var btu = sender as Button;
            var TeachClassId = btu.CommandParameter.ToString();
            if (TeachClassId != null)
            {
                Debug.WriteLine(TeachClassId);
                await RemoveCourse(TeachClassId, true);
            }
            await RefreshSheet();
        }
        private async Task<string> RemoveCourse(string TeachClassId, bool showMessage = true)
        {
            try
            {
                HttpMessage resp = await SelectCourseHelper.RemoveCourse(
                    TeachClassId, loginData.access_token);
                switch (resp.statusCode)
                {
                    case HttpStatusCode.OK:
                        var msg = resp.data.ToString().Replace("\"", "");
                        if (showMessage)
                        {
                            MessageBox.Show(msg, "提示");
                        }
                        else
                        {
                            Debug.WriteLine(msg);
                        }
                        return msg;
                    default:
                        var err = resp.data.ToString().Replace("\"", "");
                        if (showMessage)
                        {
                            MessageBox.Show(err, "RemoveCourse异常错误");
                        }
                        else
                        {
                            Debug.WriteLine(err);
                        }
                        return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        private async Task RefreshSelectCarStatus(SelectClassVM item = null)
        {
            try
            {
                await RefreshSheet();
                if (item == null)
                {
                    var selected = selectedClassVM.ToList();
                    var list = selectClassCarVM.ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        var cla = list[i];
                        var count = selected.Where(t => t.TeachClassId == cla.TeachClassId).Count();
                        cla.SelectStatus = (count == 0) ? "未选上" : "已成功";
                        selectClassCarVM_Update(cla);
                        await RrefreshSelectCar();
                    }
                }
                else
                {
                    var selected = selectedClassVM.ToList();
                    var count = selected.Where(t => t.TeachClassId == item.TeachClassId).Count();
                    item.SelectStatus = (count == 0) ? "未选上" : "已成功";
                    selectClassCarVM_Update(item);
                    await RrefreshSelectCar();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void btu_selectClassInCar_Click(object sender, RoutedEventArgs e)
        {
            btu_selectClass_Click(sender, e);
            RefreshSelectCarStatus();
        }

        private void btu_autoSelectClassInCar_Click(object sender, RoutedEventArgs e)
        {
            btu_autoSelectClass_Click(sender, e);
            RefreshSelectCarStatus();
        }
    }
}
