/*
 * Created by JacobKay - 2022.08.24
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JKCalendarController
{
    public int Year { set; get; }
    public int Month { set; get; }
    public int Day { set; get; }
    /// <summary>
    /// ��ǰ�Ƿ�������ѡ��״̬
    /// </summary>
    private bool isInRange = false;
    public bool IsInRange { get { return isInRange; } }
    private string week;
    private DateTime now;
    private int days;
    /// <summary>
    /// ��ǰѡ�е�λ��
    /// </summary>
    public Vector3 pos;
    private int lastMonthDays;
    private int nextMonthDays;
    public JKCalendar jkCalendar;
    public JKCalendarModel jkCalendarModel;
    public DateTime nowTime = DateTime.Now;
    private int lastMonthEmptyDays;
    bool isShow = true;
    bool isInit = false;
    /// <summary>
    /// ����������ɫ
    /// </summary>
    public Color greyColor;

    public System.Globalization.ChineseLunisolarCalendar cncld = new System.Globalization.ChineseLunisolarCalendar();
    /// <summary>
    /// ũ����
    /// </summary>
    public string[] lunarMonths = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "ʮ", "ʮһ", "��" };

    public string[] lunarDaysT = { "��", "ʮ", "إ", "��" };

    /// <summary>
    /// ũ����
    /// </summary>
    public string[] lunarDays = { "һ", "��", "��", "��", "��", "��", "��", "��", "��", "ʮ" };
    DateTime monthFirstDay;
    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="date"></param>
    public void Init()
    {
        jkCalendarModel.JKCalendarController = this;
        jkCalendarModel.Init();
        if (jkCalendarModel.isStaticCalendar) return;
        // ��̬�������ɹر�
        if (jkCalendarModel.isPopupCalendar)
        {
            jkCalendarModel.btnClose.onClick.AddListener(() =>
            {
                Hide();
            });
        }
        jkCalendarModel.btnLastYear.onClick.AddListener(LastYear);
        jkCalendarModel.btnNextYear.onClick.AddListener(NextYear);
        jkCalendarModel.btnLastMonth.onClick.AddListener(LastMonth);
        jkCalendarModel.btnNextMonth.onClick.AddListener(NextMonth);
    }

    /// <summary>
    /// ���չ涨ʱ���ʼ������
    /// </summary>
    public void InitDate(DateTime date)
    {
        now = date;
        DestroyAllChildren();
        UpdateYear();
        UpdateMonth();
        UpdateDays();
        UpdateData();
        if (!isInit)
        {
            isInit = true;
            jkCalendar.DateComplete();
        }
    }
    void LastYear()
    {
        now = now.AddYears(-1);
        DestroyAllChildren();
        UpdateYear();
        UpdateMonth();
        UpdateDays();
        UpdateData();
    }
    void NextYear()
    {
        now = now.AddYears(1);
        DestroyAllChildren();
        UpdateYear();
        UpdateMonth();
        UpdateDays();
        UpdateData();
    }
    void LastMonth()
    {
        now = now.AddMonths(-1);
        DestroyAllChildren();
        UpdateYear();
        UpdateMonth();
        UpdateDays();
        UpdateData();
    }
    void NextMonth()
    {
        now = now.AddMonths(1);
        DestroyAllChildren();
        UpdateYear();
        UpdateMonth();
        UpdateDays();
        UpdateData();
    }

    List<JKCalendarDayItem> dayItemList = new List<JKCalendarDayItem>();

    /// <summary>
    /// ���������������ѡ��ʱ��ʱ����Ҫ�жϵ�ǰ����ѡ��״̬
    /// </summary>
    /// <returns></returns>
    public void ChangeRangeType(JKCalendarDayItem dayItem)
    {
        isInRange = !isInRange;
        if (dayItemList.Count >= 2)
        {
            dayItemList.Clear();
        }
        if (dayItemList.Count == 0)
        {
            dayItemList.Add(dayItem);
        }
        else
        {
            int compare = DateTime.Compare(dayItemList[0].dateTime, dayItem.dateTime);
            if (compare <= 0)
            {
                dayItemList.Add(dayItem);
            }
            else
            {
                dayItemList.Insert(0, dayItem);
            }
        }
        if (!isInRange)
        {
            jkCalendar.RangeCalendar(dayItemList[0], dayItemList[1]);
        }
    }
    /// <summary>
    /// ��ʾ����
    /// </summary>
    public void Show()
    {
        if (pos != null && !isShow)
        {
            isShow = true;
            jkCalendar.transform.localPosition = pos;
        }
    }
    /// <summary>
    /// ��������
    /// </summary>
    public void Hide()
    {
        if (isShow && !isInRange)
        {
            isShow = false;
            Debug.Log("hide");
            jkCalendar.transform.localPosition = new Vector3(pos.x, 5000, pos.z);
        }
    }
    /// <summary>
    /// ��ѯ������
    /// </summary>
    void UpdateYear()
    {
        Year = now.Year;
    }
    /// <summary>
    /// ��ѯ������
    /// </summary>
    void UpdateMonth()
    {
        Month = int.Parse(now.Month.ToString("00"));
    }
    /// <summary>
    /// ����Ҫ��ѯ����
    /// </summary>
    /// <returns></returns>
    void UpdateDays()
    {
        days = DateTime.DaysInMonth(Year, Month);
        if (Day == 0)
        {
            Day = now.Day;
        }
        else if (Day > days)
        {
            Day = days;
        }
    }
    /// <summary>
    /// ������ʾ�·�
    /// </summary>
    void UpdateData()
    {
        jkCalendarModel.SetTimeTxt(Year, Month);
        FillLastMonth();
        for (int i = 0; i < days; i++)
        {
            AddDayItem(monthFirstDay.AddDays(i));
        }
        FillNextMonth();
    }
    /// <summary>
    /// �Զ�����ϸ�������
    /// </summary>
    void FillLastMonth()
    {
        monthFirstDay = new DateTime(Year, Month, 1);
        lastMonthEmptyDays = GetLastMonthDays();
        if (jkCalendarModel.autoFillDate)
        {
            for (int i = lastMonthEmptyDays; i > 0; i--)
            {
                AddDayItem(monthFirstDay.AddDays(-i));
            }
        }
        else
        {
            for (int i = 0; i < lastMonthEmptyDays; i++)
            {
                JKCalendarDayItem dayItem = jkCalendarModel.Instantiate();
                dayItem.jkCalendarController = this;
                dayItem.CloseClickAble();
            }
        }
    }
    /// <summary>
    /// ����¸��µ�ʱ��
    /// </summary>
    void FillNextMonth()
    {
        int nextMonthDays = 42 - (lastMonthEmptyDays + days);
        if (nextMonthDays != 0)
        {
            if (jkCalendarModel.autoFillDate)
            {
                DateTime lastDay = monthFirstDay.AddDays(days);
                for (int i = 0; i < nextMonthDays; i++)
                {
                    AddDayItem(lastDay.AddDays(i));
                }
            }
        }
    }
    /// <summary>
    /// ������ڶ���
    /// </summary>
    void AddDayItem(DateTime dateTime)
    {
        JKCalendarDayItem dayItem = jkCalendarModel.Instantiate();
        dayItem.jkCalendarController = this;
        dayItem.Init(dateTime, Day);
        jkCalendar.UpdateDate(dayItem);
        if (!isInRange && dayItemList.Count > 0)
        {
            dayItem.IsRangeDayItem(dayItemList[0], dayItemList[1]);
        }
    }
    /// <summary>
    /// �ж���һ�����м���
    /// </summary>
    /// <returns></returns>
    int GetLastMonthDays()
    {
        string firstWeek = new DateTime(Year, Month, 1).DayOfWeek.ToString();
        return (int)Enum.Parse(typeof(DayOfWeek), firstWeek);
    }
    /// <summary>
    /// ɾ����������
    /// </summary>
    void DestroyAllChildren()
    {
        List<Transform> lst = new List<Transform>();
        int count = jkCalendarModel.dayContent.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = jkCalendarModel.dayContent.GetChild(i);
            lst.Add(child);
        }
        for (int i = 0; i < lst.Count; i++)
        {
            MonoBehaviour.Destroy(lst[i].gameObject);
        }
    }
}
