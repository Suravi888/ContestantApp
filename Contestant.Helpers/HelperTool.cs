using Contestant.ICommon.Interface;
using Contestant.IDataRepository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contestant.Helpers
{
    public class HelperTool : IHelperTool
    {
        private readonly IBaseRepository repo;

        public HelperTool(IBaseRepository _repo)
        {
            repo = _repo;
        }

        //general
        public int GetPageSkip(int pq_curPage, int pq_rPP, int totalRecords)
        {
            int skip = (pq_rPP * (pq_curPage - 1));
            if (skip >= totalRecords)
            {
                pq_curPage = (int)Math.Ceiling(((double)totalRecords) / pq_rPP);
                skip = (pq_rPP * (pq_curPage - 1));
            }
            skip = skip < 0 ? 0 : skip;
            return skip;
        }

        //date miti
        public DateTime GetAppEngDate()
        {
            return DateTime.Now;
        }

        //crypto
        public string EncodeBase64(string Text)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(Text);
                var base64 = Convert.ToBase64String(bytes);
                return base64;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public string DecodeBase64(string Text)
        {
            try
            {
                var data = Convert.FromBase64String(Text);
                string str = Encoding.UTF8.GetString(data);
                return str;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        ////only used to store data in db
        //public async Task<DateTime> GetDateConversionToSave(string date)
        //{
        //    if (await GetOprDate() == OprDateType.English)
        //    {
        //        return DateTime.Parse(date);
        //    }
        //    else
        //    {
        //        return GetDateInAD(date);
        //    }
        //}

        ////get date based on opr date type
        //public async Task<string> GetDateConversionToDisplay(string date)
        //{
        //    var oprDate = await GetOprDate();
        //    if (date == null || date == string.Empty)
        //    {
        //        return date;
        //    }
        //    if (oprDate == OprDateType.English)
        //    {
        //        return date;
        //    }
        //    else
        //    {
        //        return GetDateInBS(DateTime.Parse(date));
        //    }
        //}

        ////Converts BS to AD
        //public DateTime GetDateInAD(string dateInBS)
        //{
        //    DateTime dateAD;
        //    var k = new string[3];
        //    var d = new string[3];
        //    int bsYear = 0, bsMonth = 0, bsDay = 0;
        //    if ((dateInBS.Contains("-")) || (dateInBS.Contains("/")))
        //    {
        //        if (dateInBS.Contains("-"))
        //        {
        //            d = dateInBS.Split("-");
        //            if (d.Length == 3)
        //            {
        //                dateInBS = d[0] + '/' + d[1] + '/' + d[2];
        //            }
        //        }
        //        if (dateInBS.Contains("/"))
        //        {
        //            k = dateInBS.Split("/");
        //            if (k.Length == 3)
        //            {
        //                bsYear = (int.TryParse(k[0], out int yr)) == true ? yr : 0;
        //                bsMonth = (int.TryParse(k[1], out int mm)) == true ? mm : 0;
        //                bsDay = (int.TryParse(k[2], out int dd)) == true ? dd : 0;
        //            }
        //        }
        //    }
        //    if (bsYear <= 0 || bsMonth <= 0 || bsDay <= 0 || bsMonth > 12 || bsDay > 32)
        //    {
        //        return DateTime.MinValue;
        //    }
        //    int DayOfYearInBS = 0;
        //    int yearInAD = bsYear - 57;
        //    int[] bsDateData = DateArray.DateDataArray(yearInAD + 1);
        //    if (bsDateData == null) { return DateTime.MinValue; }
        //    for (int j = 3; j <= bsMonth + 1; j++)
        //    {
        //        DayOfYearInBS += bsDateData[j];
        //    }
        //    DayOfYearInBS += bsDay;
        //    int initMonthInAD = 4;
        //    int[] months = new int[] { 0, 30, 31, 30, 31, 31, 30, 31, 30, 31, 31, 28, 31, 30 };
        //    int[] leapmonths = new int[] { 0, 30, 31, 30, 31, 31, 30, 31, 30, 31, 31, 29, 31, 30 };
        //    int initDaysInMonthAD = months[1];
        //    int adTempDays = months[1] - bsDateData[0] + 1;

        //    for (int i = 2; DayOfYearInBS > adTempDays; i++)
        //    {
        //        if (DateTime.IsLeapYear(yearInAD))
        //        {
        //            adTempDays += leapmonths[i];
        //            initDaysInMonthAD = leapmonths[i];
        //        }
        //        else
        //        {
        //            adTempDays += months[i];
        //            initDaysInMonthAD = months[i];
        //        }
        //        initMonthInAD++;
        //        if (initMonthInAD > 12)
        //        {
        //            initMonthInAD -= 12;
        //            yearInAD++;
        //        }
        //    }
        //    int dayInAD = initDaysInMonthAD - (adTempDays - DayOfYearInBS);
        //    string finalMonthInAD = (initMonthInAD < 10) ? "0" + initMonthInAD : initMonthInAD.ToString();
        //    string finaldayInAD = (dayInAD < 10) ? "0" + dayInAD : dayInAD.ToString();
        //    string dateInAD = String.Format("{0}/{1}/{2}", yearInAD, finalMonthInAD, finaldayInAD);
        //    bool hasDate = DateTime.TryParse(dateInAD, out dateAD);
        //    if (hasDate == true)
        //    {
        //        return dateAD;
        //    }
        //    else return DateTime.MinValue;
        //}

        //public int GetYear(DateTime dateInAD, OprDateType opr)
        //{
        //    if (opr == OprDateType.English)
        //    {
        //        return dateInAD.Year;
        //    }
        //    else
        //    {
        //        string dateInBS = GetDateInBS(dateInAD);
        //        string[] k = new string[] { };
        //        if (dateInBS != null && dateInBS.Contains("-"))
        //        {
        //            k = dateInBS.Split('-');
        //        }
        //        int year;
        //        bool check = int.TryParse(k[0], out year);
        //        return (check == true ? year : dateInAD.Year);
        //    }
        //}

        //public int GetDay(DateTime dateInAD, OprDateType opr)
        //{
        //    if (opr == OprDateType.English)
        //    {
        //        return dateInAD.Day;
        //    }
        //    else
        //    {
        //        string dateInBS = GetDateInBS(dateInAD);
        //        string[] k = new string[] { };
        //        if (dateInBS != null && dateInBS.Contains("-"))
        //        {
        //            k = dateInBS.Split('-');
        //        }
        //        int day;
        //        bool check = int.TryParse(k[2], out day);
        //        return (check == true ? day : dateInAD.Day);
        //    }
        //}

        ////Converts AD to BS
        //public string GetDateInBS(DateTime dateInAD)
        //{
        //    //Getting BS date data for BS date calculation
        //    int yearInAD = dateInAD.Year;
        //    int[] bsFirstData = DateArray.DateDataArray(yearInAD);
        //    int[] bsSecondData = DateArray.DateDataArray(yearInAD + 1);
        //    //Getting AD day of the year
        //    int dayOfYearInAD = dateInAD.DayOfYear;

        //    //Initializing BS Year from the data
        //    int yearInBS = bsFirstData[1];

        //    //Initializing BS month to Poush          
        //    int initMonthInBS = 9;

        //    //Initializing BS DaysInMonth with total days in the month of Poush
        //    int bsDaysInMonth = bsFirstData[11];

        //    //Initializing temp nepali days
        //    //This is sum of total days in each BS month starting Jan 1 in month Poush
        //    int bsTempDays = bsFirstData[11] - bsFirstData[2] + 1;

        //    List<int> fromBSFirstData = new List<int>();
        //    //generates array of days from poush to chait
        //    for (int p = 11; p < bsFirstData.Length; p++)
        //    {
        //        fromBSFirstData.Add(bsFirstData[p]);
        //    }
        //    List<int> fromBSSecondData = new List<int>();
        //    //generate array of days from baisakh to poush
        //    for (int q = 3; q < 12; q++)
        //    {
        //        fromBSSecondData.Add(bsSecondData[q]);
        //    }
        //    //  generate array of days in BS months
        //    int[] newBSDateArray = fromBSFirstData.Concat(fromBSSecondData).ToArray();

        //    //Looping through BS date data array to get exact BS month, BS year & BS daysInMonth information
        //    for (int i = 1; dayOfYearInAD > bsTempDays; i++)
        //    {
        //        bsTempDays += newBSDateArray[i];
        //        bsDaysInMonth = newBSDateArray[i];
        //        initMonthInBS++;

        //        if (initMonthInBS > 12)
        //        {
        //            initMonthInBS -= 12;
        //            yearInBS++;
        //        }
        //    }
        //    //Calculating BS day  
        //    int dayInBS = bsDaysInMonth - (bsTempDays - dayOfYearInAD);
        //    string finalMonthInBS = (initMonthInBS < 10) ? "0" + initMonthInBS : initMonthInBS.ToString();
        //    string finaldayInBS = (dayInBS < 10) ? "0" + dayInBS : dayInBS.ToString();
        //    string dateInBS = String.Format("{0}-{1}-{2}", yearInBS, finalMonthInBS, finaldayInBS);
        //    return dateInBS;
        //}

        public bool IsApprovalRequired(string ControllerName)
        {
            return true;
            //request from auth
            //ApplicationMenu model = repo.GetModelList<ApplicationMenu>().Where(a => a.Controller == ControllerName).AsNoTracking().FirstOrDefault();
            //return model.RequiredApproval;
        }

        //public async Task<OprDateType> GetOprDate()
        //{
        //    var globalsetting = await repo.GetModelList<HRGlobalPara>().FirstOrDefaultAsync();
        //    if (globalsetting != null)
        //    {
        //        return globalsetting.OprDateType;
        //    }
        //    else
        //    {
        //        return OprDateType.English;
        //    }
        //}

        //public string GetDocPath(string Extension)
        //{
        //    if (Extension == "pdf")
        //    {
        //        return Path.Combine("/images/pdf.png");
        //    }
        //    else if (Extension == "jpg" || Extension == "png" || Extension == "jpeg")
        //    {
        //        return Path.Combine("/images/image.png");
        //    }
        //    else if (Extension == "xls" || Extension == "xlm" || Extension == "xlsx" || Extension == "xlsb")
        //    {
        //        return Path.Combine("/images/excel.png");
        //    }
        //    else if (Extension == "doc" || Extension == "dot" || Extension == "docx" || Extension == "docm")
        //    {
        //        return Path.Combine("/images/doc.png");
        //    }
        //    else
        //    {
        //        return Path.Combine("/images/txt.png");
        //    }
        //}

        //public async Task<bool> GetMigrationMode()
        //{
        //    var globalSetting = await repo.GetModelList<HRGlobalPara>().Select(n => new HRGlobalPara
        //    {
        //        IsMigrationMode = n.IsMigrationMode
        //    }).SingleOrDefaultAsync();
        //    if (globalSetting != null)
        //    {
        //        return globalSetting.IsMigrationMode;
        //    }
        //    else return false;
        //}

        //public int GetUserMapId(int LoginId)
        //{
        //    var data = repo.GetModelList<SyncApplicationUser>().Where(a => a.Id == LoginId).FirstOrDefault();
        //    int userMapId = data.UserMapId ?? 0;
        //    return userMapId;
        //}

        //public string GetLetterContent(LetterAttribute model, int templateId)
        //{
        //    var template = repo.GetModelList<LetterTemplate>().Where(x => x.Id == templateId).FirstOrDefault();

        //    //Incase of new enum (in LetterAttributes) added to Letter Data , replace here too .. for e.g  "[[YourEnum]]"
        //    var content = template.Content;
        //    content = Regex.Replace(content, @"\[\[AppointmentDate]]", model.AppointmentDate != null ? model.AppointmentDate : "");
        //    content = Regex.Replace(content, @"\[\[ActionOnExplanationDate]]", model.ActionOnExplanationDate != null ? model.ActionOnExplanationDate : "");
        //    content = Regex.Replace(content, @"\[\[ActionOnExplanationDesc]]", model.ActionOnExplanationDesc != null ? model.ActionOnExplanationDesc : "");
        //    content = Regex.Replace(content, @"\[\[ActionOnSuspensionDate]]", model.ActionOnSuspensionDate != null ? model.ActionOnSuspensionDate : "");
        //    content = Regex.Replace(content, @"\[\[ActionOnSuspensionDesc]]", model.ActionOnSuspensionDesc != null ? model.ActionOnSuspensionDesc : "");
        //    content = Regex.Replace(content, @"\[\[ExplanationAskingDate]]", model.ExplanationAskingDate != null ? model.ExplanationAskingDate : "");
        //    content = Regex.Replace(content, @"\[\[ExplanationAskingDesc]]", model.ExplanationAskingDesc != null ? model.ExplanationAskingDesc : "");
        //    content = Regex.Replace(content, @"\[\[BranchName]]", model.BranchName != null ? model.BranchName : "");
        //    content = Regex.Replace(content, @"\[\[BranchNameLocLang]]", model.BranchNameLocLang != null ? model.BranchNameLocLang : "");
        //    content = Regex.Replace(content, @"\[\[CurrentDate]]", DateTime.Now.Date.ToString("yyyy/MM/dd"));
        //    content = Regex.Replace(content, @"\[\[Designation]]", model.Designation != null ? model.Designation : "");
        //    content = Regex.Replace(content, @"\[\[DesignationLocalLang]]", model.DesignationLocalLang != null ? model.DesignationLocalLang : "");
        //    content = Regex.Replace(content, @"\[\[DesignationChangeStatus]]", model.DesignationChangeStatus != null ? model.DesignationChangeStatus : "");
        //    content = Regex.Replace(content, @"\[\[Department]]", model.Department != null ? model.Department : "");
        //    content = Regex.Replace(content, @"\[\[DepartmentLocLang]]", model.DepartmentLocLang != null ? model.DepartmentLocLang : "");
        //    content = Regex.Replace(content, @"\[\[Employee]]", model.EmployeeName != null ? model.EmployeeName : "");
        //    content = Regex.Replace(content, @"\[\[EmployeeNameLocLang]]", model.EmployeeNameLocLang != null ? model.EmployeeNameLocLang : "");
        //    content = Regex.Replace(content, @"\[\[ExpectedDepartureDate]]", model.ExpectedDepartureDate != null ? model.ExpectedDepartureDate : "");
        //    content = Regex.Replace(content, @"\[\[FunctionalDesignation]]", model.FunctionalDesignation != null ? model.FunctionalDesignation : "");
        //    content = Regex.Replace(content, @"\[\[FunctionalDesignationLocLang]]", model.FunctionalDesignationLocLang != null ? model.FunctionalDesignationLocLang : "");
        //    content = Regex.Replace(content, @"\[\[MaxAnsweringDays]]", model.MaxAnsweringDays != null ? model.MaxAnsweringDays : "");
        //    content = Regex.Replace(content, @"\[\[ExplanationSubmissionDate]]", model.ExplanationSubmissionDate != null ? model.ExplanationSubmissionDate : "");
        //    content = Regex.Replace(content, @"\[\[ExplanationSubmissionDesc]]", model.ExplanationSubmissionDesc != null ? model.ExplanationSubmissionDesc : "");
        //    content = Regex.Replace(content, @"\[\[SuspensionDate]]", model.ExplanationSuspensionDate != null ? model.ExplanationSuspensionDate : "");
        //    content = Regex.Replace(content, @"\[\[SuspensionDesc]]", model.ExplanationSuspensionDesc != null ? model.ExplanationSuspensionDesc : "");
        //    content = Regex.Replace(content, @"\[\[OrganizationalUnit]]", model.OrganizationalUnit != null ? model.OrganizationalUnit : "");
        //    content = Regex.Replace(content, @"\[\[ProbationStartDate]]", model.ProbationStartDate != null ? model.ProbationStartDate : "");
        //    content = Regex.Replace(content, @"\[\[ProbationExpiryDate]]", model.ProbationExpiryDate != null ? model.ProbationExpiryDate : "");
        //    content = Regex.Replace(content, @"\[\[ResignationDate]]", model.ResignationDate != null ? model.ResignationDate : "");
        //    content = Regex.Replace(content, @"\[\[ResignationEffectDate]]", model.ResignationEffectDate != null ? model.ResignationEffectDate : "");
        //    content = Regex.Replace(content, @"\[\[Salary]]", model.Salary != null ? model.Salary : "");
        //    content = Regex.Replace(content, @"\[\[SuspenseRefOfficeName]]", model.SuspenseRefOfficeName != null ? model.SuspenseRefOfficeName : "");
        //    content = Regex.Replace(content, @"\[\[SuspenseRefOfficeLetterNo]]", model.SuspenseRefOfficeLetterNo != null ? model.SuspenseRefOfficeLetterNo : "");
        //    content = Regex.Replace(content, @"\[\[SuspenseRefOfficeLetterDate]]", model.SuspenseRefOfficeLetterDate != null ? model.SuspenseRefOfficeLetterDate : "");
        //    content = Regex.Replace(content, @"\[\[TransferType]]", model.TransferType != null ? model.TransferType : "");
        //    content = Regex.Replace(content, @"\[\[FromBranch]]", model.FromBranch != null ? model.FromBranch : "");
        //    content = Regex.Replace(content, @"\[\[ToBranch]]", model.ToBranch != null ? model.ToBranch : "");
        //    content = Regex.Replace(content, @"\[\[DepartureDate]]", model.DepartureDate != null ? model.DepartureDate : "");
        //    content = Regex.Replace(content, @"\[\[ExpectedOfficeJoinDate]]", model.ExpectedOfficeJoinDate != null ? model.ExpectedOfficeJoinDate : "");
        //    content = Regex.Replace(content, @"\[\[OfficeAttendEndDate]]", model.OfficeAttendEndDate != null ? model.OfficeAttendEndDate : "");
        //    content = Regex.Replace(content, @"\[\[TransferDate]]", model.TransferDate != null ? model.TransferDate : "");
        //    content = Regex.Replace(content, @"\[\[TOREffectDate]]", model.TOREffectDate != null ? model.TOREffectDate : "");
        //    content = Regex.Replace(content, @"\[\[TORDescription]]", model.TORDescription != null ? model.TORDescription : "");
        //    content = Regex.Replace(content, @"\[\[TORDescriptionLocLang]]", model.TORDescriptionLocLang != null ? model.TORDescriptionLocLang : "");
        //    content = Regex.Replace(content, @"\[\[ActionDate]]", model.ActionDate != null ? model.ActionDate : "");
        //    content = Regex.Replace(content, @"\[\[ActingEffectDate]]", model.ActingEffectDate != null ? model.ActingEffectDate : "");
        //    content = Regex.Replace(content, @"\[\[ActingEffectEndDate]]", model.ActingEffectEndDate != null ? model.ActingEffectEndDate : "");
        //    content = Regex.Replace(content, @"\[\[ActingCoreDesignation]]", model.ActingCoreDesignation != null ? model.ActingCoreDesignation : "");
        //    content = Regex.Replace(content, @"\[\[ActingFunctionalDesignation]]", model.ActingFunctionalDesignation != null ? model.ActingFunctionalDesignation : "");
        //    content = Regex.Replace(content, @"\[\[ActingUnit]]", model.ActingUnit != null ? model.ActingUnit : "");
        //    content = Regex.Replace(content, @"\[\[DepartureDate]]", model.DepartureDate != null ? model.DepartureDate : "");
        //    content = Regex.Replace(content, @"\[\[OfficiatingEffectDate]]", model.OfficiatingEffectDate != null ? model.OfficiatingEffectDate : "");
        //    content = Regex.Replace(content, @"\[\[OfficiatingEndDate]]", model.OfficiatingEndDate != null ? model.OfficiatingEndDate : "");
        //    content = Regex.Replace(content, @"\[\[OfficiatingEmployeeName]]", model.OfficiatingEmployeeName != null ? model.OfficiatingEmployeeName : "");
        //    content = Regex.Replace(content, @"\[\[OfficiatingEmployeeNameLocLang]]", model.OfficiatingEmployeeNameLocLang != null ? model.OfficiatingEmployeeNameLocLang : "");
        //    content = Regex.Replace(content, @"\[\[OfficiatingDesignation]]", model.OfficiatingDesignation != null ? model.OfficiatingDesignation : "");
        //    return content;
        //}

        //public async Task<string> GetFiscalYear(DateTime date)
        //{
        //    int givenYear, givenMonth, yearAfterGivenYear, yearBeforeGivenYear;
        //    string requiredFiscalYear, possibleFiscalYearOne, possibleFiscalYearTwo;

        //    string appointmentDate = GetDateInBS(date);
        //    string[] k = appointmentDate.Split('-');
        //    givenYear = int.Parse(k[0]);
        //    givenMonth = int.Parse(k[1]);
        //    yearAfterGivenYear = givenYear + 1;
        //    yearBeforeGivenYear = givenYear - 1;
        //    //if fiscal year comprises of two different years. (ex:2075/76=>{2-1, 3-2, 4-3})
        //    possibleFiscalYearOne = (yearBeforeGivenYear + "/" + givenYear % 100); //one possible fiscal year for appointed year
        //    possibleFiscalYearTwo = (givenYear + "/" + yearAfterGivenYear % 100);  // another possible fiscal year for appointed year
        //    var durationOfPossibleFiscalYearOne = await repo.GetModelList<FiscalYear>().Where(w => w.Year.Contains(possibleFiscalYearOne)).Select(fm => new
        //    {
        //        StartMonth = fm.FromMonth,
        //        EndMonth = fm.ToMonth,
        //    }).FirstOrDefaultAsync();

        //    //if fiscal year ends within a single year (ex:2075=>{1=12, 4=12, 3-12})
        //    if (durationOfPossibleFiscalYearOne == null)
        //    {
        //        var fiscalYearNew = await repo.GetModelList<FiscalYear>().Where(w => w.Year.Equals(givenYear)).FirstOrDefaultAsync(); //Select(fm => fm.FromMonth).
        //        if (fiscalYearNew != null)
        //        {
        //            requiredFiscalYear = fiscalYearNew.ToString();
        //        }
        //        else
        //        {
        //            requiredFiscalYear = "";
        //        }
        //    }
        //    else
        //    {
        //        requiredFiscalYear = (givenMonth < durationOfPossibleFiscalYearOne.StartMonth && givenMonth <= durationOfPossibleFiscalYearOne.EndMonth) ? possibleFiscalYearOne : possibleFiscalYearTwo;
        //    }

        //    return requiredFiscalYear;
        //}

        //public async Task<int> GetMaxGradeCount(int coreDesignId)
        //{
        //    var maxGrade = await (repo.GetModelList<CoreDesignation>().Where(c => c.Id.Equals(coreDesignId)).FirstOrDefaultAsync());
        //    return maxGrade.MaxGradeCount > 0 ? maxGrade.MaxGradeCount : 0;
        //}
    }
}
