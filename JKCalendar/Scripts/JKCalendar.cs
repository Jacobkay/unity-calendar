using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(JKCalendarModel))]
public class JKCalendar : MonoBehaviour
{
    /// <summary>
    /// 数据更新时，可获取到每一个日期，并对其进行操作
    /// </summary>
    public event Action<JKCalendarDayItem> UpdateDateEvent;
    /// <summary>
    /// 可以获取到点击的某一天
    /// </summary>
    public event Action<JKCalendarDayItem> ChoiceDayEvent;
    /// <summary>
    /// 选择区间时间事件
    /// </summary>
    public event Action<JKCalendarDayItem, JKCalendarDayItem> RangeTimeEvent;
    /// <summary>
    /// 日历加载结束
    /// </summary>
    public event Action CompleteEvent;
    /// <summary>
    /// 获取当前选中的天对象
    /// </summary>
    public JKCalendarDayItem CrtTime { get; set; }
    /// <summary>
    /// model
    /// </summary>
    private JKCalendarModel JKCalendarModel;
    /// <summary>
    /// controller
    /// </summary>
    private JKCalendarController JKCalendarController;
    /// <summary>
    /// 入口
    /// </summary>
    private void Start()
    {
        JKCalendarModel = this.GetComponent<JKCalendarModel>();
        JKCalendarController = new JKCalendarController()
        {
            jkCalendar = this,
            jkCalendarModel = JKCalendarModel,
            pos = this.transform.localPosition
        };
        JKCalendarController.Init();
        // 开启时自动初始化
        if (JKCalendarModel.awake2Init)
        {
            Init();
        }
    }
    /// <summary>
    /// 按照现在时间初始化
    /// </summary>
    public void Init()
    {
        JKCalendarController.InitDate(DateTime.Now);
    }
    /// <summary>
    /// 按照DateTime格式初始化日历
    /// </summary>
    public void Init(DateTime dateTime)
    {
        JKCalendarController.InitDate(dateTime);
    }
    /// <summary>
    /// 按照YYYY-MM-DD格式初始化日历
    /// </summary>
    public void Init(string dateTime)
    {
        string[] dateTimes = dateTime.Split('-');
        JKCalendarController.InitDate(new DateTime(int.Parse(dateTimes[0]), int.Parse(dateTimes[1]), int.Parse(dateTimes[2])));
    }

    /// <summary>
    /// 切换时间
    /// </summary>
    /// <param name="obj"></param>
    public void UpdateDate(JKCalendarDayItem obj)
    {
        if (null != UpdateDateEvent)
        {
            UpdateDateEvent.Invoke(obj);
        }
    }
    /// <summary>
    /// 日期点击
    /// </summary>
    public void DayClick(JKCalendarDayItem dayItem)
    {
        if (null != ChoiceDayEvent)
        {
            ChoiceDayEvent.Invoke(dayItem);
        }
        CrtTime = dayItem;
    }
    /// <summary>
    /// 加载结束
    /// </summary>
    public void DateComplete()
    {
        if (null != CompleteEvent)
        {
            CompleteEvent.Invoke();
        }
    }
    /// <summary>
    /// 区间日期选择
    /// </summary>
    /// <param name="firstDay"></param>
    /// <param name="secondDay"></param>
    public void RangeCalendar(JKCalendarDayItem firstDay, JKCalendarDayItem secondDay )
    {
        if (null != RangeTimeEvent)
        {
            RangeTimeEvent.Invoke(firstDay, secondDay);
        }
    }
    /// <summary>
    /// 显示弹窗
    /// </summary>
    public void Show()
    {
        JKCalendarController.Show();
    }
    /// <summary>
    /// 隐藏弹窗
    /// </summary>
    public void Hide()
    {
        JKCalendarController.Hide();
    }
    private void OnDestroy()
    {
        JKCalendarController = null;
        GC.Collect();
    }
}
