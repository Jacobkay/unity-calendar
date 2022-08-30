using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarTest : MonoBehaviour
{
    public ZCalendar zcalendar;
    // Start is called before the first frame update
    void Awake()
    {
        //zcalendar.RangeTimeEvent += (ZCalendarDayItem d1, ZCalendarDayItem d2) =>
        //{
        //    Debug.Log(d1.Day +"::"+ d2.Day);
        //};
        //zcalendar.ChoiceDayEvent += (item) =>
        //{
        //    Debug.Log(item.Day);
        //};
        //zcalendar.CompleteEvent += () =>
        //{
        //    Debug.Log(zcalendar.CrtTime.Year +"::"+ zcalendar.CrtTime.Month);
        //};
        //zcalendar.UpdateDateEvent += (item) =>
        //{
        //    Debug.Log("¸üÐÂ£º£º" + item.Day);
        //};
    }
}
