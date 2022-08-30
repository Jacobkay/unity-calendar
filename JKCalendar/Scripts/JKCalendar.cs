using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(JKCalendarModel))]
public class JKCalendar : MonoBehaviour
{
    /// <summary>
    /// ���ݸ���ʱ���ɻ�ȡ��ÿһ�����ڣ���������в���
    /// </summary>
    public event Action<JKCalendarDayItem> UpdateDateEvent;
    /// <summary>
    /// ���Ի�ȡ�������ĳһ��
    /// </summary>
    public event Action<JKCalendarDayItem> ChoiceDayEvent;
    /// <summary>
    /// ѡ������ʱ���¼�
    /// </summary>
    public event Action<JKCalendarDayItem, JKCalendarDayItem> RangeTimeEvent;
    /// <summary>
    /// �������ؽ���
    /// </summary>
    public event Action CompleteEvent;
    /// <summary>
    /// ��ȡ��ǰѡ�е������
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
    /// ���
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
        // ����ʱ�Զ���ʼ��
        if (JKCalendarModel.awake2Init)
        {
            Init();
        }
    }
    /// <summary>
    /// ��������ʱ���ʼ��
    /// </summary>
    public void Init()
    {
        JKCalendarController.InitDate(DateTime.Now);
    }
    /// <summary>
    /// ����DateTime��ʽ��ʼ������
    /// </summary>
    public void Init(DateTime dateTime)
    {
        JKCalendarController.InitDate(dateTime);
    }
    /// <summary>
    /// ����YYYY-MM-DD��ʽ��ʼ������
    /// </summary>
    public void Init(string dateTime)
    {
        string[] dateTimes = dateTime.Split('-');
        JKCalendarController.InitDate(new DateTime(int.Parse(dateTimes[0]), int.Parse(dateTimes[1]), int.Parse(dateTimes[2])));
    }

    /// <summary>
    /// �л�ʱ��
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
    /// ���ڵ��
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
    /// ���ؽ���
    /// </summary>
    public void DateComplete()
    {
        if (null != CompleteEvent)
        {
            CompleteEvent.Invoke();
        }
    }
    /// <summary>
    /// ��������ѡ��
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
    /// ��ʾ����
    /// </summary>
    public void Show()
    {
        JKCalendarController.Show();
    }
    /// <summary>
    /// ���ص���
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
