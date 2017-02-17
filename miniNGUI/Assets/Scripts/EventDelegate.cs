using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventDelegate {

    public bool oneShot = false;

	// execute list of delegates
    static public void Execute(List<EventDelegate> list)
    {
        if (list != null)
        {
            for (int i = 0; i < list.Count; )
            {
                EventDelegate del = list[i];

                if (del != null)
                {
                    //del.Execute();

                    if (i >= list.Count) break;
                    if (list[i] != del) continue;

                    if (del.oneShot)
                    {
                        list.RemoveAt(i);
                    }
                }

            }
        }
    }


}
