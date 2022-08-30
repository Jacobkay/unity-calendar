using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text;

public class JKCalendarDayItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject imgBk;
    public GameObject rangeBk;
    public Text txt;
    public Button btn;
    public Text lunarTxt;
    [HideInInspector]
    public JKCalendarController jkCalendarController;
    private bool isCanClick = true;
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public DateTime dateTime;
    private bool isOn = false;
    public bool IsOn
    {
        set
        {
            if (isOn != value || isOn)
            {
                isOn = value;
                imgBk?.SetActive(value);
                if (value)
                {
                    if (!jkCalendarController.IsInRange)
                    {
                        jkCalendarController.jkCalendar.DayClick(this);
                    }
                    if (jkCalendarController.jkCalendarModel.rangeCalendar)
                    {
                        jkCalendarController.ChangeRangeType(this);
                    }
                    if (jkCalendarController.jkCalendarModel.isPopupCalendar)
                    {
                        jkCalendarController.Hide();
                    }
                }
            }
        }
        get { return isOn; }
    }
    public bool IsOnWithOutEvent
    {
        set
        {
            if (isOn != value)
            {
                isOn = value;
                imgBk?.SetActive(value);
            }
        }
    }
    private bool isRange;
    public bool IsRange
    {
        set
        {
            if (isRange != value)
            {
                isRange = value;
                rangeBk?.SetActive(value);
            }
        }
        get { return isRange; }
    }
    Color greyColor;
    /// <summary>
    /// 初始化日期
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <param name="day">日</param>
    /// <param name="nowTime"></param>
    /// <param name="crtDay">当前天</param>
    public void Init(DateTime dateTime, int crtDay)
    {
        isRange = rangeBk.activeInHierarchy;
        isOn = imgBk.activeInHierarchy;
        IsOnWithOutEvent = false;
        IsRange = false;
        this.dateTime = dateTime;
        this.Year = dateTime.Year;
        this.Month = dateTime.Month;
        this.Day = dateTime.Day;
        txt.text = Day.ToString("00");
        if (!jkCalendarController.jkCalendarModel.rangeCalendar)
        {
            IsOn = Day == crtDay;
        }
        else
        {
            jkCalendarController.jkCalendar.RangeTimeEvent += RangeTimeEvent;
        }
        isCanClick = !jkCalendarController.jkCalendarModel.isStaticCalendar;
        greyColor = jkCalendarController.greyColor.a == 0 ? new Color(txt.color.r, txt.color.g, txt.color.b, 0.1f) : jkCalendarController.greyColor;
        if (!jkCalendarController.jkCalendarModel.isStaticCalendar)
        {
            btn.onClick.AddListener(() =>
            {
                IsOn = true;
            });
            jkCalendarController.jkCalendar.ChoiceDayEvent += ChangeState;
        }
        if (!jkCalendarController.jkCalendarModel.isUnexpiredTimeCanClick)
            IsUnexpiredTime(jkCalendarController.nowTime, dateTime);
        if (jkCalendarController.jkCalendarModel.autoFillDate)
        {
            IsCrtMonth(jkCalendarController.Month);
        }
        if (jkCalendarController.jkCalendarModel.lunar)
        {
            lunarTxt.gameObject.SetActive(true);
            SolarToLunar(dateTime);
        }
    }
    /// <summary>
    /// 关闭可点击权限
    /// </summary>
    public void CloseClickAble()
    {
        isRange = rangeBk.activeInHierarchy;
        isOn = imgBk.activeInHierarchy;
        IsOn = false;
        txt.text = "";
        enabled = false;
        IsOnWithOutEvent = false;
        IsRange = false;
    }
    /// <summary>
     /// 判断是否在选择区间内的时间
     /// </summary>
    public void IsRangeDayItem(JKCalendarDayItem d1, JKCalendarDayItem d2)
    {
        RangeTimeEvent(d1, d2);
        if (DateTime.Compare(d1.dateTime, dateTime) == 0 || DateTime.Compare(d2.dateTime, dateTime) == 0)
        {
            IsOnWithOutEvent = true;
        }
    }
    /// <summary>
    /// 判断当前是否在区域选择时间内
    /// </summary>
    /// <param name="d1"></param>
    /// <param name="d2"></param>
    void RangeTimeEvent(JKCalendarDayItem d1, JKCalendarDayItem d2)
    {
        if (DateTime.Compare(d1.dateTime, dateTime) < 0 && DateTime.Compare(d2.dateTime, dateTime) > 0)
        {
            IsRange = true;
        }
    }
    /// <summary>
    /// 改变当前状态
    /// </summary>
    void ChangeState(JKCalendarDayItem dayItem)
    {
        if (dayItem != this)
        {
            IsOn = false;
            IsRange = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isOn && isCanClick)
        {
            imgBk.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isOn && isCanClick)
        {
            imgBk.SetActive(false);
        }
    }
    /// <summary>
    /// 判断是否超过了今天的时间
    /// </summary>
    void IsUnexpiredTime(DateTime time, DateTime crtTime)
    {
        int compNum = DateTime.Compare(time, crtTime);
        if (compNum < 0)
        {
            btn.interactable = false;
            isCanClick = false;
            txt.color = greyColor;
            lunarTxt.color = greyColor;
        }
    }
    /// <summary>
    /// 判断是否为本月日期
    /// </summary>
    void IsCrtMonth(int time)
    {
        if (time != Month)
        {
            btn.interactable = false;
            isCanClick = false;
            txt.color = greyColor;
            lunarTxt.color = greyColor;
        }
    }
    /// <summary>
    /// 显示农历日期
    /// </summary>
    /// <param name="time"></param>
    void SolarToLunar(DateTime dt)
    {
        int year = jkCalendarController.cncld.GetYear(dt);
        int flag = jkCalendarController.cncld.GetLeapMonth(year);
        int month = jkCalendarController.cncld.GetMonth(dt);
        if (flag > 0)
        {
            if (flag == month)
            {
                //闰月
                month--;
            }
            else if (month > flag)
            {
                month--;
            }
        }
        int day = jkCalendarController.cncld.GetDayOfMonth(dt);
        lunarTxt.text = (day == 1) ? GetLunarMonth(month) : GetLunarDay(day);
        //Debug.Log($"{year}-{(month.ToString().Length == 1 ? "0" + month : month + "")}-{(day.ToString().Length == 1 ? "0" + day : day + "")}");
    }
    string GetLunarMonth(int month)
    {
        if (month < 13 && month > 0)
        {
            return $"{jkCalendarController.lunarMonths[month - 1]}月";
        }

        throw new ArgumentOutOfRangeException("无效的月份!");
    }

    string GetLunarDay(int day)
    {
        if (day > 0 && day < 32)
        {
            if (day != 20 && day != 30)
            {
                return string.Concat(jkCalendarController.lunarDaysT[(day - 1) / 10], jkCalendarController.lunarDays[(day - 1) % 10]);
            }
            else
            {
                return string.Concat(jkCalendarController.lunarDays[(day - 1) / 10], jkCalendarController.lunarDaysT[1]);
            }
        }
        throw new ArgumentOutOfRangeException("无效的日!");
    }
    private void OnDestroy()
    {
        if (!jkCalendarController.jkCalendarModel.isStaticCalendar)
        {
            jkCalendarController.jkCalendar.ChoiceDayEvent -= ChangeState;
        }
        jkCalendarController.jkCalendar.RangeTimeEvent -= RangeTimeEvent;
    }
}
